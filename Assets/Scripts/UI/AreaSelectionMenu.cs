using UnityEngine;
using UnityEngine.UI;

namespace EPRA.Utilities
{
    public class AreaSelectionMenu : MenuController
    {
        [SerializeField] private Button _backButton;

        [SerializeField] private Button _construction;
        [SerializeField] private Button _steelIndustry;

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
            Menu = MenuType.AreaSelectionMenu;

            _backButton.onClick.AddListener(CloseMenu);

            _construction.onClick.AddListener(GoToConstruction);
            _steelIndustry.onClick.AddListener(GoToSteelIndustry);
        }

        private void Finish()
        {
            _backButton.onClick.RemoveAllListeners();

            _construction.onClick.RemoveAllListeners();
            _steelIndustry.onClick.RemoveAllListeners();
        }


        private void GoToConstruction()
        {
            SceneLoader.Instance.LoadLevel(2, LoadMode.Replace);

            CanvasManager.Instance.CloseAllMenus();

            GameManager.Instance.UpdateGameState(GameState.GameState);
        }

        private void GoToSteelIndustry()
        {
            SceneLoader.Instance.LoadLevel(3, LoadMode.Replace);

            CanvasManager.Instance.CloseAllMenus();

            GameManager.Instance.UpdateGameState(GameState.GameState);
        }


        public override void SelectUI()
        {
            _construction.Select();
        }
    }
}
