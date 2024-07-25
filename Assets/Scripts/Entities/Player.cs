using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private CharacterState _state = CharacterState.None;

        [SerializeField] private Animator _animator;

        [Header("Systems")]
        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private MovementSystem _movementSystem;
        [SerializeField] private EquipmentSystem _equipmentSystem;
        [SerializeField] private ArrowSystem _arrowSystem;

        public HealthSystem HealthSystem => _healthSystem;
        public MovementSystem MovementSystem => _movementSystem;
        public EquipmentSystem EquipmentSystem => _equipmentSystem;
        public ArrowSystem ArrowSystem => _arrowSystem;

        public CharacterState State => _state;


        public event System.Action<CharacterState> OnStateChange;
        public event System.Action OnDied;


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

            ChangeState(CharacterState.Roaming);
        }

        private void Finish()
        {
            _healthSystem.OnDied -= Die;
        }

        public void ChangeState(CharacterState state)
        {
            if (State == CharacterState.Dying) return;
            //if (State == CharacterState.Dancing) return;

            if (_state != state)
            {
                _state = state;

                OnStateChange?.Invoke(state);
            }
        }

        private void Die()
        {
            Vibrator.Vibrate(300);

            _movementSystem.StandStill();
            _movementSystem.CanMove = false;

            OnDied?.Invoke();

            ChangeState(CharacterState.Dying);
        }

        public void Win()
        {
            _movementSystem.StandStill();
            _movementSystem.CanMove = false;

            ChangeState(CharacterState.Dancing);
        }

        public void Refresh()
        {
            ChangeState(CharacterState.Roaming);

            _healthSystem.Refresh();
            _movementSystem.Refresh();
        }
    }


    public enum CharacterState
    {
        None = -1,
        Roaming = 0,
        Dying = 1,
        Dancing = 2,
        Talking = 3,
        Carrying = 4,
        Looking = 5,
    }
}