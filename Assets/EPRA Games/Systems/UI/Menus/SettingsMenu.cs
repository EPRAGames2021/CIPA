using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Text;

namespace EPRA.Utilities
{
    public class SettingsMenu : MenuController
    {
        [Header("Customize menu")]
        [SerializeField] private Complexity _complexity;

        [Header("Dev area")]
        [Space(10)]
        [SerializeField] private Button _backButton;

        [SerializeField] private Toggle _vibrationEnabled;

        [SerializeField] private Toggle _SFXEnabled;
        [SerializeField] private Toggle _musicEnabled;

        [SerializeField] private Slider _SFXVolume;
        [SerializeField] private Slider _musicVolume;

        [SerializeField] private TMP_Dropdown _languageDropdown;

        [Min(1), SerializeField] private int _targetFramerate = 60;

        [SerializeField] private Button _mainMenuButton;

        [Header("Other")]
        [SerializeField] private TextMeshProUGUI _gameVersion;

        private enum Complexity
        {
            ShowAll = 0,
            ShowTogglesOnly = 1,
            ShowSlidersOnly = 2,
        }


        private void OnEnable()
        {
            _mainMenuButton.gameObject.SetActive(GameManager.Instance.State != GameState.MainMenuState);
        }

        private void Start()
        {
            Init();
        }

        private void OnValidate()
        {
            ValidateComplexity();

            if (Application.isPlaying)
            {
                SetFramerate();
            }
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void Init()
        {
            _gameVersion.text = $"v_{Application.version}";

            Menu = MenuType.SettingsMenu;

            _backButton.onClick.AddListener(Back);

            _vibrationEnabled.onValueChanged.AddListener(ToggleVibration);
            _SFXEnabled.onValueChanged.AddListener(SFXEnabled);
            _SFXVolume.onValueChanged.AddListener(SFXVolume);
            _musicEnabled.onValueChanged.AddListener(MusicEnabled);
            _musicVolume.onValueChanged.AddListener(MusicVolume);
            _languageDropdown.onValueChanged.AddListener(SetLanguage);

            _mainMenuButton.onClick.AddListener(ReturnToMainMenu);

            _vibrationEnabled.isOn = Settings.Instance.Configuration.CanVibrate;
            _SFXEnabled.isOn = Settings.Instance.Configuration.CanPlaySFX;
            _musicEnabled.isOn = Settings.Instance.Configuration.CanPlayMusic;

            //Mixer goes from -80 to 0, slider goes from 0 to 1
            //Yes, I could change slider range but if someone ever decides to display slider value on menu it'll show weird numbers
            _SFXVolume.value = Remap(Settings.Instance.Configuration.SFXVolume, -80, 0, 0, 1);
            _musicVolume.value = Remap(Settings.Instance.Configuration.MusicVolume, -80, 0, 0, 1);
        }

        private void ValidateComplexity()
        {
            _SFXEnabled.transform.parent.gameObject.SetActive(_complexity == Complexity.ShowAll || _complexity == Complexity.ShowTogglesOnly);
            _musicEnabled.transform.parent.gameObject.SetActive(_complexity == Complexity.ShowAll || _complexity == Complexity.ShowTogglesOnly);

            _SFXVolume.transform.parent.gameObject.SetActive(_complexity == Complexity.ShowAll || _complexity == Complexity.ShowSlidersOnly);
            _musicVolume.transform.parent.gameObject.SetActive(_complexity == Complexity.ShowAll || _complexity == Complexity.ShowSlidersOnly);
        }

        private void Finish()
        {
            _backButton.onClick.RemoveAllListeners();

            _vibrationEnabled.onValueChanged.RemoveAllListeners();
            _SFXEnabled.onValueChanged.RemoveAllListeners();
            _SFXVolume.onValueChanged.RemoveAllListeners();
            _musicEnabled.onValueChanged.RemoveAllListeners();
            _musicVolume.onValueChanged.RemoveAllListeners();
            _languageDropdown.onValueChanged.RemoveAllListeners();

            _mainMenuButton.onClick.RemoveAllListeners();
        }


        private void Back()
        {
            CanvasManager.Instance?.CloseMenu(Menu);
        }



        private void ToggleVibration(bool value)
        {
            Settings.Instance?.SetVibration(value);
        }

        private void SFXEnabled(bool value)
        {
            Settings.Instance?.SetSFX(value);
        }

        private void SFXVolume(float value)
        {
            if (!_SFXEnabled.isOn && value > 0) _SFXEnabled.isOn = true;
            if (_SFXEnabled.isOn && value == 0) _SFXEnabled.isOn = false;

            //Mixer goes from -80 to 0, slider goes from 0 to 1
            //Yes, I could change slider range but if someone ever decides to display slider value on menu it'll show weird numbers
            value = Remap(value, 0, 1, -80, 0);

            Settings.Instance?.SetSFXVolume(value);
        }

        private void MusicEnabled(bool value)
        {
            Settings.Instance?.SetMusic(value);
        }

        private void MusicVolume(float value)
        {
            if (!_musicEnabled.isOn && value > 0) _musicEnabled.isOn = true;
            if (_musicEnabled.isOn && value == 0) _musicEnabled.isOn = false;

            //Mixer goes from -80 to 0, slider goes from 0 to 1
            //Yes, I could change slider range but if someone ever decides to display slider value on menu it'll show weird numbers
            value = Remap(value, 0, 1, -80, 0);

            Settings.Instance?.SetMusicVolume(value);
        }

        private void SetLanguage(int index)
        {
            switch (index)
            {
                case 0:
                    Settings.Instance?.SetLanguage(SystemLanguage.English);
                    break;
                case 1:
                    Settings.Instance?.SetLanguage(SystemLanguage.Portuguese);
                    break;
                default:
                    Settings.Instance?.SetLanguage(SystemLanguage.English);
                    break;

            }
        }

        private void SetFramerate()
        {
            Settings.Instance?.SetFramerate(_targetFramerate);
        }


        private void ReturnToMainMenu()
        {
            SceneLoader.Instance.LoadLevel(1, LoadMode.Replace);

            Back();

            CanvasManager.Instance.OpenMenu(MenuType.MainMenu);

            GameManager.Instance.UpdateGameState(GameState.MainMenuState);
        }


        private float Remap(float value, float originalMin, float originalMax, float newMin, float newMax)
        {
            return Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(originalMin, originalMax, value));
        }
    }
}