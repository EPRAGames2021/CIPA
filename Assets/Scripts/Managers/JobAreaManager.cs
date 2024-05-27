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
        RewardAndPenaltyManager.Instance.ResetScore();

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


        if (_playerDetector == null)
        {
            Debug.LogWarning("Player detector is null.");
        }
        else
        {
            _playerDetector.OnPlayerDetected += InitiateMinigameProcess;
        }

        if (_player == null)
        {
            Debug.LogWarning("Player is null.");
        }
        else
        {
            InputHandler.Instance.SetPlayer(_player);

            _player.HealthSystem.OnDied += PlayerDied;
            _player.EquipmentSystem.OnEquipped += EquipPlayer;
        }
    }

    private void Finish()
    {
        if (_playerDetector != null) _playerDetector.OnPlayerDetected -= InitiateMinigameProcess;

        if (_player != null) _player.HealthSystem.OnDied -= PlayerDied;
        if (_player != null) _player.EquipmentSystem.OnEquipped -= EquipPlayer;
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
            RewardAndPenaltyManager.Instance.PlayerHasEquippedEquipment();
        }
    }

    private void InitiateMinigameProcess()
    {
        _arrivedAtMinigameLocation = true;
        _playerCamera.Priority = 9;

        if (_player.EquipmentSystem.WearingEquipment)
        {
            RewardAndPenaltyManager.Instance.PlayerHasArrivedAtJob();
        }
        else
        {
            RewardAndPenaltyManager.Instance.PlayerHasArrivedAtJobUnequipped();
        }

        InitiateMinigame();
    }

    private void InitiateMinigame()
    {
        GameManager.Instance.UpdateGameState(GameState.MiniGameState);

        _player.HealthSystem.Invincible = true;

        for (int i = 0; i < _minigamesUIs.Count; i++)
        {
            _minigamesUIs[i].SetActive(i == _jobSectorSO.Day);
        }
    }

    public void MinigameSuccessed()
    {
        RewardAndPenaltyManager.Instance.PlayerHasCompletedJob();

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
        RewardAndPenaltyManager.Instance.PlayerHasFailedJob();

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
