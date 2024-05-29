using ES3Types;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

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

            AdaptToGameState(GameManager.Instance.State);
            GameManager.Instance.OnGameStateChanged += AdaptToGameState;
        }

        private void Finish()
        {
            _settingsButton.onClick.RemoveAllListeners();

            _dayScore.OnChangeValue -= UpdateDayScore;

            LanguageManager.OnLanguageChanged -= AdaptToLanguage;
            MissionManager.OnMissionChanged -= DisplayMission;

            GameManager.Instance.OnGameStateChanged -= AdaptToGameState;
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

        private void AdaptToGameState(GameState gameState)
        {
            _gameScreen.SetActive(gameState == GameState.GameState);
        }

        private void AdaptToLanguage(SystemLanguage systemLanguage)
        {
            UpdateDayScore(_score);
            SetDay(_day);

            int missionIndex = MissionManager.Instance.CurrentMissionIndex;
            DisplayMission(missionIndex);
        }

        private void DisplayMission(int missionIndex)
        {
            int dayIndex = JobAreaManager.Instance.JobSectorAreaSO.Day;

            string key = "day" + dayIndex + "mission" + missionIndex;

            _currentMission.text = LanguageManager.GetTranslation(key);
        }


        public void SetDay(int day)
        {
            _day = day;

            _dayText.text = LanguageManager.GetTranslation("gameDay", _day + 1);
        }
    }
}
