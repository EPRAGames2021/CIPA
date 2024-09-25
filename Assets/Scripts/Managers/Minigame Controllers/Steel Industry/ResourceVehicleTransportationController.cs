using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EPRA.Utilities;
using System.Collections;

namespace CIPA
{
    public class ResourceVehicleTransportationController : MonoBehaviour
    {
        private Player _player;

        [SerializeField] private Player _vehiclePlayer;
        [SerializeField] protected PlayerVehicle _vehicle;

        private Vector3 _vehicleInitialPosition;
        private Quaternion _vehicleInitialRotation;

        [SerializeField] private GameObject _minigameTrigger;
        [SerializeField] private GameObject _minigameUI;

        [SerializeField] private MovementSystem _vehicleMovementSystem;
        [SerializeField] private CinemachineVirtualCamera _vehicleVirtualCamera;
        [SerializeField] protected ArrowSystem _vehicleArrowSystem;

        [SerializeField] private PlayerVehicleDetector _collectingSpot;
        [SerializeField] protected List<PlayerVehicleDetector> _deliveringSpotsList;

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


        protected virtual void Init()
        {
            _player = JobAreaManager.Instance.Player;

            _vehicleInitialPosition = _vehiclePlayer.transform.position;
            _vehicleInitialRotation = _vehiclePlayer.transform.rotation;

            _vehicleVirtualCamera.gameObject.SetActive(false);
            _vehicleArrowSystem.SetEnabled(false);
            _collectingSpot.gameObject.SetActive(false);

            SetAllDeliverySubsActive(false);

            CanvasManager.Instance.EnableHUD(true);
            CanvasManager.Instance.EnableVirtualJoystick(true);

            CustomGameEvents.OnMinigameStarted += InitiateMiniGame;
            CustomGameEvents.OnMinigameEnded += EndMiniGame;
        }

        protected virtual void Finish()
        {
            CustomGameEvents.OnMinigameStarted -= InitiateMiniGame;
            CustomGameEvents.OnMinigameEnded -= EndMiniGame;

            SetAllDeliverySubsActive(false);
        }


        protected virtual void InitiateMiniGame()
        {
            _vehiclePlayer.OnDied += VehicleCrashed;
            _vehicle.OnCarryingChanged += UpdateCargo;


            SetAllDeliverySubsActive(false);

            _currentDeliverySpotIndex = 0;

            _minigameTrigger.SetActive(false);

            _player.MovementSystem.StandStill();
            _player.gameObject.SetActive(false);

            _collectingSpot.gameObject.SetActive(true);

            SetupVehicle();
            InputHandler.Instance.SetMovementSystem(_vehicleMovementSystem);

            EnableUI(true);
        }

        protected virtual void SetupVehicle()
        {
            _vehicle.SetCarrying(CargoType.None, false);

            _vehiclePlayer.gameObject.SetActive(false);
            _vehicleMovementSystem.StandStill();
            _vehiclePlayer.Refresh();
            _vehiclePlayer.transform.SetPositionAndRotation(_vehicleInitialPosition, _vehicleInitialRotation);
            _vehiclePlayer.gameObject.SetActive(true);

            _vehicleVirtualCamera.gameObject.SetActive(true);

            _vehicleArrowSystem.SetTarget(_collectingSpot.transform);
            _vehicleArrowSystem.SetEnabled(true);
        }

        private void EnableUI(bool display)
        {
            CanvasManager.Instance.EnableHUD(display);
            CanvasManager.Instance.EnableVirtualJoystick(display);

            StartCoroutine(OpenMenuDelay());
            IEnumerator OpenMenuDelay()
            {
                yield return new WaitForSeconds(0.5f);
                _minigameUI.SetActive(display);
            }
        }

        private void EndMiniGame()
        {
            _vehiclePlayer.OnDied -= VehicleCrashed;
            _vehicle.OnCarryingChanged -= UpdateCargo;

            EnableUI(false);

            _player.gameObject.SetActive(true);
            _player.ArrowSystem.SetEnabled(false);

            _vehicleMovementSystem.CanMove = false;
            InputHandler.Instance.SetMovementSystem(_player.MovementSystem);

            _vehicleVirtualCamera.gameObject.SetActive(false);
        }

        private void UpdateCargo(bool enableCarrying)
        {
            if (enableCarrying)
            {
                CollectCargo();
            }
            else
            {
                DeliverCargo();
            }
        }

        protected virtual void CollectCargo()
        {
            MissionManager.Instance.GoToNextMission();

            _collectingSpot.gameObject.SetActive(false);
            _deliveringSpotsList[_currentDeliverySpotIndex].gameObject.SetActive(true);
            _vehicleArrowSystem.SetTarget(_deliveringSpotsList[_currentDeliverySpotIndex].transform);
        }

        protected virtual void DeliverCargo()
        {
            if (_currentDeliverySpotIndex < _deliveringSpotsList.Count - 1)
            {
                MissionManager.Instance.ReturnToPreviousMission();

                _deliveringSpotsList[_currentDeliverySpotIndex].gameObject.SetActive(false);
                _collectingSpot.gameObject.SetActive(true);
                _vehicleArrowSystem.SetTarget(_collectingSpot.transform);

                _currentDeliverySpotIndex++;
            }
            else
            {
                JobAreaManager.Instance.FinishMinigame(true);
            }
        }

        protected void SetAllDeliverySubsActive(bool active)
        {
            for (int i = 0; i < _deliveringSpotsList.Count; i++)
            {
                _deliveringSpotsList[i].gameObject.SetActive(active);
            }
        }

        private void VehicleCrashed()
        {
            JobAreaManager.Instance.PlayerDied();
        }
    }
}
