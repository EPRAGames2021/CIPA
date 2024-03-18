using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficCone : MonoBehaviour
{
    [SerializeField] private bool _displaced;

    public event System.Action<TrafficCone> OnDisplaced;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (!_displaced)
            {
                OnDisplaced?.Invoke(this);

                _displaced = true;
            }
        }
    }
}
