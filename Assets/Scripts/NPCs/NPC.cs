using DG.Tweening;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PatrolSystem))]
public class NPC : MonoBehaviour
{
    [Header("GD area")]
    [SerializeField] private float _speed;
    [SerializeField] private bool _carryingBox;
    [SerializeField] private bool _pushingHandCart;

    [Header("Dev area")]
    [SerializeField] private Animator _animator;

    [SerializeField] private PatrolSystem _patrolSystem;
    [SerializeField] private float _distanceToTarget;

    [SerializeField] private CharacterState _characterState;


    [SerializeField] private GameObject _box;
    [SerializeField] private GameObject _handcart;


    private void OnValidate()
    {
        if (_patrolSystem == null) _patrolSystem = GetComponent<PatrolSystem>();
        if (_animator == null) _animator = GetComponentInChildren<Animator>();

        if (EditorApplication.isPlaying) CheckCarrying();
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
        CheckCarrying();

        if (_patrolSystem.HasTargets)
        {
            _characterState = CharacterState.Roaming;
        }
        else
        {
            Debug.Log("Character has no patrol points");

            _characterState = CharacterState.Talking;

            Talk();
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

    private void Talk()
    {
        int random = Random.Range(0, 2);

        _animator.SetBool("IsTalking" + random.ToString(), _characterState == CharacterState.Talking);
    }

    private void CheckCarrying()
    {
        _box.SetActive(_carryingBox);
        _handcart.SetActive(_pushingHandCart);

        _animator.SetBool("IsCarrying", _carryingBox || _pushingHandCart);
    }
}
