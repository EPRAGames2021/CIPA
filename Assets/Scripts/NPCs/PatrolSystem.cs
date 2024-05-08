using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolSystem : MonoBehaviour
{
    [SerializeField] private List<Transform> _patrolPoints;

    [SerializeField] private Transform _currentTarget;
    [SerializeField] private int _currentTargetIndex;


    private void Start()
    {
        Init();
    }


    private void Init()
    {
        _currentTargetIndex = 0;
        _currentTarget = _patrolPoints[_currentTargetIndex];
    }


    private void NextPatrolPoint()
    {
        _currentTargetIndex++;
        
        if (_currentTargetIndex >= _patrolPoints.Count)
        {
            _currentTargetIndex = 0;
        }

        _currentTarget = _patrolPoints[_currentTargetIndex];
    }

    private void PreviousPatrolPoint()
    {
        _currentTargetIndex--;

        if (_currentTargetIndex < 0)
        {
            _currentTargetIndex = _patrolPoints.Count - 1;
        }

        _currentTarget = _patrolPoints[_currentTargetIndex];
    }
}
