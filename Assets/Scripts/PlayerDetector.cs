using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    public class PlayerDetector : MonoBehaviour
    {
        public event System.Action OnPlayerDetected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<HealthSystem>() != null)
            {
                OnPlayerDetected?.Invoke();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
