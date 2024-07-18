using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPRA.Utilities;

namespace CIPA
{
    public class JobScoreMenu : MenuController
    {
        [SerializeField] private Button _closeMenu;

        [SerializeField] private CurrencySO _dayScore;
        [SerializeField] private TextMeshProUGUI _dayScoreText;

        [SerializeField] private JobSectorAreaSO _jobSectorArea;

        private void OnEnable()
        {
            GetScore();
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
            Menu = MenuType.DayScoreMenu;

            _closeMenu.onClick.AddListener(CloseMenu);
        }

        private void Finish()
        {
            _closeMenu.onClick.RemoveAllListeners();
        }


        private void GetScore()
        {
            if (JobAreaManager.Instance == null) return;

            _jobSectorArea = JobAreaManager.Instance.JobSectorAreaSO;

            _dayScoreText.text = "";

            if (!_jobSectorArea.IsFinalDay)
            {
                _dayScoreText.text = LanguageManager.GetTranslation("scoreOfTheDay", _dayScore.Value);
            }
            else
            {
                //starts at 1 because 0 is tutorial and shouldn't count towards score
                for (int i = 1; i < _jobSectorArea.TotalDays; i++)
                {
                    _dayScoreText.text += LanguageManager.GetTranslation("gameDay", i) + " | ";
                    _dayScoreText.text += LanguageManager.GetTranslation(_jobSectorArea.Jobs[i].KeyName) + " | ";
                    _dayScoreText.text += LanguageManager.GetTranslation("gameScore", _jobSectorArea.Jobs[i].Score) + "\n";
                }
            }

        }


        public override void SelectUI()
        {
            _closeMenu.Select();
        }
    }
}
