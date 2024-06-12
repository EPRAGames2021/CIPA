using EPRA.Utilities;
using UnityEngine;

namespace CIPA
{
    public class ResourceArrivalController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private MovementSystem _truckMovementSystem;

        [SerializeField] private GameObject _truckArrowSystem;

        [SerializeField] private PlayerVehicleDetector _collectionSpot;
        [SerializeField] private PlayerVehicleDetector _deliveringSpot;

        private void OnEnable()
        {
            CanvasManager.Instance.EnableHUD(true);
            CanvasManager.Instance.EnableVirtualJoystick(true);

            _collectionSpot.OnPlayerVehicleDetected += FillTrunk;
            _deliveringSpot.OnPlayerVehicleDetected += CheckContent;

            CustomGameEvents.OnPlayerWorePPEs += PrepareTruck;
            CustomGameEvents.OnMinigameStarted += TransferControlToTruck;
            CustomGameEvents.OnMinigameEnded += EndMiniGame;
        }

        private void OnDisable()
        {
            _collectionSpot.OnPlayerVehicleDetected += FillTrunk;
            _deliveringSpot.OnPlayerVehicleDetected += CheckContent;

            CustomGameEvents.OnPlayerWorePPEs -= PrepareTruck;
            CustomGameEvents.OnMinigameStarted -= TransferControlToTruck;
            CustomGameEvents.OnMinigameEnded -= EndMiniGame;
        }



        private void PrepareTruck()
        {
            _truckArrowSystem.SetActive(false);
        }

        private void TransferControlToTruck()
        {
            _player.MovementSystem.StandStill();
            _player.gameObject.SetActive(false);

            _truckMovementSystem.StandStill();
            _truckMovementSystem.TemporarilyDisableMovement(0.25f);
            InputHandler.Instance.SetMovementSystem(_truckMovementSystem);
            _truckArrowSystem.SetActive(true);
        }

        private void EndMiniGame()
        {
            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);
        }

        private void FillTrunk(PlayerVehicle vehicle)
        {
            vehicle.SetCarryign(true);

            MissionManager.Instance.GoToNextMission();
        }

        private void CheckContent(PlayerVehicle vehicle)
        {
            JobAreaManager.Instance.FinishMinigame(vehicle.Carrying);
        }
    }
}
