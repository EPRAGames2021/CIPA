using UnityEngine;

namespace CIPA
{
    public class PlayerVehicleDetector : MonoBehaviour
    {
        [SerializeField] private bool _trigerred;

        [SerializeField] private bool _canBeTriggeredInfiniteTimes;

        public event System.Action<PlayerVehicle> OnPlayerVehicleDetected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerVehicle>() != null && !_trigerred)
            {
                PlayerVehicle vehicle = other.GetComponent<PlayerVehicle>();

                OnPlayerVehicleDetected?.Invoke(vehicle);

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
