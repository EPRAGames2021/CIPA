using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatrolSystem : MonoBehaviour
{
    [Header("GD area")]
    [SerializeField] private List<PatrolPoint> _points;
    [SerializeField] private List<Transform> _patrolPoints;

    [Header("Dev area")]
    [SerializeField] private Transform _currentTarget;
    [SerializeField] private int _currentTargetIndex;

    public Transform CurrentTarget => _currentTarget;
    public float Delay => _points[_currentTargetIndex].Delay;
    public bool HasTargets => _patrolPoints.Count > 0;


    private void Awake()
    {
        Init();
    }

    private void OnValidate()
    {
        if (_points.Count == 0 && _patrolPoints.Count > 0)
        {
            for (int i = 0; i < _patrolPoints.Count; i++)
            {
                PatrolPoint patrol = new(_patrolPoints[i].transform, 0);

                _points.Add(patrol);
            }
        }
    }


    private void Init()
    {
        _currentTargetIndex = 0;

        if (HasTargets)
        {
            _currentTarget = _points[_currentTargetIndex].Transform;
        }
    }


    public void NextPatrolPoint()
    {
        _currentTargetIndex++;

        if (_currentTargetIndex >= _points.Count)
        {
            _currentTargetIndex = 0;
        }

        _currentTarget = _points[_currentTargetIndex].Transform;
    }

    public void PreviousPatrolPoint()
    {
        _currentTargetIndex--;

        if (_currentTargetIndex < 0)
        {
            _currentTargetIndex = _points.Count - 1;
        }

        _currentTarget = _points[_currentTargetIndex].Transform;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < _points.Count; i++)
        {
            Gizmos.DrawWireCube(_points[i].Transform.position, new Vector3(1, 1, 1));
        }
    }
}

[System.Serializable]
public class PatrolPoint
{
    [SerializeField] private Transform _transform;
    [SerializeField] private float _delay;

    public Transform Transform => _transform;
    public float Delay => _delay;

    public PatrolPoint(Transform transform, float delay)
    {
        _transform = transform;
        _delay = delay;
    }
}