using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class JobAreaManager : MonoBehaviour
    {
        public static JobAreaManager Instance { get; private set; }

        [SerializeField] private JobSectorAreaSO _jobSectorSO;
        [SerializeField] private DialogSO _boss;
        [SerializeField] private DialogSO _doctor;

        [SerializeField] private List<GameObject> _minigameContextObjects;

        [SerializeField] private Player _player;

        [SerializeField] private CurrencySO _dayScore;

        [SerializeField] private bool _arrivedAtMinigameLocation;

        [SerializeField] private Transform _doctorsOffice;

        [Header("Sounds")]
        [SerializeField] private AudioClipCollection _victorySFX;
        [SerializeField] private AudioClipCollection _defeatSFX;
        [SerializeField] private AudioClipCollection _deathSFX;


        public JobSectorAreaSO JobSectorAreaSO => _jobSectorSO;
        public Player Player => _player;
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
            _arrivedAtMinigameLocation = false;

            RewardAndPenaltyManager.Instance.ResetScore();
            GameManager.Instance.UpdateGameState(GameState.GameState);
            CanvasManager.Instance.GameScreen.SetDay(_jobSectorSO);
            InputHandler.Instance.SetMovementSystem(_player.MovementSystem);

            for (int i = 0; i < _minigameContextObjects.Count; i++)
            {
                _minigameContextObjects[i].SetActive(i == _jobSectorSO.Day);
            }

            CustomGameEvents.OnPlayerArrivedAtMinigameLocation += PreInitiateMinigame;
            
            _player.OnDied += PlayerDied;
            _player.EquipmentSystem.OnEquipped += EquipPlayer;
        }

        private void Finish()
        {
            CustomGameEvents.OnPlayerArrivedAtMinigameLocation -= PreInitiateMinigame;

            _player.OnDied -= PlayerDied;
            _player.EquipmentSystem.OnEquipped -= EquipPlayer;
        }


        private void EquipPlayer(bool equip)
        {
            if (equip)
            {
                RewardAndPenaltyManager.Instance.PlayerHasEquippedEquipment();
            }
        }

        private void PreInitiateMinigame()
        {
            _arrivedAtMinigameLocation = true;

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
            CanvasManager.Instance.InitiateFadeSequence();

            CanvasManager.Instance.CloseAllMenus();
            GameManager.Instance.UpdateGameState(GameState.MiniGameState);

            _player.HealthSystem.Invincible = true;

            CustomGameEvents.InvokeOnMinigameStarted();
        }

        private void MinigameSuccessed()
        {
            _jobSectorSO.CurrentJob.AddUniqueAction("playerCompletedDay", true);
            RewardAndPenaltyManager.Instance.PlayerHasCompletedJob();
            AudioManager.Instance.PlayRandomSFX(_victorySFX);
            Vibrator.Vibrate(100);

            _jobSectorSO.SetScoreToDay(_jobSectorSO.Day, _dayScore.Value);

            CustomGameEvents.InvokeOnMinigameEnded();

            StartCoroutine(OpenBossDialogDelay());
        }

        private IEnumerator OpenBossDialogDelay()
        {
            yield return new WaitForSeconds(1.0f);

            _player.Win();

            yield return new WaitForSeconds(1.5f);

            CanvasManager.Instance.DialogScreen.SetDialogSO(_boss);
            CanvasManager.Instance.DialogScreen.OnDialogsFinished += MoveToDoctor;
        }

        private void MoveToDoctor()
        {
            CanvasManager.Instance.DialogScreen.OnDialogsFinished -= MoveToDoctor;

            StartCoroutine(MoveToDoctorDelay());
        }

        private IEnumerator MoveToDoctorDelay()
        {
            CanvasManager.Instance.InitiateFadeSequence();

            yield return new WaitForSeconds(0.5f);

            _player.transform.SetLocalPositionAndRotation(_doctorsOffice.position, _doctorsOffice.rotation);

            CanvasManager.Instance.DialogScreen.SetDialogSO(_doctor);
            CanvasManager.Instance.DialogScreen.OnDialogsFinished += OpenReportMenu;
        }

        private void OpenReportMenu()
        {
            CanvasManager.Instance.DialogScreen.OnDialogsFinished -= OpenReportMenu;

            DayReportMenu reportMenu = CanvasManager.Instance.OpenMenu(MenuType.DayReportMenu) as DayReportMenu;

            reportMenu.SetDay(_jobSectorSO.CurrentJob);

            CanvasManager.Instance.OpenMenu(MenuType.VictoryMenu);
            CanvasManager.Instance.OpenMenu(MenuType.DayScoreMenu);
            
            _jobSectorSO.FinishDay();
        }

        private void MinigameFailed()
        {
            _jobSectorSO.CurrentJob.AddUniqueAction("playerCompletedDay", false);

            RewardAndPenaltyManager.Instance.PlayerHasFailedJob();

            EndJob();
        }

        private void EndJob()
        {
            CanvasManager.Instance.EnableHUD(false);
            CanvasManager.Instance.OpenMenu(MenuType.GameOverMenu);

            AudioManager.Instance.PlayRandomSFX(_defeatSFX);

            Vibrator.Vibrate(100);

            CustomGameEvents.InvokeOnMinigameEnded();
        }


        public void PlayerDied()
        {
            _jobSectorSO.CurrentJob.AddUniqueAction("playerFollowedPath", false);

            AudioManager.Instance.PlayRandomSFX(_deathSFX);

            EndJob();
        }

        public void FinishMinigame(bool success)
        {
            CanvasManager.Instance.InitiateFadeSequence();

            if (success)
            {
                MinigameSuccessed();
            }
            else
            {
                MinigameFailed();
            }
        }

        public void FinishTutorial()
        {
            _jobSectorSO.CurrentJob.AddUniqueAction("playerCompletedDay", true);

            AudioManager.Instance.PlayRandomSFX(_victorySFX);
            Vibrator.Vibrate(100);
            _player.Win();
            StartCoroutine(FinishTutorialDelay());

            IEnumerator FinishTutorialDelay()
            {
                yield return new WaitForSeconds(1.5f);

                CanvasManager.Instance.OpenMenu(MenuType.VictoryMenu);

                _jobSectorSO.FinishDay();
            }
        }

        public void RestartJob()
        {
            if (_arrivedAtMinigameLocation)
            {
                InitiateMinigame();

                //a rework of this system would come in handy
                //2 because player will have already completed missions 0 and 1
                //0 -> wear ppe
                //1 -> attive at location
                MissionManager.Instance.GoToMission(2);
            }
            else
            {
                SceneLoader.Instance.ReloadLevel();
            }
        }
    }
}

