using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EPRA.Utilities;

namespace CIPA
{
    public class TransportingAndStoringController : MonoBehaviour
    {
        [SerializeField] private Player _player;

        [SerializeField] private GameObject _minigameTrigger;
        [SerializeField] private GameObject _minigameUI;

        [SerializeField] private MovementSystem _forkliftMovementSystem;
        [SerializeField] private CinemachineVirtualCamera _forkliftVirtualCamera;
        [SerializeField] private ArrowSystem _forkliftArrowSystem;

        [SerializeField] private PlayerVehicleDetector _collectingSpot;
        [SerializeField] private List<PlayerVehicleDetector> _deliveringSpotsList;

        [SerializeField] private int _currentDeliverySpotIndex;

        public int CurrentDeliverySpotIndex => _currentDeliverySpotIndex;
        public int AmountOfDeliveries => _deliveringSpotsList.Count;


        private void OnEnable()
        {
            CanvasManager.Instance.EnableHUD(true);
            CanvasManager.Instance.EnableVirtualJoystick(true);

            _forkliftVirtualCamera.gameObject.SetActive(false);

            _collectingSpot.OnPlayerVehicleDetected += CollectCargo;

            _currentDeliverySpotIndex = 0;

            CustomGameEvents.OnPlayerWorePPEs += PrepareForklift;
            CustomGameEvents.OnMinigameStarted += TransferControlToForklift;
            CustomGameEvents.OnMinigameEnded += EndMiniGame;
        }

        private void OnDisable()
        {
            _collectingSpot.OnPlayerVehicleDetected -= CollectCargo;

            for (int i = 0; i < _deliveringSpotsList.Capacity; i++)
            {
                _deliveringSpotsList[i].OnPlayerVehicleDetected -= DeliverCargo;
            }

            CustomGameEvents.OnPlayerWorePPEs -= PrepareForklift;
            CustomGameEvents.OnMinigameStarted -= TransferControlToForklift;
            CustomGameEvents.OnMinigameEnded -= EndMiniGame;
        }



        private void PrepareForklift()
        {
            _forkliftArrowSystem.SetEnabled(false);

            _collectingSpot.gameObject.SetActive(true);

            for (int i = 0; i < _deliveringSpotsList.Count; i++)
            {
                _deliveringSpotsList[i].gameObject.SetActive(false);
            }
        }

        private void TransferControlToForklift()
        {
            _player.MovementSystem.StandStill();
            _player.gameObject.SetActive(false);

            _minigameTrigger.SetActive(false);
            _minigameUI.SetActive(true);

            _forkliftVirtualCamera.gameObject.SetActive(true);
            _forkliftVirtualCamera.Priority = 11;

            _forkliftArrowSystem.SetTarget(_collectingSpot.transform);
            _forkliftArrowSystem.SetEnabled(true);

            InputHandler.Instance.SetMovementSystem(_forkliftMovementSystem);
            _forkliftMovementSystem.StandStill();
            _forkliftMovementSystem.TemporarilyDisableMovement(1f);
        }

        private void EndMiniGame()
        {
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
                _forkliftArrowSystem.SetTarget(_collectingSpot.transform);

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

            _forkliftArrowSystem.SetTarget(_deliveringSpotsList[_currentDeliverySpotIndex].transform);
        }
    }
}
