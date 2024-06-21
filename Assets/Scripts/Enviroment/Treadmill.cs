using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class Treadmill : MonoBehaviour
    {
        [SerializeField] private Transform _scrapSpawnPoint;
        [SerializeField] private List<Scrap> _scrapsPrefabs;

        [Min(0.1f)]
        [Tooltip("Measured in seconds")]
        [SerializeField] private float _spawnFrequency;
        private Coroutine _spawnNewScrap;

        [SerializeField] private float _speed;
        [SerializeField] private bool _shouldRun;
        private bool _isRunning;
        
        [SerializeField] private List<Rigidbody> _bodies;

        public bool Active { get { return _isRunning; } set { _shouldRun = value; } }


        public event System.Action<Scrap> OnScrapSpawned;


        private void Awake()
        {
            Init();
        }

        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out Rigidbody rigidbody);

            if (rigidbody != null && !_bodies.Contains(rigidbody))
            {
                _bodies.Add(rigidbody);
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            other.TryGetComponent(out Rigidbody rb);

            if (rb != null && _bodies.Contains(rb))
            {
                _bodies.Remove(rb);
            }
        }
    
        private void Update()
        {
            SpawningBehaviour();
            RunningBehaviour();
        }
    


        private void Init()
        {
            _spawnNewScrap = null;
        }

        private IEnumerator SpawnNewScrap()
        {
            yield return new WaitForSeconds(_spawnFrequency);

            int randomIndex = Random.Range(0, _scrapsPrefabs.Count);

            GameObject scrapObject = Instantiate(_scrapsPrefabs[randomIndex].gameObject, _scrapSpawnPoint);

            scrapObject.TryGetComponent(out Scrap newScrap);

            if (newScrap != null)
            {
                OnScrapSpawned?.Invoke(newScrap);
            }

            _spawnNewScrap = null;
        }

        private void SpawningBehaviour()
        {
            if (_spawnNewScrap == null && _isRunning)
            {
                _spawnNewScrap = StartCoroutine(SpawnNewScrap());
            }
        }

        private void RunningBehaviour()
        {
            if (_isRunning && !_shouldRun)
            {
                _isRunning = false;
            }
            else if (!_isRunning && _shouldRun)
            {
                _isRunning = true;
            }

            if (_isRunning)
            {
                foreach (Rigidbody rigidbody in _bodies)
                {
                    rigidbody.velocity = new Vector3(0, 0, -1) * _speed;
                }
            }
        }
    }
}
