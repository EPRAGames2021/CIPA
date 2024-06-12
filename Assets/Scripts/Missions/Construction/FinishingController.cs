using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class FinishingController : MonoBehaviour
    {
        private void OnEnable()
        {
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
            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);

            JobAreaManager.Instance.ArrivedAtMinigameLocation = true;
        }
    }
}
