using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyState _state = EnemyState.None;
    [SerializeField] private EnemyState _previousState = EnemyState.None;

    [SerializeField] private bool _validEnemy;
    [SerializeField] private Animator _animator;

    [Header("Systems")]
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private MovementSystem _movementSystem;

    public HealthSystem HealthSystem => _healthSystem;
    public MovementSystem MovementSystem => _movementSystem;

    public EnemyState State => _state;

    public event System.Action<EnemyState> OnStateChange;
    public event System.Action OnDied;

    private void Awake()
    {
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
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
        _healthSystem.OnDied += Die;

        _validEnemy = true;

        ChangeState(EnemyState.Roaming);
        _previousState = _state;
        //this must not be here but I'm too lazy to fix this properly
        _animator.updateMode = AnimatorUpdateMode.Normal;
    }

    private void Finish()
    {
        _healthSystem.OnDied -= Die;
    }

    public void ChangeState(EnemyState state)
    {
        if (State == EnemyState.Dying) return;
        if (State == EnemyState.Dancing) return;

        if (_state != state)
        {
            _previousState = _state;
            _state = state;

            OnStateChange?.Invoke(state);
        }
    }

    private void Die()
    {
        if (!_validEnemy) return;

        _validEnemy = false;

        _movementSystem.StandStill();
        _movementSystem.SetCanMove(false);

        OnDied?.Invoke();

        ChangeState(EnemyState.Dying);

        _animator.SetTrigger("Die");

        Destroy(gameObject, 2f);
    }

    public void Win()
    {
        if (!_validEnemy) return;

        ChangeState(EnemyState.Dancing);

        _animator.SetTrigger("Win");
    }
}


public enum EnemyState
{
    None = -1,
    Roaming = 0,
    Attacking = 1,
    Pursuing = 7,
    SwappingGuns = 2,
    LookingForHealth = 3,
    Airborne = 4,
    Dying = 5,
    Dancing = 6,
}