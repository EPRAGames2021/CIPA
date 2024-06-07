using UnityEngine;

namespace EPRA.Utilities
{
    public class PlayerDetector : MonoBehaviour
    {
        public event System.Action<Player> OnPlayerDetected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>() != null)
            {
                Player player = other.GetComponent<Player>();

                OnPlayerDetected?.Invoke(player);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
