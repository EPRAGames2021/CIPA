using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EPRA.Utilities
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private Button _settingsButton;


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
        }

        private void Finish()
        {
            _settingsButton.onClick.RemoveAllListeners();
        }


        private void OpenSettings()
        {
            CanvasManager.Instance.OpenMenu(MenuType.SettingsMenu);
        }
    }
}
