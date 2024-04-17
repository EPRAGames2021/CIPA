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
        _playerDetector.OnPlayerDetected += EquipPlayer;
    }

    private void Finish()
    {
        _playerDetector.OnPlayerDetected -= EquipPlayer;
    }


    private void EquipPlayer()
    {
        _player.EquipmentSystem.EquipPlayer(true);

        _playerDetector.gameObject.SetActive(false);

        Vibrator.Vibrate(100);
    }
}
