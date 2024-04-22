using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EPRA.Utilities;

public class CurrentMissionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentMission;

    [SerializeField] private int _day;
    [SerializeField] private int _mission;

    private void OnEnable()
    {
        DisplayMission();

        LanguageManager.OnLanguageChanged += DisplayMission;
        MissionManager.OnMissionChanged += DisplayMission;
    }

    private void OnDisable()
    {
        LanguageManager.OnLanguageChanged -= DisplayMission;
        MissionManager.OnMissionChanged -= DisplayMission;
    }


    private void DisplayMission(SystemLanguage language)
    {
        DisplayMission();
    }

    private void DisplayMission(int missionIndex)
    {
        DisplayMission();
    }

    private void DisplayMission()
    {
        _day = JobAreaManager.Instance.JobSectorAreaSO.Day;
        _mission = MissionManager.Instance.CurrentMissionIndex;

        string key = "day" + _day + "mission" + _mission;

        _currentMission.text = "-" + LanguageManager.GetTranslation(key);
    }
}
