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

            AdaptToGameState(GameManager.Instance.State);
            GameManager.Instance.OnGameStateChanged += AdaptToGameState;
        }

        private void Finish()
        {
            _settingsButton.onClick.RemoveAllListeners();

            _dayScore.OnChangeValue -= UpdateDayScore;

            LanguageManager.OnLanguageChanged += AdaptToLanguage;

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
        }


        public void SetDay(int day)
        {
            _day = day;

            _dayText.text = LanguageManager.GetTranslation("gameDay", _day);
        }
    }
}
