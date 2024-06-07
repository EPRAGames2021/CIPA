using UnityEngine;
using EPRA.Utilities;
using DG.Tweening;

[RequireComponent(typeof(PatrolSystem))]
public class DodgeableTruck : MonoBehaviour
{
    [SerializeField] private PlayerDetector _playerDetector;

    [SerializeField] private bool _moving;
    [SerializeField, Min(0.1f)] private float _speed;

    [SerializeField] private Animator _animator;

    [SerializeField] private PatrolSystem _patrolSystem;

    private void OnValidate()
    {
        if (_patrolSystem == null) _patrolSystem = GetComponent<PatrolSystem>();
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
        _playerDetector.OnPlayerDetected += HandlePlayerDetection;

        transform.LookAt(_patrolSystem.CurrentTarget);
    }

    private void Finish()
    {
        _playerDetector.OnPlayerDetected -= HandlePlayerDetection;
    }


    private void HandlePlayerDetection(Player player)
    {
        InitiateTruckMovement();
    }


    private void InitiateTruckMovement()
    {
        _moving = true;

        Move();
    }

    private void Move()
    {
        if (!_moving) return;

        float distance = Vector3.Distance(transform.position, _patrolSystem.CurrentTarget.position);
        float time = distance / _speed;

        transform.DOMove(_patrolSystem.CurrentTarget.position, time);
    }
}
