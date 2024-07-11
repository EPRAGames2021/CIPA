using System.Collections.Generic;
using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class LockerTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerDetector _playerDetector;

        private Player _player;

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void Init()
        {
            _playerDetector.OnPlayerDetected += HandlePlayerDetection;
        }

        private void Finish()
        {
            _playerDetector.OnPlayerDetected -= HandlePlayerDetection;
        }


        private void HandlePlayerDetection(Player player)
        {
            _player = player;

            InitiateEquipmentSelection();
        }


        private void InitiateEquipmentSelection()
        {
            CanvasManager.Instance.OpenMenu(MenuType.PPESelectionMenu);

            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);

            PPESelectionMenu.OnSelectionIsCorrect += EquipPlayer;

            PlayerCameraHandler.Instance.FocusOnPPEBoard(true);
        }

        private void EquipPlayer(bool equip)
        {
            if (equip)
            {
                CanvasManager.Instance.CloseMenu(MenuType.PPESelectionMenu);
                CanvasManager.Instance.EnableVirtualJoystick(true);
                CanvasManager.Instance.EnableHUD(true);

                MissionManager.Instance.GoToNextMission();
                PlayerCameraHandler.Instance.FocusOnPPEBoard(false);

                List<EquipmentSO> equipmentSOList = JobAreaManager.Instance.JobSectorAreaSO.CurrentJob.RequiredEquipmentSO;
                _player.EquipmentSystem.EquipPlayer(equipmentSOList, true);

                _playerDetector.gameObject.SetActive(false);

                CustomGameEvents.InvokeOnPlayerWorePPEs();
                Vibrator.Vibrate(100);
            }
            else
            {
                CanvasManager.Instance.CloseMenu(MenuType.PPESelectionMenu);
                JobAreaManager.Instance.FinishMinigame(false);
            }

            PPESelectionMenu.OnSelectionIsCorrect -= EquipPlayer;
        }
    }
}
