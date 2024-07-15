using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class DodgeableVehicleTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerDetector _playerDetector;

        [SerializeField] private List<DodgeableTruck> _vehicles;

        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            Finish();
        }


        private void Init()
        {
            _playerDetector.OnPlayerDetected += HandlePlayerDetection;
        }

        private void Finish()
        {
            _playerDetector.OnPlayerDetected -= HandlePlayerDetection;
        }


        private void HandlePlayerDetection(Player player)
        {
            TriggerVehicles();
        }

        private void TriggerVehicles()
        {
            for (int i = 0; i < _vehicles.Count; i++)
            {
                _vehicles[i].InitiateTruckMovement();
            }
        }
    }
}
