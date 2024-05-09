using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PatrolSystem))]
public class NPC : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private PatrolSystem _patrolSystem;
    [SerializeField] private float _distanceToTarget;

    [SerializeField] private CharacterState _characterState;

    [SerializeField] private float _speed;


    private void OnValidate()
    {
        if (_patrolSystem == null) _patrolSystem = GetComponent<PatrolSystem>();
        if (_animator == null) _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Move();
    }


    private void Init()
    {
        if (_patrolSystem.CurrentTarget == null)
        {
            Debug.Log("Character has no patrol points");

            _characterState = CharacterState.None;
        }
        else
        {
            _characterState = CharacterState.Roaming;
        }
    }

    private void Move()
    {
        _animator.SetBool("IsWalking", _characterState == CharacterState.Roaming);

        if (!_characterState.Equals(CharacterState.Roaming)) return;

        transform.DOLookAt(_patrolSystem.CurrentTarget.position, 0.25f);
        transform.position = Vector3.MoveTowards(transform.position, _patrolSystem.CurrentTarget.position, _speed * Time.deltaTime);

        _distanceToTarget = Vector3.Distance(transform.position, _patrolSystem.CurrentTarget.position);

        if (_distanceToTarget < 0.5f)
        {
            _patrolSystem.NextPatrolPoint();
        }        
    }
}
