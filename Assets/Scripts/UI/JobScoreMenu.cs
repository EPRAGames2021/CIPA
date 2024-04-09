using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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


    private void CloseMenu()
    {
        CanvasManager.Instance.CloseMenu(Menu);
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
            for (int i = 0; i < _jobSectorArea.TotalDays; i++)
            {
                //_dayScoreText.text += LanguageManager.GetTranslation("scoreOfTheDayScore", i, _jobSectorArea.Jobs[i].JobName + ": " + _jobSectorArea.Jobs[i].Score + "\n");
                //_dayScoreText.text += LanguageManager.GetTranslation("dayJobScore", i, _jobSectorArea.Jobs[i].KeyName, _jobSectorArea.Jobs[i].Score + "\n");

                _dayScoreText.text += LanguageManager.GetTranslation("gameDay", i + 1) + " | ";
                _dayScoreText.text += LanguageManager.GetTranslation(_jobSectorArea.Jobs[i].KeyName) + " | ";
                _dayScoreText.text += LanguageManager.GetTranslation("gameScore", _jobSectorArea.Jobs[i].Score) + "\n";
            }
        }

    }
}
