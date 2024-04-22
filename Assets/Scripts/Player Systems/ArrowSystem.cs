using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSystem : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Transform _target;


    private void Start()
    {
        Init();
    }

    private void LateUpdate()
    {
        _arrow.transform.LookAt(_target);
    }

    private void OnDestroy()
    {
        Finish();
    }


    private void Init()
    {
        _arrow.SetActive(false);

        MissionManager.OnMissionChanged += CheckArrow;
    }

    private void Finish()
    {
        MissionManager.OnMissionChanged -= CheckArrow;
    }


    private void CheckArrow(int missionIndex)
    {
        int day = JobAreaManager.Instance.JobSectorAreaSO.Day;

        _arrow.SetActive(missionIndex == 1 && day < 2);
    }
}
