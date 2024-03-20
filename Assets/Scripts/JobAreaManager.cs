using Cinemachine;
using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobAreaManager : MonoBehaviour
{
    [SerializeField] private JobSectorAreaSO _jobSectorSO;

    [SerializeField] private List<GameObject> _minigameContextObjects;
    [SerializeField] private List<GameObject> _minigamesUIs;
    [SerializeField] private CinemachineVirtualCamera _minigameCamera;
    [SerializeField] private PlayerDetector _playerDetector;

    [SerializeField] private Player _player;

    [SerializeField] private List<TrafficCone> _trafficConeList;

    [SerializeField] private CurrencySO _dayScore;

    [Header("Scores and penalties")]
    [SerializeField] private int _equipEquipmentScore;
    [SerializeField] private int _hitTrafficConePenalty;


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
        _dayScore.SetCurrencyValue(0);
        
        GameManager.Instance.UpdateGameState(GameState.GameState);
        CanvasManager.Instance.GameScreen.SetDay(_jobSectorSO.Day);

        for (int i = 0; i < _minigameContextObjects.Count; i++)
        {
            _minigameContextObjects[i].SetActive(i == _jobSectorSO.Day);
        }

        for (int i = 0; i < _minigamesUIs.Count; i++)
        {
            _minigamesUIs[i].SetActive(false);
        }


        _playerDetector.OnPlayerDetected += InitiateMinigame;

        _player.HealthSystem.OnDied += PlayerDied;
        _player.OnEquip += EquipPlayer;

        foreach (TrafficCone trafficCone in _trafficConeList)
        {
            trafficCone.OnDisplaced += PlayerHitTrafficCone;
        }
    }

    private void Finish()
    {
        _playerDetector.OnPlayerDetected -= InitiateMinigame;

        _player.HealthSystem.OnDied -= PlayerDied;
        _player.OnEquip -= EquipPlayer;

        foreach (TrafficCone trafficCone in _trafficConeList)
        {
            trafficCone.OnDisplaced -= PlayerHitTrafficCone;
        }
    }


    private void PlayerDied()
    {
        CanvasManager.Instance.OpenMenu(MenuType.GameOverMenu);
    }

    private void EquipPlayer(bool equip)
    {
        if (equip)
        {
            _dayScore.AddToCurrency(_equipEquipmentScore);
        }
    }

    private void PlayerHitTrafficCone(TrafficCone trafficCone)
    {
        _dayScore.RemoveFromCurrency(_hitTrafficConePenalty);

        for (int i = 0; i < _trafficConeList.Count; i++)
        {
            if (_trafficConeList[i] == trafficCone)
            {
                trafficCone.OnDisplaced -= PlayerHitTrafficCone;
            }
        }
    }

    private void InitiateMinigame()
    {
        GameManager.Instance.UpdateGameState(GameState.MiniGameState);

        _minigameCamera.Priority = 10;

        for (int i = 0; i < _minigamesUIs.Count; i++)
        {
            _minigamesUIs[i].SetActive(i == _jobSectorSO.Day);
        }
    }

}
