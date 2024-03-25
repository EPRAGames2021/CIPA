using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EPRA.Utilities
{
    public class VictoryMenu : MenuController
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _nextDayButton;

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
            Menu = MenuType.VictoryMenu;
    
            _mainMenuButton.onClick.AddListener(GoToMainMenu);
            _nextDayButton.onClick.AddListener(GoToNextDay);
        }
    
        private void Finish()
        {
            _mainMenuButton.onClick.RemoveAllListeners();
            _nextDayButton.onClick.RemoveAllListeners();
        }
    
    
        private void GoToNextDay()
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
