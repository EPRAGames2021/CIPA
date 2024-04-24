using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [SerializeField] private JobSectorAreaSO _jobSectorAreaSO;

    [SerializeField] private int _currentMissionIndex;

    public int CurrentMissionIndex => _currentMissionIndex;

    public static event System.Action<int> OnMissionChanged;


    private void Awake()
    {
        InitSingleton();
    }

    private void Start()
    {
        Init();
    }


    private void InitSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        _jobSectorAreaSO = JobAreaManager.Instance.JobSectorAreaSO;

        _currentMissionIndex = 0;

        OnMissionChanged?.Invoke(_currentMissionIndex);
    }

    public void GoToNextMission()
    {
        _currentMissionIndex++;

        OnMissionChanged?.Invoke(_currentMissionIndex);
    }

    public void GoToMission(int missions)
    {
        _currentMissionIndex = missions;

        OnMissionChanged?.Invoke(_currentMissionIndex);
    }
}
