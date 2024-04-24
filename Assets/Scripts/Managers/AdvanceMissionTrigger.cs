using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    public class AdvanceMissionTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerDetector _playerDetector;

        [SerializeField] private bool _triggered;

        [Tooltip("Should it just go to the next mission?")]
        [SerializeField] private bool _simpleAdvance;

        [SerializeField] private int _missionToGoTo;


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

            if (_simpleAdvance)
            {
                MissionManager.Instance.GoToNextMission();
            }
            else
            {
                MissionManager.Instance.GoToMission(_missionToGoTo);
            }

            _triggered = true;
        }
    }
}
