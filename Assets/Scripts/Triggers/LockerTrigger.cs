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
        _playerDetector.OnPlayerDetected += InitiateMinigame;
    }

    private void Finish()
    {
        _playerDetector.OnPlayerDetected -= InitiateMinigame;
    }


    private void InitiateMinigame()
    {
        _player.Equip(true);
    }
}
