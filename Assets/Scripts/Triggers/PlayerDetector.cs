using UnityEngine;

namespace CIPA
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private bool _trigerred;

        [SerializeField] private bool _canBeTriggeredInfiniteTimes;

        public event System.Action<Player> OnPlayerDetected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>() != null && !_trigerred)
            {
                Player player = other.GetComponent<Player>();

                OnPlayerDetected?.Invoke(player);

                if (!_canBeTriggeredInfiniteTimes)
                {
                    _trigerred = true;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
