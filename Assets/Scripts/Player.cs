using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private EnemyState _state = EnemyState.None;

    [SerializeField] private bool _alive;
    [SerializeField] private bool _wearingEquipment;

    [SerializeField] private Animator _animator;

    [Header("Systems")]
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private MovementSystem _movementSystem;

    public HealthSystem HealthSystem => _healthSystem;
    public MovementSystem MovementSystem => _movementSystem;

    public EnemyState State => _state;
    public bool WearingEquipment => _wearingEquipment;

    public event System.Action<EnemyState> OnStateChange;
    public event System.Action OnDied;
    public event System.Action<bool> OnEquip;

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

        _alive = true;
        _wearingEquipment = false;

        ChangeState(EnemyState.Roaming);

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
            _state = state;

            OnStateChange?.Invoke(state);
        }
    }

    private void Die()
    {
        if (!_alive) return;

        _alive = false;

        _movementSystem.StandStill();
        _movementSystem.SetCanMove(false);

        OnDied?.Invoke();

        ChangeState(EnemyState.Dying);

        _animator.SetTrigger("Die");

        Destroy(gameObject, 2f);
    }

    public void Win()
    {
        if (!_alive) return;

        ChangeState(EnemyState.Dancing);

        _animator.SetTrigger("Win");
    }

    public void Equip(bool equip)
    {
        if (_wearingEquipment == equip) return;

        _wearingEquipment = equip;

        OnEquip?.Invoke(equip);
    }
}


public enum EnemyState
{
    None = -1,
    Roaming = 0,
    Dying = 1,
    Dancing = 2,
}