using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class RevestingController : MonoBehaviour
    {
        [SerializeField] private GameObject _minigameUI;

        private void OnEnable()
        {
            _minigameUI.SetActive(false);

            CanvasManager.Instance.EnableVirtualJoystick(true);
            CanvasManager.Instance.EnableHUD(true);

            CustomGameEvents.OnPlayerArrivedAtMinigameLocation += StartMiniGame;
            CustomGameEvents.OnMinigameStarted += ActivateUI;
        }

        private void OnDisable()
        {
            CustomGameEvents.OnPlayerArrivedAtMinigameLocation -= StartMiniGame;
            CustomGameEvents.OnMinigameStarted -= ActivateUI;
        }


        private void StartMiniGame()
        {
            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);

            JobAreaManager.Instance.ArrivedAtMinigameLocation = true;
        }

        private void ActivateUI()
        {
            _minigameUI.SetActive(true);
        }
    }
}
