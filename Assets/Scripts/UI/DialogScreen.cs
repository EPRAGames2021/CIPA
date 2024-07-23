using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPRA.Utilities;

namespace CIPA
{
    public class DialogScreen : MonoBehaviour
    {
        [SerializeField] private ScreenTouchController _screenTouchController;

        [SerializeField] private GameObject _background;
        [SerializeField] private GameObject _descriptionPanel;

        [SerializeField] private TMP_Text _speaker;
        [SerializeField] private Image _speakerIcon;
        [SerializeField] private TMP_Text _descriptionText;


        [SerializeField] private Button _closePanelButton;

        [SerializeField] private DialogSO _dialogSO;
        [SerializeField] private int _dialogIndex;
        [SerializeField] private List<string> _dialogs;

        public event System.Action OnDialogsFinished;


        private void Awake()
        {
            SetDescriptionPanelActive(false);            
        }

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
            _dialogIndex = 0;

            _screenTouchController.OnPressed += AdvanceDialog;
        }

        private void Finish()
        {
            _screenTouchController.OnPressed -= AdvanceDialog;
        }


        private void AdvanceDialog()
        {
            if (_dialogIndex < _dialogs.Count - 1)
            {
                _dialogIndex++;

                ShowCurrentDialog();
            }            
            else
            {
                ClearDialogs();
                SetDescriptionPanelActive(false);

                OnDialogsFinished?.Invoke();
            }
        }

        private void ClearDialogs()
        {
            _dialogSO = null;
            _dialogs.Clear();

            _descriptionText.text = string.Empty;
        }

        public void SetDialogSO(DialogSO dialogSO)
        {
            ClearDialogs();

            _dialogSO = dialogSO;
            _dialogIndex = 0;

            SetDialogs(_dialogSO.Dialogs);
            SetSpeaker(_dialogSO.Speaker, _dialogSO.SpeakerIcon);

            ShowCurrentDialog();
        }

        private void SetDialogs(List<string> dialogs)
        {
            _dialogs = new(dialogs);
        }

        private void SetSpeaker(string nameKey, Sprite icon)
        {
            _speaker.text = LanguageManager.GetTranslation(nameKey);
            _speakerIcon.sprite = icon;
        }

        private void ShowCurrentDialog()
        {
            _descriptionText.text = LanguageManager.GetTranslation(_dialogs[_dialogIndex]);

            SetDescriptionPanelActive(true);

            _screenTouchController.ReInit();
        }

        public void SetDescriptionPanelActive(bool active)
        {
            _background.SetActive(active);
            _descriptionPanel.SetActive(active);

            gameObject.SetActive(active);
        }
    }
}
