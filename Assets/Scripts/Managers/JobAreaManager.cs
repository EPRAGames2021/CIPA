using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EPRA.Utilities;

namespace CIPA
{
    public class JobAreaManager : MonoBehaviour
    {
        public static JobAreaManager Instance { get; private set; }

        [SerializeField] private JobSectorAreaSO _jobSectorSO;

        [SerializeField] private List<GameObject> _minigameContextObjects;

        [SerializeField] private CinemachineVirtualCamera _playerCamera;

        [SerializeField] private Player _player;

        [SerializeField] private CurrencySO _dayScore;

        [SerializeField] private bool _arrivedAtMinigameLocation;

        [Header("Sounds")]
        [SerializeField] private AudioClipCollection _victorySFX;
        [SerializeField] private AudioClipCollection _defeatSFX;
        [SerializeField] private AudioClipCollection _deathSFX;


        public JobSectorAreaSO JobSectorAreaSO => _jobSectorSO;
        public bool ArrivedAtMinigameLocation { get { return _arrivedAtMinigameLocation; } set { _arrivedAtMinigameLocation = value; } }


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

            CustomGameEvents.OnPlayerArrivedAtMinigameLocation += InitiateMinigameProcess;

            for (int i = 0; i < _minigameContextObjects.Count; i++)
            {
                _minigameContextObjects[i].SetActive(i == _jobSectorSO.Day);
            }


            if (_player == null)
            {
                Debug.LogWarning("Player is null.");
            }
            else
            {
                InputHandler.Instance.SetMovementSystem(_player.MovementSystem);

                _player.HealthSystem.OnDied += PlayerDied;
                _player.EquipmentSystem.OnEquipped += EquipPlayer;
            }
        }

        private void Finish()
        {
            CustomGameEvents.OnPlayerArrivedAtMinigameLocation -= InitiateMinigameProcess;

            if (_player != null) _player.HealthSystem.OnDied -= PlayerDied;
            if (_player != null) _player.EquipmentSystem.OnEquipped -= EquipPlayer;
        }


        private void PlayerDied()
        {
            _jobSectorSO.CurrentJob.AddUniqueAction("playerDisruptedFlow", true);

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
            _playerCamera.Priority = 9;

            if (_player.EquipmentSystem.WearingEquipment)
            {
                RewardAndPenaltyManager.Instance.PlayerHasArrivedAtJob();

            }
            else
            {
                RewardAndPenaltyManager.Instance.PlayerHasArrivedAtJobUnequipped();
            }

            _jobSectorSO.CurrentJob.AddUniqueAction("playerUsedPPE", _player.EquipmentSystem.WearingEquipment);

            InitiateMinigame();
        }

        private void InitiateMinigame()
        {
            GameManager.Instance.UpdateGameState(GameState.MiniGameState);

            _player.HealthSystem.Invincible = true;

            CustomGameEvents.InvokeOnMinigameStarted();
        }

        public void FinishMinigame(bool success)
        {
            if (success)
            {
                MinigameSuccessed();
            }
            else
            {
                MinigameFailed();
            }
        }

        private void MinigameSuccessed()
        {
            _jobSectorSO.CurrentJob.AddUniqueAction("playerCompletedDay", true);

            RewardAndPenaltyManager.Instance.PlayerHasCompletedJob();

            _jobSectorSO.SetScoreToDay(_jobSectorSO.Day, _dayScore.Value);

            CanvasManager.Instance.OpenMenu(MenuType.VictoryMenu);
            CanvasManager.Instance.OpenMenu(MenuType.DayScoreMenu);

            Vibrator.Vibrate(100);
            AudioManager.Instance.PlayRandomSFX(_victorySFX);

            _jobSectorSO.FinishDay();

            CustomGameEvents.InvokeOnMinigameEnded();
        }

        private void MinigameFailed()
        {
            _jobSectorSO.CurrentJob.AddUniqueAction("playerCompletedDay", false);

            RewardAndPenaltyManager.Instance.PlayerHasFailedJob();

            CanvasManager.Instance.OpenMenu(MenuType.GameOverMenu);

            Vibrator.Vibrate(100);
            AudioManager.Instance.PlayRandomSFX(_defeatSFX);

            CustomGameEvents.InvokeOnMinigameEnded();
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
}

