using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EPRA.Utilities;

public class JobAreaManager : MonoBehaviour
{
    public static JobAreaManager Instance { get; private set; }

    [SerializeField] private JobSectorAreaSO _jobSectorSO;

    [SerializeField] private List<GameObject> _minigameContextObjects;
    [SerializeField] private List<GameObject> _minigamesUIs;
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private PlayerDetector _playerDetector;

    [SerializeField] private Player _player;

    [SerializeField] private CurrencySO _dayScore;

    [SerializeField] private bool _arrivedAtMinigameLocation;

    [Header("Scores and penalties")]
    [SerializeField] private int _equipEquipmentScore;
    [SerializeField] private int _arriveAtJobAreaScore;
    [SerializeField] private int _completeJobScore;
    [SerializeField] private int _arriveAtJobAreaUnequippedPenalty;
    [SerializeField] private int _failJobPenalty;

    [Header("Sounds")]
    [SerializeField] private AudioClipCollection _victorySFX;
    [SerializeField] private AudioClipCollection _defeatSFX;
    [SerializeField] private AudioClipCollection _deathSFX;


    public JobSectorAreaSO JobSectorAreaSO => _jobSectorSO;


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

        _arrivedAtMinigameLocation = false;
        
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


        _playerDetector.OnPlayerDetected += InitiateMinigameProcess;

        _player.HealthSystem.OnDied += PlayerDied;
        _player.EquipmentSystem.OnEquipped += EquipPlayer;
    }

    private void Finish()
    {
        _playerDetector.OnPlayerDetected -= InitiateMinigameProcess;

        _player.HealthSystem.OnDied -= PlayerDied;
        _player.EquipmentSystem.OnEquipped -= EquipPlayer;
    }


    private void PlayerDied()
    {
        CanvasManager.Instance.OpenMenu(MenuType.GameOverMenu);
        AudioManager.Instance.PlayRandomSFX(_deathSFX);
        AudioManager.Instance.PlayRandomSFX(_defeatSFX);
    }

    private void EquipPlayer(bool equip)
    {
        if (equip)
        {
            _dayScore.AddToCurrency(_equipEquipmentScore);
        }
    }

    private void InitiateMinigameProcess()
    {
        _arrivedAtMinigameLocation = true;
        _playerCamera.Priority = 9;

        if (_player.EquipmentSystem.WearingEquipment)
        {
            _dayScore.AddToCurrency(_arriveAtJobAreaScore);
        }
        else
        {
            _dayScore.RemoveFromCurrency(_arriveAtJobAreaUnequippedPenalty);
        }

        InitiateMinigame();
    }

    private void InitiateMinigame()
    {
        GameManager.Instance.UpdateGameState(GameState.MiniGameState);

        for (int i = 0; i < _minigamesUIs.Count; i++)
        {
            _minigamesUIs[i].SetActive(i == _jobSectorSO.Day);
        }
    }

    public void MinigameSuccessed()
    {
        _dayScore.AddToCurrency(_completeJobScore);

        _jobSectorSO.SetScoreToDay(_jobSectorSO.Day, _dayScore.Value);

        GameManager.Instance.UpdateGameState(GameState.PausedState);
        CanvasManager.Instance.OpenMenu(MenuType.VictoryMenu);
        CanvasManager.Instance.OpenMenu(MenuType.DayScoreMenu);

        Vibrator.Vibrate(100);
        AudioManager.Instance.PlayRandomSFX(_victorySFX);

        _jobSectorSO.FinishDay();
    }

    public void MinigameFailed()
    {
        _dayScore.RemoveFromCurrency(_failJobPenalty);

        GameManager.Instance.UpdateGameState(GameState.PausedState);
        CanvasManager.Instance.OpenMenu(MenuType.GameOverMenu);

        Vibrator.Vibrate(100);
        AudioManager.Instance.PlayRandomSFX(_defeatSFX);
    }

    public void RestartJob()
    {
        if (_arrivedAtMinigameLocation)
        {
            InitiateMinigame();
        }
        else
        {
            SceneLoader.Instance.ReloadLevel();
        }
    }

}
