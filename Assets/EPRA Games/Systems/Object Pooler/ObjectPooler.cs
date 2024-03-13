using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    public interface IPooledObjects
    {
        public void OnObjectSpawn();
        public bool PersistentToRepool { get; set; }
    }


    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;

        public Pool(GameObject _prefab, int _size)
        {
            prefab = _prefab;
            size = _size;
        }
    }


    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler Instance { get; private set; }

        public List<Pool> pools;
        public Dictionary<GameObject, Queue<GameObject>> poolDictionary;


        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                GeneratePool(pool);
            }
        }


        private void InitSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public void AddPool(Pool pool)
        {
            AddPool(pool.prefab, pool.size);
        }

        public void AddPool(GameObject prefab, int size)
        {
            if (poolDictionary.ContainsKey(prefab))
            {
                Debug.Log("Pool with GameObject " + prefab + " already exist. Extending it by " + size);

                foreach (Pool pool in pools)
                {
                    if (pool.prefab == prefab)
                    {
                        ExtendPool(pool, size);

                        return;
                    }
                }
            }
            else
            {
                Pool pool = new(prefab, size);

                pools.Add(pool);

                GeneratePool(pool);
            }
        }

        public void RemovePool(Pool pool)
        {
            RemovePool(pool.prefab);
        }

        public void RemovePool(GameObject _prefab)
        {
            if (!poolDictionary.ContainsKey(_prefab))
            {
                Debug.Log("Pool with GameObject " + _prefab + " doesn't exist. Can't remove it.");
            }
            else
            {
                foreach (Pool pool in pools)
                {
                    if (pool.prefab == _prefab)
                    {
                        pools.Remove(pool);

                        DestroyPool(pool);

                        return;
                    }
                }
            }
        }

        public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return SpawnFromPool(prefab, position, rotation, transform);
        }

        public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!poolDictionary.ContainsKey(prefab))
            {
                Debug.LogWarning("Pool with GameObject " + prefab + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[prefab].Dequeue();

            if (objectToSpawn.TryGetComponent<IPooledObjects>(out var pooledObject))
            {
                if (pooledObject.PersistentToRepool)
                {
                    Debug.Log("Object pooler tried to spawn an object marked as persistent. Going to the next");

                    poolDictionary[prefab].Enqueue(objectToSpawn);

                    SpawnFromPool(prefab, position, rotation, parent);
                }
                else
                {
                    objectToSpawn.SetActive(true);
                    objectToSpawn.transform.SetPositionAndRotation(position, rotation);
                    objectToSpawn.transform.parent = parent;

                    pooledObject.OnObjectSpawn();

                    poolDictionary[prefab].Enqueue(objectToSpawn);
                }
            }

            return objectToSpawn;
        }


        private void GeneratePool(Pool pool)
        {
            Queue<GameObject> objectPool = new();

            if (pool.size < 1) pool.size = 1;

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = this.transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.prefab, objectPool);
        }

        private void ExtendPool(Pool pool, int size)
        {
            for (int i = 0; i < size; i++)
            {
                GameObject objectToAdd = Instantiate(pool.prefab);

                objectToAdd.transform.parent = this.transform;
                objectToAdd.SetActive(false);

                poolDictionary[pool.prefab].Enqueue(objectToAdd);

                pool.size++;
            }
        }

        private void DestroyPool(Pool pool)
        {
            for (int i = 0; i < pool.size; i++)
            {
                GameObject objectToDestroy = poolDictionary[pool.prefab].Dequeue();

                Destroy(objectToDestroy);
            }

            poolDictionary.Remove(pool.prefab);
        }
    }
}