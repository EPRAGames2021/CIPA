using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class InitiateMinigameTrigger : MonoBehaviour
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

            _playerDetector.OnPlayerDetected += HandlePlayerDetection;
        }

        private void Finish()
        {
            _playerDetector.OnPlayerDetected -= HandlePlayerDetection;
        }


        private void HandlePlayerDetection(Player player)
        {
            if (_triggered) return;

            InitiateMinigame();

            _triggered = true;
        }

        private void InitiateMinigame()
        {
            CustomGameEvents.InvokeOnMinigameStarted();
        }
    }
}
