using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EPRA.Utilities
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance { get; private set; }

        [SerializeField] private List<MenuController> _menuControllersList;

        [SerializeField] private List<MenuController> _allActiveMenus;

        [SerializeField] private MenuController _currentMenu;

        [SerializeField] private FloatingJoystick _floatingJoystick;

        [SerializeField] private GameScreen _gameScreen;
        [SerializeField] private LoadingScreen _loadingScreen;


        public FloatingJoystick FloatingJoystick => _floatingJoystick;
        public GameScreen GameScreen => _gameScreen;
        public LoadingScreen LoadingScreen => _loadingScreen;


        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            GameManager.Instance.OnGameStateChanged += AdaptToGameState;

            SceneLoader.Instance.OnLoadIsInProgress += DisplayLoadingScreen;
            SceneLoader.Instance.OnProgressChanges += LoadingScreen.SetPercentage;

            CloseAllMenus();
            OpenMenu(MenuType.LoginMenu);
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameStateChanged -= AdaptToGameState;

            SceneLoader.Instance.OnLoadIsInProgress -= DisplayLoadingScreen;
            SceneLoader.Instance.OnProgressChanges -= LoadingScreen.SetPercentage;
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

        private void AdaptToGameState(GameState gameState)
        {
            FloatingJoystick.gameObject.SetActive(gameState == GameState.GameState);
        }

        private void DisplayLoadingScreen(bool display)
        {
            _loadingScreen.DisplayLoadingScreen(display);

            if (_loadingScreen.IsBeingDisplayed)
            {
                _loadingScreen.SelectUI();
            }
            else
            {
                _currentMenu?.SelectUI();
            }
        }


        public bool AddMenuToList(MenuController menuController)
        {
            if (_menuControllersList.Contains(menuController))
            {
                Debug.Log(menuController + " already exists in list of menus. Skipping it.");

                return false;
            }
            else
            {
                Debug.Log("Adding " + menuController + " to list of menus.");

                _menuControllersList.Add(menuController);

                return true;
            }
        }

        public bool RemoveMenuFromList(MenuController menuController)
        {
            if (!_menuControllersList.Contains(menuController))
            {
                Debug.Log(menuController + " cannot be removed because it does not exist in list of menus.");

                return false;
            }
            else
            {
                Debug.Log("Removing " + menuController + " from list of menus.");

                _menuControllersList.Remove(menuController);

                return true;
            }
        }


        public void OpenMenu(MenuType menu)
        {
            MenuController desiredMenu = _menuControllersList.Find(menuController => menuController.Menu == menu);

            if (desiredMenu == null)
            {
                Debug.LogWarning($" {menu} cannot be opened because it is null");

                return;
            }

            if (_allActiveMenus.Contains(desiredMenu))
            {
                Debug.LogWarning($"Cannot open {menu} because it has already been opened");

                return;
            }
            else
            {
                _currentMenu = desiredMenu;

                _allActiveMenus.Add(_currentMenu);

                _currentMenu.gameObject.SetActive(true);

                if (!_loadingScreen.IsBeingDisplayed) _currentMenu?.SelectUI();
            }
        }

        public void SwitchMenu(MenuType menu)
        {
            CloseCurrentMenu();

            OpenMenu(menu);
        }

        public void CloseMenu(MenuType menu)
        {
            MenuController desiredMenu = _menuControllersList.Find(menuController => menuController.Menu == menu);

            if (desiredMenu == null)
            {
                Debug.LogWarning($" {menu} cannot be closed because it is null");

                return;
            }

            if (!_allActiveMenus.Contains(desiredMenu))
            {
                Debug.LogWarning($"Cannot close {menu} because it has not been opened");

                return;
            }
            else
            {
                _currentMenu = desiredMenu;

                _allActiveMenus.Remove(_currentMenu);

                _currentMenu.gameObject.SetActive(false);

                _currentMenu = _allActiveMenus.LastOrDefault();

                if (!_loadingScreen.IsBeingDisplayed) _currentMenu?.SelectUI();
            }
        }

        public void CloseCurrentMenu()
        {
            CloseMenu(_currentMenu.Menu);
        }

        public void CloseAllMenus()
        {
            while (_allActiveMenus.Count > 0)
            {
                CloseCurrentMenu();
            }
        }


        public void SetHudEnabled(bool enable)
        {
            _gameScreen.gameObject.SetActive(enable);
        }

        public void SwitchSettings()
        {
            if (_currentMenu?.Menu == MenuType.SettingsMenu)
            {
                CloseCurrentMenu();
            }
            else
            {
                OpenMenu(MenuType.SettingsMenu);
            }
        }
    }

    public enum MenuType
    {
        MainMenu = 0,
        SettingsMenu = 1,
        GameOverMenu = 2,
        VictoryMenu = 3,
        AreaSelectionMenu = 4,
        DayScoreMenu = 5,
        LoginMenu = 6,
        AdminMenu = 7,
        PPESelectionMenu = 8,
    }
}