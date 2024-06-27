using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EPRA.Utilities;

namespace CIPA
{
    public class ResourceVehicleTransportationController : MonoBehaviour
    {
        private Player _player;
        [SerializeField] private Player _playerVehicle;
        private Vector3 _vehicleInitialPosition;
        private Quaternion _vehicleInitialRotation;

        [SerializeField] private GameObject _minigameTrigger;
        [SerializeField] private GameObject _minigameUI;

        [SerializeField] private MovementSystem _vehicleMovementSystem;
        [SerializeField] private CinemachineVirtualCamera _vehicleVirtualCamera;
        [SerializeField] private ArrowSystem _vehicleArrowSystem;

        [SerializeField] private PlayerVehicleDetector _collectingSpot;
        [SerializeField] private List<PlayerVehicleDetector> _deliveringSpotsList;

        [SerializeField] private int _currentDeliverySpotIndex;

        public int CurrentDeliverySpotIndex => _currentDeliverySpotIndex;
        public int AmountOfDeliveries => _deliveringSpotsList.Count;


        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            Finish();
        }


        private void Init()
        {
            _player = JobAreaManager.Instance.Player;

            _vehicleInitialPosition = _playerVehicle.transform.position;
            _vehicleInitialRotation = _playerVehicle.transform.rotation;

            _vehicleVirtualCamera.gameObject.SetActive(false);
            _vehicleArrowSystem.SetEnabled(false);
            _collectingSpot.gameObject.SetActive(false);

            for (int i = 0; i < _deliveringSpotsList.Count; i++)
            {
                _deliveringSpotsList[i].gameObject.SetActive(false);
            }

            CanvasManager.Instance.EnableHUD(true);
            CanvasManager.Instance.EnableVirtualJoystick(true);

            CustomGameEvents.OnMinigameStarted += TransferControlToVehicle;
            CustomGameEvents.OnMinigameEnded += EndMiniGame;

            _playerVehicle.OnDied += PlayerDied;
            _collectingSpot.OnPlayerVehicleDetected += CollectCargo;
        }

        private void Finish()
        {
            CustomGameEvents.OnMinigameStarted -= TransferControlToVehicle;
            CustomGameEvents.OnMinigameEnded -= EndMiniGame;

            _playerVehicle.OnDied -= PlayerDied;
            _collectingSpot.OnPlayerVehicleDetected -= CollectCargo;

            for (int i = 0; i < _deliveringSpotsList.Capacity; i++)
            {
                _deliveringSpotsList[i].OnPlayerVehicleDetected -= DeliverCargo;
            }
        }

        private void TransferControlToVehicle()
        {
            Debug.Log("transfer");

            _player.MovementSystem.StandStill();
            _player.gameObject.SetActive(false);

            _playerVehicle.gameObject.SetActive(true);
            _playerVehicle.transform.SetPositionAndRotation(_vehicleInitialPosition, _vehicleInitialRotation);
            _playerVehicle.Refresh();

            _currentDeliverySpotIndex = 0;

            _minigameTrigger.SetActive(false);
            _minigameUI.SetActive(true);

            _vehicleVirtualCamera.gameObject.SetActive(true);
            _vehicleVirtualCamera.Priority = 11;

            _collectingSpot.gameObject.SetActive(true);
            _vehicleArrowSystem.SetTarget(_collectingSpot.transform);
            _vehicleArrowSystem.SetEnabled(true);

            InputHandler.Instance.SetMovementSystem(_vehicleMovementSystem);
            _vehicleMovementSystem.StandStill();
            _vehicleMovementSystem.TemporarilyDisableMovement(1f);
        }

        private void EndMiniGame()
        {
            Debug.Log("end");

            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);

            _minigameUI.SetActive(false);
        }

        private void CollectCargo(PlayerVehicle vehicle)
        {
            vehicle.SetCarrying(true);

            _collectingSpot.gameObject.SetActive(false);

            SetupDeliverySpot();

            MissionManager.Instance.GoToNextMission();
        }

        private void DeliverCargo(PlayerVehicle vehicle)
        {
            vehicle.SetCarrying(false);

            if (_currentDeliverySpotIndex < _deliveringSpotsList.Count - 1)
            {
                MissionManager.Instance.ReturnToPreviousMission();

                _collectingSpot.gameObject.SetActive(true);
                _vehicleArrowSystem.SetTarget(_collectingSpot.transform);

                _deliveringSpotsList[_currentDeliverySpotIndex].gameObject.SetActive(false);
                _deliveringSpotsList[_currentDeliverySpotIndex].OnPlayerVehicleDetected -= DeliverCargo;

                _currentDeliverySpotIndex++;
            }
            else
            {
                JobAreaManager.Instance.FinishMinigame(true);
            }
        }

        private void SetupDeliverySpot()
        {
            _deliveringSpotsList[_currentDeliverySpotIndex].gameObject.SetActive(true);
            _deliveringSpotsList[_currentDeliverySpotIndex].OnPlayerVehicleDetected += DeliverCargo;

            _vehicleArrowSystem.SetTarget(_deliveringSpotsList[_currentDeliverySpotIndex].transform);
        }

        private void PlayerDied()
        {
            JobAreaManager.Instance.PlayerDied();
        }
    }
}
