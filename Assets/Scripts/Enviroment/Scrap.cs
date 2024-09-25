using System.Collections;
using UnityEngine;


namespace CIPA
{
    public class Scrap : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;

        [SerializeField] private bool _markedToBeDestroyed;
        [SerializeField] private bool _properlyCollected;

        [SerializeField] private bool _collected;


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
            _collected = false;
        }

        
        public void Collect(bool properlyCollected)
        {
            if (_collected)
            {
                return;
            }

            _collected = true;

            _properlyCollected = properlyCollected;

            OnCollected?.Invoke(this);

            _markedToBeDestroyed = true;

            Destroy(gameObject, properlyCollected ? 0.25f : 2f);
        }
    }
}
