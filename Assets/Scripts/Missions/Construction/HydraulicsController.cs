using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class HydraulicsController : MonoBehaviour
    {
        [SerializeField] private GameObject _minigameUI;
        [SerializeField] private GameObject _spotlight;

        private void OnEnable()
        {
            _minigameUI.SetActive(false);

            CanvasManager.Instance.EnableVirtualJoystick(true);
            CanvasManager.Instance.EnableHUD(true);

            CustomGameEvents.OnPlayerArrivedAtMinigameLocation += StartMiniGame;
            CustomGameEvents.OnMinigameStarted += ActivateUI;

            _spotlight.SetActive(false);
        }

        private void OnDisable()
        {
            CustomGameEvents.OnPlayerArrivedAtMinigameLocation -= StartMiniGame;
            CustomGameEvents.OnMinigameStarted -= ActivateUI;
        }


        private void StartMiniGame()
        {
            _minigameUI.SetActive(true);

            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);

            JobAreaManager.Instance.ArrivedAtMinigameLocation = true;

            _spotlight.SetActive(true);
        }

        private void ActivateUI()
        {
            _minigameUI.SetActive(true);
        }
    }
}
