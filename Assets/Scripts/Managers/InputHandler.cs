using UnityEngine;
using UnityEngine.InputSystem;
using EPRA.Utilities;

namespace CIPA
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler Instance;

        [SerializeField] private FloatingJoystick _joystick;
        [SerializeField] private InputAction _movementAction;

        [SerializeField] private MovementSystem _movementSystem;

        [SerializeField] private float _x;
        [SerializeField] private float _z;

        [SerializeField] private InputAction _openSettingsAction;

        private float X
        {
            get
            {
                return _x;
            }
            set
            {
                if (value > 1f) value = 1f;
                else if (value < -1f) value = -1f;

                _x = value;
            }
        }
        private float Z
        {
            get
            {
                return _z;
            }
            set
            {
                if (value > 1f) value = 1f;
                else if (value < -1f) value = -1f;

                _z = value;
            }
        }



        private void Awake()
        {
            InitSingleton();
        }

        private void OnEnable()
        {
            _movementAction.Enable();
            _openSettingsAction.Enable();

            _openSettingsAction.performed += OpenSettings;
        }

        private void OnDisable()
        {
            _movementAction.Disable();
            _openSettingsAction.Disable();

            _openSettingsAction.performed -= OpenSettings;
        }

        private void Update()
        {
            HandleMovement();
        }


        private void InitSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetMovementSystem(MovementSystem system)
        {
            _movementSystem = system;
        }

        private void HandleMovement()
        {
            if (_movementSystem == null) return;

            X = _joystick.Direction.x + _movementAction.ReadValue<Vector2>().x;
            Z = _joystick.Direction.y + _movementAction.ReadValue<Vector2>().y;

            _movementSystem.InputDirection = new(X, 0, Z);
        }

        private void OpenSettings(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.State.Equals(GameState.GameState)) return;

            CanvasManager.Instance.SwitchSettings();
        }
    }
}
