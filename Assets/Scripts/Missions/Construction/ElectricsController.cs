using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class ElectricsController : MonoBehaviour
    {
        [SerializeField] private GameObject _spotlight;

        private void OnEnable()
        {
            CanvasManager.Instance.EnableVirtualJoystick(true);
            CanvasManager.Instance.EnableHUD(true);

            CustomGameEvents.OnMinigameStarted += StartMiniGame;

            _spotlight.SetActive(false);
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

            _spotlight.SetActive(true);
        }
    }
}
