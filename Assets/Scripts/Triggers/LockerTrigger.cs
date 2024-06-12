using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class LockerTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerDetector _playerDetector;

        [SerializeField] private Player _player;

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

                MissionManager.Instance.GoToNextMission();
                PlayerCameraHandler.Instance.FocusOnPPEBoard(false);

                JobSO job = JobAreaManager.Instance.JobSectorAreaSO.CurrentJob;
                _player.EquipmentSystem.EquipPlayer(job.RequiredEquipment, true);

                _playerDetector.gameObject.SetActive(false);

                CustomGameEvents.InvokeOnPlayerWorePPEs();
            }
            else
            {
                CanvasManager.Instance.SwitchMenu(MenuType.GameOverMenu);
            }

            CanvasManager.Instance.EnableVirtualJoystick(true);
            CanvasManager.Instance.EnableHUD(true);

            PPESelectionMenu.OnSelectionIsCorrect -= EquipPlayer;
            Vibrator.Vibrate(100);
        }
    }
}
