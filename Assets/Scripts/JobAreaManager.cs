using Cinemachine;
using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobAreaManager : MonoBehaviour
{
    public static JobAreaManager Instance { get; private set; }

    [SerializeField] private JobSectorAreaSO _jobSectorSO;

    [SerializeField] private List<GameObject> _minigameContextObjects;
    [SerializeField] private List<GameObject> _minigamesUIs;
    [SerializeField] private CinemachineVirtualCamera _minigameCamera;
    [SerializeField] private PlayerDetector _playerDetector;

    [SerializeField] private Player _player;

    [SerializeField] private List<TrafficCone> _trafficConeList;

    [SerializeField] private CurrencySO _dayScore;
    [SerializeField] private DayScoreListSO _dayScoreList;

    [Header("Scores and penalties")]
    [SerializeField] private int _equipEquipmentScore;
    [SerializeField] private int _arriveAtJobAreaScore;
    [SerializeField] private int _completeJobScore;
    [SerializeField] private int _arriveAtJobAreaUnequippedPenalty;
    [SerializeField] private int _hitTrafficConePenalty;
    [SerializeField] private int _failJobPenalty;


    public JobSectorAreaSO JobSectorAreaSO => _jobSectorSO;
    public DayScoreListSO DayScoreList => _dayScoreList;


    private void Awake()
    {
        InitSingleton();
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Finish();
    }


    private void InitSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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

        if (_player.WearingEquipment)
        {
            _dayScore.AddToCurrency(_arriveAtJobAreaScore);
        }
        else
        {
            _dayScore.RemoveFromCurrency(_arriveAtJobAreaUnequippedPenalty);
        }

        _minigameCamera.Priority = 10;

        for (int i = 0; i < _minigamesUIs.Count; i++)
        {
            _minigamesUIs[i].SetActive(i == _jobSectorSO.Day);
        }
    }

    public void MinigameSuccessed()
    {
        Debug.Log("Congrats");

        _dayScore.AddToCurrency(_completeJobScore);

        _dayScoreList.SetScoreToDay(_jobSectorSO.Day, _dayScore.Value);

        GameManager.Instance.UpdateGameState(GameState.PausedState);
        CanvasManager.Instance.OpenMenu(MenuType.VictoryMenu);
        CanvasManager.Instance.OpenMenu(MenuType.DayScoreMenu);

        _jobSectorSO.FinishDay();
    }

    public void MinigameFailed()
    {
        Debug.Log("Sorry bro");

        _dayScore.RemoveFromCurrency(_failJobPenalty);

        GameManager.Instance.UpdateGameState(GameState.PausedState);
        CanvasManager.Instance.OpenMenu(MenuType.GameOverMenu);
    }

}
