using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _playerDetector.OnPlayerDetected += InitiateEquipmentSelection;
    }

    private void Finish()
    {
        _playerDetector.OnPlayerDetected -= InitiateEquipmentSelection;
    }


    private void InitiateEquipmentSelection()
    {
        CanvasManager.Instance.OpenMenu(MenuType.PPESelectionMenu);
        CanvasManager.Instance.SetHudEnabled(false);
        CanvasManager.Instance.FloatingJoystick.gameObject.SetActive(false);

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
        }
        else
        {
            CanvasManager.Instance.SwitchMenu(MenuType.GameOverMenu);
        }

        CanvasManager.Instance.SetHudEnabled(true);
        CanvasManager.Instance.FloatingJoystick.gameObject.SetActive(true);

        PPESelectionMenu.OnSelectionIsCorrect -= EquipPlayer;
        Vibrator.Vibrate(100);
    }
}
