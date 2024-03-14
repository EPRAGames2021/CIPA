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

            AdaptToGameState(GameManager.Instance.State);
            GameManager.Instance.OnGameStateChanged += AdaptToGameState;
        }

        private void Finish()
        {
            _settingsButton.onClick.RemoveAllListeners();

            GameManager.Instance.OnGameStateChanged -= AdaptToGameState;
        }


        private void OpenSettings()
        {
            CanvasManager.Instance.OpenMenu(MenuType.SettingsMenu);
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
