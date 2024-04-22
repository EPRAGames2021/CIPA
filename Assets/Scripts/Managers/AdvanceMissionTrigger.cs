using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    public class AdvanceMissionTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerDetector _playerDetector;

        [SerializeField] private bool _triggered;


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
            _triggered = false;

            _playerDetector.OnPlayerDetected += AdvanceMission;
        }

        private void Finish()
        {
            _playerDetector.OnPlayerDetected -= AdvanceMission;
        }


        private void AdvanceMission()
        {
            if (_triggered)
            {
                return;
            }

            MissionManager.Instance.MissionCompleted();

            _triggered = true;
        }
    }
}
