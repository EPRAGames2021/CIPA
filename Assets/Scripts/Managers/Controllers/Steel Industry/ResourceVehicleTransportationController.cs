using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EPRA.Utilities;

namespace CIPA
{
    public class ResourceVehicleTransportationController : MonoBehaviour
    {
        private Player _player;

        [SerializeField] private Player _vehiclePlayer;
        [SerializeField] private PlayerVehicle _vehicle;

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

            _vehicleInitialPosition = _vehiclePlayer.transform.position;
            _vehicleInitialRotation = _vehiclePlayer.transform.rotation;

            _vehicleVirtualCamera.gameObject.SetActive(false);
            _vehicleArrowSystem.SetEnabled(false);
            _collectingSpot.gameObject.SetActive(false);

            for (int i = 0; i < _deliveringSpotsList.Count; i++)
            {
                _deliveringSpotsList[i].gameObject.SetActive(false);
            }

            CanvasManager.Instance.EnableHUD(true);
            CanvasManager.Instance.EnableVirtualJoystick(true);

            CustomGameEvents.OnMinigameStarted += InitiateMiniGame;
            CustomGameEvents.OnMinigameEnded += EndMiniGame;

            _vehiclePlayer.OnDied += PlayerDied;
            _collectingSpot.OnPlayerVehicleDetected += CollectCargo;
        }

        private void Finish()
        {
            CustomGameEvents.OnMinigameStarted -= InitiateMiniGame;
            CustomGameEvents.OnMinigameEnded -= EndMiniGame;

            _vehiclePlayer.OnDied -= PlayerDied;
            _collectingSpot.OnPlayerVehicleDetected -= CollectCargo;

            ClearDeliverySubs();
        }


        private void InitiateMiniGame()
        {
            ClearDeliverySubs();

            _currentDeliverySpotIndex = 0;

            _minigameTrigger.SetActive(false);

            _player.MovementSystem.StandStill();
            _player.gameObject.SetActive(false);

            _collectingSpot.gameObject.SetActive(true);

            InputHandler.Instance.SetMovementSystem(_vehicleMovementSystem);

            SetupVehicle();
            EnableUI(true);
        }

        private void SetupVehicle()
        {
            _vehicle.SetCarrying(false);

            _vehiclePlayer.gameObject.SetActive(true);
            _vehiclePlayer.transform.SetPositionAndRotation(_vehicleInitialPosition, _vehicleInitialRotation);
            _vehiclePlayer.Refresh();

            _vehicleVirtualCamera.gameObject.SetActive(true);

            _vehicleArrowSystem.SetTarget(_collectingSpot.transform);
            _vehicleArrowSystem.SetEnabled(true);
            _vehicleMovementSystem.StandStill();
            _vehicleMovementSystem.TemporarilyDisableMovement(1f);
        }

        private void EnableUI(bool display)
        {
            CanvasManager.Instance.EnableHUD(display);
            CanvasManager.Instance.EnableVirtualJoystick(display);

            _minigameUI.SetActive(display);
        }

        private void EndMiniGame()
        {
            EnableUI(false);
        }

        private void CollectCargo(PlayerVehicle vehicle)
        {
            vehicle.SetCarrying(true);

            MissionManager.Instance.GoToNextMission();

            _collectingSpot.gameObject.SetActive(false);
            EnableDeliverySpot(_currentDeliverySpotIndex, true);
            _vehicleArrowSystem.SetTarget(_deliveringSpotsList[_currentDeliverySpotIndex].transform);
        }

        private void DeliverCargo(PlayerVehicle vehicle)
        {
            vehicle.SetCarrying(false);

            if (_currentDeliverySpotIndex < _deliveringSpotsList.Count - 1)
            {
                MissionManager.Instance.ReturnToPreviousMission();

                EnableDeliverySpot(_currentDeliverySpotIndex, false);
                _collectingSpot.gameObject.SetActive(true);
                _vehicleArrowSystem.SetTarget(_collectingSpot.transform);

                _currentDeliverySpotIndex++;
            }
            else
            {
                JobAreaManager.Instance.FinishMinigame(true);
            }
        }

        private void EnableDeliverySpot(int index, bool enable)
        {
            _deliveringSpotsList[index].gameObject.SetActive(enable);

            if (enable)
            {
                _deliveringSpotsList[index].OnPlayerVehicleDetected += DeliverCargo;
            }
            else
            {
                _deliveringSpotsList[index].OnPlayerVehicleDetected -= DeliverCargo;
            }
        }

        private void ClearDeliverySubs()
        {
            for (int i = 0; i < _deliveringSpotsList.Count; i++)
            {
                EnableDeliverySpot(i, false);
            }
        }

        private void PlayerDied()
        {
            JobAreaManager.Instance.PlayerDied();
        }
    }
}
