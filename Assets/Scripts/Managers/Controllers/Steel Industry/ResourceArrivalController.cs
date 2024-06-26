using EPRA.Utilities;
using UnityEngine;
using Cinemachine;

namespace CIPA
{
    public class ResourceArrivalController : MonoBehaviour
    {
        [SerializeField] private Player _player;

        [SerializeField] private GameObject _minigameTrigger;

        [SerializeField] private MovementSystem _truckMovementSystem;
        [SerializeField] private CinemachineVirtualCamera _truckVirtualCamera;
        [SerializeField] private ArrowSystem _truckArrowSystem;

        [SerializeField] private PlayerVehicleDetector _collectingSpot;
        [SerializeField] private PlayerVehicleDetector _deliveringSpot;

        private void OnEnable()
        {
            CanvasManager.Instance.EnableHUD(true);
            CanvasManager.Instance.EnableVirtualJoystick(true);

            _truckVirtualCamera.gameObject.SetActive(false);

            _collectingSpot.OnPlayerVehicleDetected += FillTrunk;

            CustomGameEvents.OnPlayerWorePPEs += PrepareTruck;
            CustomGameEvents.OnMinigameStarted += TransferControlToTruck;
            CustomGameEvents.OnMinigameEnded += EndMiniGame;
        }

        private void OnDisable()
        {
            _collectingSpot.OnPlayerVehicleDetected -= FillTrunk;
            _deliveringSpot.OnPlayerVehicleDetected -= CheckContent;

            CustomGameEvents.OnPlayerWorePPEs -= PrepareTruck;
            CustomGameEvents.OnMinigameStarted -= TransferControlToTruck;
            CustomGameEvents.OnMinigameEnded -= EndMiniGame;
        }



        private void PrepareTruck()
        {
            _truckArrowSystem.SetEnabled(false);

            _collectingSpot.gameObject.SetActive(false);
            _deliveringSpot.gameObject.SetActive(false);
        }

        private void TransferControlToTruck()
        {
            _player.MovementSystem.StandStill();
            _player.gameObject.SetActive(false);

            _minigameTrigger.SetActive(false);
            _collectingSpot.gameObject.SetActive(true);

            _truckVirtualCamera.gameObject.SetActive(true);
            _truckVirtualCamera.Priority = 11;

            _truckArrowSystem.SetTarget(_collectingSpot.transform);

            _truckMovementSystem.StandStill();
            _truckMovementSystem.TemporarilyDisableMovement(1f);
            InputHandler.Instance.SetMovementSystem(_truckMovementSystem);

            _truckArrowSystem.SetEnabled(true);
        }

        private void EndMiniGame()
        {
            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);
        }

        private void FillTrunk(PlayerVehicle vehicle)
        {
            vehicle.SetCarrying(true);

            MissionManager.Instance.GoToNextMission();

            _collectingSpot.gameObject.SetActive(false);
            _deliveringSpot.gameObject.SetActive(true);

            _truckArrowSystem.SetTarget(_deliveringSpot.transform);

            _deliveringSpot.OnPlayerVehicleDetected += CheckContent;
        }

        private void CheckContent(PlayerVehicle vehicle)
        {
            JobAreaManager.Instance.FinishMinigame(vehicle.Carrying);

            _truckArrowSystem.SetEnabled(false);
        }
    }
}
