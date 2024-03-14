using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EPRA.Utilities
{
    public class GameOverMenu : MenuController
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _restartButton;

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
            Menu = MenuType.GameOverMenu;

            _mainMenuButton.onClick.AddListener(GoToMainMenu);
            _restartButton.onClick.AddListener(RestartGame);
        }

        private void Finish()
        {
            _mainMenuButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();
        }


        private void RestartGame()
        {
            SceneLoader.Instance.ReloadLevel();

            CanvasManager.Instance.CloseMenu(Menu);
        }

        private void GoToMainMenu()
        {
            SceneLoader.Instance.LoadLevel(1, LoadMode.Replace);

            CanvasManager.Instance.CloseMenu(Menu);

            CanvasManager.Instance.OpenMenu(MenuType.MainMenu);

            GameManager.Instance.UpdateGameState(GameState.MainMenuState);
        }
    }
}
