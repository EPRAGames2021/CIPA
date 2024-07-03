using UnityEngine;
using Cinemachine;
using EPRA.Utilities;
using System.Collections;

namespace CIPA
{
    public class PlateAndCoilProductionController : MonoBehaviour
    {
        [SerializeField] private GameObject _minigameUI;
        [SerializeField] private CinemachineVirtualCamera _camera;

        [SerializeField] private PlateAndCoilGrid _plateAndCoilGrid;

        private void OnEnable()
        {
            _minigameUI.SetActive(false);
            _camera.gameObject.SetActive(false);

            CanvasManager.Instance.EnableVirtualJoystick(true);
            CanvasManager.Instance.EnableHUD(true);

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
            _camera.gameObject.SetActive(true);

            _plateAndCoilGrid.ResetGrid();
            _plateAndCoilGrid.Filling = false;
            StartCoroutine(StartFillingDelay());

            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);

            Camera.main.orthographic = true;
        }

        private IEnumerator StartFillingDelay()
        {
            yield return new WaitForSeconds(1);

            _plateAndCoilGrid.Filling = true;
        }

        private void EndMiniGame()
        {
            Camera.main.orthographic = false;
        }
    }
}
