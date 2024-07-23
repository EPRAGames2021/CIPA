using System.Collections.Generic;
using UnityEngine;
using EPRA.Utilities;
using UnityEngine.UI;
using TMPro;

namespace CIPA
{
    public class DayReportMenu : MenuController
    {
        [SerializeField] private Button _closeButton;

        [SerializeField] private TMP_Text _actionsText;


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
            _closeButton.onClick.AddListener(CloseMenu);
        }

        private void Finish()
        {
            _closeButton.onClick.RemoveAllListeners();
        }


        public void SetDay(JobSO job)
        {
            List<TrackableAction> actions = job.Actions;
            _actionsText.text = string.Empty;

            for (int i = 0; i < actions.Count; i++)
            {
                string action = LanguageManager.GetTranslation(actions[i].Action);
                string performed = actions[i].Performed ? LanguageManager.GetTranslation("yes") : LanguageManager.GetTranslation("no");

                _actionsText.text += action + ": " + performed + "\n";
            }
        }


        public override void SelectUI()
        {
            _closeButton.Select();
        }
    }
}
