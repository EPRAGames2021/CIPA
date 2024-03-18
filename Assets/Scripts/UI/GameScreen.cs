using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

            AdaptToGameState(GameManager.Instance.State);
            GameManager.Instance.OnGameStateChanged += AdaptToGameState;
        }

        private void Finish()
        {
            _settingsButton.onClick.RemoveAllListeners();

            _dayScore.OnChangeValue -= UpdateDayScore;

            GameManager.Instance.OnGameStateChanged -= AdaptToGameState;
        }


        private void OpenSettings()
        {
            CanvasManager.Instance.OpenMenu(MenuType.SettingsMenu);
        }

        private void UpdateDayScore(int score)
        {
            _dayScoreText.text = "score: " + score;
        }

        private void AdaptToGameState(GameState gameState)
        {
            _gameScreen.SetActive(gameState == GameState.GameState);
        }


        public void SetDay(int day)
        {
            _dayText.text = "Day: " + day.ToString();
        }
    }
}
