using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class SlowDownZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out MovementSystem movementSystem);

            if (movementSystem != null)
            {
                movementSystem.RestrictMovement(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            other.TryGetComponent(out MovementSystem movementSystem);

            if (movementSystem != null)
            {
                movementSystem.RestrictMovement(false);
            }
        }
    }
}
