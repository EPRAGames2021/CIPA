using UnityEngine;
using Cinemachine;
using EPRA.Utilities;

namespace CIPA
{
    public class ResourceTreatmentController : MonoBehaviour
    {
        [SerializeField] private GameObject _minigameUI;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        [SerializeField] private Player _player;

        private void OnEnable()
        {
            _minigameUI.SetActive(false);
            _virtualCamera.gameObject.SetActive(false);

            CanvasManager.Instance.EnableHUD(true);
            CanvasManager.Instance.EnableVirtualJoystick(true);

            CustomGameEvents.OnMinigameStarted += StartMiniGame;
            CustomGameEvents.OnMinigameEnded += EndMiniGame;
        }

        private void OnDisable()
        {
            CustomGameEvents.OnMinigameStarted -= StartMiniGame;
            CustomGameEvents.OnMinigameEnded -= EndMiniGame;
        }


        private void StartMiniGame()
        {
            _minigameUI.SetActive(true);
            _virtualCamera.gameObject.SetActive(true);
            _virtualCamera.Priority = 11;

            _player.ArrowSystem.SetEnabled(false);

            CanvasManager.Instance.EnableHUD(false);
            CanvasManager.Instance.EnableVirtualJoystick(false);
        }

        private void EndMiniGame()
        {
            _minigameUI.SetActive(false);
        }
    }
}
