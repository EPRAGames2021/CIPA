using System.Collections;
using UnityEngine;


namespace CIPA
{
    public class Scrap : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;

        [SerializeField] private bool _markedToBeDestroyed;
        [SerializeField] private bool _properlyCollected;


        public bool MarkedToBeDestroyed => _markedToBeDestroyed;
        public bool ProperlyCollected => _properlyCollected;



        public event System.Action<Scrap> OnCollected;


        private void Awake()
        {
            Init();
        }


        private void Init()
        {
            _properlyCollected = false;
        }

        
        public void Collect(bool collected)
        {
            _properlyCollected = collected;

            OnCollected?.Invoke(this);

            _markedToBeDestroyed = true;

            Destroy(gameObject, collected ? 0.25f : 2f);
        }
    }
}
