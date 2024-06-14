using UnityEngine;
using Cinemachine;
using EPRA.Utilities;

namespace CIPA
{
    public class PlateAndCoilProductionController : MonoBehaviour
    {
        [SerializeField] private GameObject _minigameUI;
        [SerializeField] private CinemachineVirtualCamera _camera;

        private void OnEnable()
        {
            _minigameUI.SetActive(false);
            _camera.gameObject.SetActive(false);

            CanvasManager.Instance.EnableVirtualJoystick(true);
            CanvasManager.Instance.EnableHUD(true);

            CustomGameEvents.OnMinigameStarted += StartMiniGame;
        }

        private void OnDisable()
        {
            CustomGameEvents.OnMinigameStarted -= StartMiniGame;
        }


        private void StartMiniGame()
        {
            _minigameUI.SetActive(true);
            _camera.gameObject.SetActive(true);

            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);
        }
    }
}
