using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EPRA.Utilities
{
    public class MainMenu : MenuController
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _adminButton;
        
        [Header("Other")]
        [SerializeField] private TextMeshProUGUI _gameName;

        private void OnValidate()
        {
            _gameName.text = Application.productName;
        }

        private void OnEnable()
        {
            //_adminButton.gameObject.SetActive(FirebaseHandler.Instance.IsAdminAccount);
            _adminButton.gameObject.SetActive(false);
        }

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
            Menu = MenuType.MainMenu;

            _settingsButton.onClick.AddListener(OpenSettings);
            _startButton.onClick.AddListener(StartGame);
            _adminButton.onClick.AddListener(OpenAdminPanel);
        }

        private void Finish()
        {
            _settingsButton.onClick.RemoveAllListeners();
            _startButton.onClick.RemoveAllListeners();
            _adminButton.onClick.RemoveAllListeners();
        }


        private void StartGame()
        {
            CanvasManager.Instance.OpenMenu(MenuType.AreaSelectionMenu);
        }

        private void OpenSettings()
        {
            CanvasManager.Instance.OpenMenu(MenuType.SettingsMenu);
        }

        private void OpenAdminPanel()
        {
            CanvasManager.Instance.OpenMenu(MenuType.AdminMenu);
        }
    }
}
