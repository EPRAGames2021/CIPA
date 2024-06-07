using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class InitiateMinigameTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerDetector _playerDetector;

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
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
            InitiateMinigame();
        }

        private void InitiateMinigame()
        {
            JobAreaManager.Instance.InitiateMinigameProcess();
        }
    }
}
