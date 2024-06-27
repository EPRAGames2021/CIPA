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

        public static event System.Action<BumpableObject, Player> OnHasBeenHitByPlayer;


        private void OnValidate()
        {
            if (_collider == null) _collider = GetComponent<Collider>();
        }

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            Finish();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_hasBeenHit)
            {
                if (other.GetComponent<Player>() != null)
                {
                    Player player = other.GetComponent<Player>();

                    OnHasBeenHitByPlayer?.Invoke(this, player);

                    _hasBeenHit = true;
                }
            }
        }


        private void Init()
        {
            //this may be hard to expand in the future. Ideally this refresh comes from the outside
            CustomGameEvents.OnMinigameStarted += Refresh;

            _hasBeenHit = false;

            _collider.isTrigger = true;
        }

        private void Finish()
        {
            CustomGameEvents.OnMinigameStarted -= Refresh;
        }

        public void Refresh()
        {
            _hasBeenHit = false;

            _collider.isTrigger = true;
        }
    }
}
