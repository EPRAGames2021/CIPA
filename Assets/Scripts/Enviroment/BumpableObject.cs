using UnityEngine;

namespace CIPA
{
    [RequireComponent(typeof(Collider))]
    public class BumpableObject : MonoBehaviour
    {
        [SerializeField] private bool _hasBeenHit;
        [SerializeField] private bool _hitIsFatal;
        [SerializeField] private Collider _collider;

        public bool HitIsFatal => _hitIsFatal;

        public static event System.Action<BumpableObject> OnHasBeenHitByPlayer;


        private void OnValidate()
        {
            if (_collider == null) _collider = GetComponent<Collider>();
        }

        private void Awake()
        {
            Init();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_hasBeenHit)
            {
                if (other.GetComponent<Player>() != null)
                {
                    OnHasBeenHitByPlayer?.Invoke(this);

                    _hasBeenHit = true;
                }
                else if (other.GetComponent<PlayerVehicle>() != null)
                {
                    OnHasBeenHitByPlayer?.Invoke(this);

                    _hasBeenHit = true;
                }
            }
        }


        private void Init()
        {
            _hasBeenHit = false;

            _collider.isTrigger = true;
        }

        public void Refresh()
        {
            Init();
        }
    }
}
