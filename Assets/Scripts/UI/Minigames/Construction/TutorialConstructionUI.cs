using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPRA.Utilities;

namespace CIPA
{
    public class TutorialConstructionUI : MonoBehaviour
    {
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
            _closePanelButton.onClick.AddListener(AdvanceTutorial);
        }

        private void Finish()
        {
            _closePanelButton.onClick.RemoveAllListeners();
        }


        private void AdvanceTutorial()
        {
            SetDescriptionPanelActive(false);

            _controller.CloseTutorial();
        }

        public void ShowDescription(string key)
        {
            _descriptionText.text = LanguageManager.GetTranslation(key);

            SetDescriptionPanelActive(true);
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
