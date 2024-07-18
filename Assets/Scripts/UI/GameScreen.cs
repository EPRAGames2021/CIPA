using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CIPA;

namespace EPRA.Utilities
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private Button _settingsButton;

        [SerializeField] private GameObject _gameScreen;

        [SerializeField] private TextMeshProUGUI _dayText;

        [SerializeField] private CurrencySO _dayScore;
        [SerializeField] private TextMeshProUGUI _dayScoreText;

        [SerializeField] private TextMeshProUGUI _currentMission;

        private int _score;
        private int _day;
        private JobSectorAreaSO _jobSectorAreaSO;


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
            _settingsButton.onClick.AddListener(OpenSettings);

            _dayScore.OnChangeValue += UpdateDayScore;

            LanguageManager.OnLanguageChanged += AdaptToLanguage;
            MissionManager.OnMissionChanged += DisplayMission;
        }

        private void Finish()
        {
            _settingsButton.onClick.RemoveAllListeners();

            _dayScore.OnChangeValue -= UpdateDayScore;

            LanguageManager.OnLanguageChanged -= AdaptToLanguage;
            MissionManager.OnMissionChanged -= DisplayMission;
        }


        private void OpenSettings()
        {
            CanvasManager.Instance.OpenMenu(MenuType.SettingsMenu);
        }

        private void UpdateDayScore(int score)
        {
            _score = score;

            _dayScoreText.text = LanguageManager.GetTranslation("gameScore", score);
        }


        public void EnableHUD(bool enable)
        {
            _gameScreen.SetActive(enable);
        }


        private void AdaptToLanguage(SystemLanguage systemLanguage)
        {
            UpdateDayScore(_score);
            SetDay(_jobSectorAreaSO);

            if (MissionManager.Instance != null)
            {
                int missionIndex = MissionManager.Instance.CurrentMissionIndex;

                DisplayMission(missionIndex);
            }
        }

        private void DisplayMission(int missionIndex)
        {
            JobSector jobSector = JobAreaManager.Instance.JobSectorAreaSO.JobSector;
            int dayIndex = JobAreaManager.Instance.JobSectorAreaSO.Day;

            string key = jobSector + "Day" + dayIndex + "mission" + missionIndex;

            _currentMission.text = LanguageManager.GetTranslation(key);
        }


        public void SetDay(JobSectorAreaSO jobSectorAreaSO)
        {
            _jobSectorAreaSO = jobSectorAreaSO;
            _day = _jobSectorAreaSO.Day;

            if (_day == 0)
            {
                _dayText.text = "Tutorial";
            }
            else
            {
                _dayText.text = LanguageManager.GetTranslation("gameDay", _day) + ": " + LanguageManager.GetTranslation(_jobSectorAreaSO.CurrentJob.KeyName);
            }
        }
    }
}
