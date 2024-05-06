using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowSystem : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;

    [Header("Dev area")]
    [SerializeField] private Transform _target;
    [SerializeField] private int _index;
    [SerializeField] private List<Transform> _targets;


    private void Start()
    {
        Init();
    }

    private void LateUpdate()
    {
        _arrow.transform.DOLookAt(_target.position, 0.5f);
        CheckDistance();
    }

    private void OnDestroy()
    {
        Finish();
    }


    private void Init()
    {
        _index = 0;
        _target = _targets[_index];
        _arrow.SetActive(false);

        MissionManager.OnMissionChanged += CheckArrow;
    }

    private void Finish()
    {
        MissionManager.OnMissionChanged -= CheckArrow;
    }


    
    private void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, _targets[_index].position);

        if (distance < 3 && _index < _targets.Count - 1)
        {
            _index++;
            _target = _targets[_index];
        }
    }

    private void CheckArrow(int missionIndex)
    {
        int day = JobAreaManager.Instance.JobSectorAreaSO.Day;

        _arrow.SetActive(missionIndex == 1 && day < 2);
    }
}
