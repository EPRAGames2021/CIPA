using EPRA.Utilities;
using UnityEngine;
using Cinemachine;

namespace CIPA
{
    public class ResourceArrivalController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private MovementSystem _truckMovementSystem;
        [SerializeField] private CinemachineVirtualCamera _truckVirtualCamera;

        [SerializeField] private GameObject _truckArrowSystem;

        [SerializeField] private PlayerVehicleDetector _collectionSpot;
        [SerializeField] private PlayerVehicleDetector _deliveringSpot;

        private void OnEnable()
        {
            CanvasManager.Instance.EnableHUD(true);
            CanvasManager.Instance.EnableVirtualJoystick(true);

            _truckVirtualCamera.gameObject.SetActive(false);

            _collectionSpot.OnPlayerVehicleDetected += FillTrunk;
            _deliveringSpot.OnPlayerVehicleDetected += CheckContent;

            CustomGameEvents.OnPlayerWorePPEs += PrepareTruck;
            CustomGameEvents.OnMinigameStarted += TransferControlToTruck;
            CustomGameEvents.OnMinigameEnded += EndMiniGame;
        }

        private void OnDisable()
        {
            _collectionSpot.OnPlayerVehicleDetected -= FillTrunk;
            _deliveringSpot.OnPlayerVehicleDetected -= CheckContent;

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

            _truckVirtualCamera.gameObject.SetActive(true);
            _truckVirtualCamera.Priority = 11;

            _truckMovementSystem.StandStill();
            _truckMovementSystem.TemporarilyDisableMovement(1f);
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

            _truckArrowSystem.SetActive(false);
        }
    }
}
