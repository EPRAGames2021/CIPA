using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPRA.Utilities;

namespace CIPA
{
    public class TutorialConstructionUI : MonoBehaviour
    {
        [SerializeField] private ScreenTouchController _screenTouchController;

        [SerializeField] private TutorialConstructionController _controller;

        [SerializeField] private GameObject _tutorialHand;

        [SerializeField] private GameObject _descriptionPanel;
        [SerializeField] private TMP_Text _descriptionText;

        [SerializeField] private Button _closePanelButton;


        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            Finish();
        }


        private void Init()
        {
            //_closePanelButton.onClick.AddListener(AdvanceTutorial);

            _screenTouchController.OnPressed += AdvanceTutorial;
        }

        private void Finish()
        {
            //_closePanelButton.onClick.RemoveAllListeners();

            _screenTouchController.OnPressed -= AdvanceTutorial;
        }


        private void AdvanceTutorial()
        {
            SetDescriptionPanelActive(false);

            _controller.CloseTutorial();

            _screenTouchController.ReInit();
        }

        public void ShowDescription(string key)
        {
            _descriptionText.text = LanguageManager.GetTranslation(key);

            SetDescriptionPanelActive(true);

            _screenTouchController.ReInit();
        }

        public void SetDescriptionPanelActive(bool active)
        {
            _descriptionPanel.SetActive(active);
        }

        public void ShowHand(bool active)
        {
            _tutorialHand.SetActive(active);
        }
    }
}
