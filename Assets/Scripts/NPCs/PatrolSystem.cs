using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolSystem : MonoBehaviour
{
    [Header("GD area")]
    [SerializeField] private List<Transform> _patrolPoints;

    [Header("Dev area")]
    [SerializeField] private Transform _currentTarget;
    [SerializeField] private int _currentTargetIndex;

    public Transform CurrentTarget => _currentTarget;
    public bool HasTargets => _patrolPoints.Count > 0;


    private void Awake()
    {
        Init();
    }


    private void Init()
    {
        _currentTargetIndex = 0;

        if (HasTargets)
        {
            _currentTarget = _patrolPoints[_currentTargetIndex];
        }
        else
        {
            Debug.LogWarning(this + " has no targets. Setting target to itself.");

            _currentTarget = transform;
        }
    }


    public void NextPatrolPoint()
    {
        _currentTargetIndex++;
        
        if (_currentTargetIndex >= _patrolPoints.Count)
        {
            _currentTargetIndex = 0;
        }

        _currentTarget = _patrolPoints[_currentTargetIndex];
    }

    public void PreviousPatrolPoint()
    {
        _currentTargetIndex--;

        if (_currentTargetIndex < 0)
        {
            _currentTargetIndex = _patrolPoints.Count - 1;
        }

        _currentTarget = _patrolPoints[_currentTargetIndex];
    }
}
