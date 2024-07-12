using System.Collections;
using UnityEngine;

namespace CIPA
{
    public class MovementSystem : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;

        [Header("Movement")]
        [SerializeField] private Vector3 _inputDirection;
        [SerializeField] private Vector3 _movementDirection;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _turnSpeed;

        [SerializeField] private float _baseSpeed;
        [SerializeField] private float _restrictedSpeed;

        [Header("Optional")]
        [Tooltip("Useful to orientate movement relative to camera rather than world")]
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private bool _moveRelativeToCamera;

        [Header("Debug")]
        [SerializeField] private float _currentAngle;
        [SerializeField] private float _targetAngle;

        [SerializeField] private bool _canMove;
        [SerializeField] private bool _movimentBlocked;

        public bool CanMove { get { return _canMove; } set { _canMove = value; } }
        public Vector3 InputDirection { get { return _inputDirection; } set { _inputDirection = value; } }


        private void OnValidate()
        {
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Init();
        }

        private void FixedUpdate()
        {
            if (_movimentBlocked) return;
            if (!_canMove) return;

            HandleRotationInput();
            HandleMovementInput();

            _rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * _currentAngle));
            _rigidbody.velocity = new(_movementDirection.x * _moveSpeed, _rigidbody.velocity.y, _movementDirection.z * _moveSpeed);
        }


        private void Init()
        {
            _canMove = true;

            RestrictMovement(false);
        }


        private void HandleMovementInput()
        {
            if (_movimentBlocked) return;

            float inputMagnitude;

            if (_cameraTransform != null && _moveRelativeToCamera)
            {
                float referenceYRotation = _cameraTransform.transform.eulerAngles.y;
                Vector3 adjustedDirection = Quaternion.Euler(0, referenceYRotation, 0) * _inputDirection;

                inputMagnitude = Mathf.Abs(adjustedDirection.x) + Mathf.Abs(adjustedDirection.z);

                _movementDirection = adjustedDirection;
            }
            else
            {
                inputMagnitude = Mathf.Abs(_inputDirection.x) + Mathf.Abs(_inputDirection.z);

                _movementDirection = _inputDirection;
            }


            if (_animator == null) return;

            _animator.SetBool("IsWalking", _rigidbody.velocity.magnitude > 0.05f && _rigidbody.velocity.magnitude < _baseSpeed * 0.65f);
            _animator.SetBool("IsRunning", _rigidbody.velocity.magnitude >= _baseSpeed * 0.65f);
            //_animator.SetBool("IsWalking", inputMagnitude > 0.05f && inputMagnitude < 0.5f && _canMove);
            //_animator.SetBool("IsRunning", inputMagnitude >= 0.5f && _canMove);
        }

        private void HandleRotationInput()
        {
            if (_cameraTransform != null && _moveRelativeToCamera)
            {
                float referenceYRotation = _cameraTransform.transform.eulerAngles.y;
                Vector3 adjustedDirection = Quaternion.Euler(0, referenceYRotation, 0) * _inputDirection;

                _targetAngle = Mathf.Atan2(adjustedDirection.x, adjustedDirection.z) * Mathf.Rad2Deg;
            }
            else
            {
                _targetAngle = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg;
            }

            _currentAngle = Mathf.LerpAngle(_currentAngle, _targetAngle, _turnSpeed * _inputDirection.magnitude * Time.deltaTime);

            if (_currentAngle <= -360) _currentAngle *= -1;
            if (_currentAngle >= 360) _currentAngle %= 360;
        }


        public void StandStill()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _inputDirection.Set(0f, 0f, 0f);

            if (_animator != null)
            {
                _animator.SetBool("IsWalking", false);
                _animator.SetBool("IsRunning", false);
            }
        }

        public void TemporarilyDisableMovement(float disableTime)
        {
            _canMove = false;

            StartCoroutine(ReenableDelay());
            IEnumerator ReenableDelay()
            {
                yield return new WaitForSeconds(disableTime);

                _canMove = true;
            }
        }

        public void Refresh()
        {
            Init();
        }

        public void RestrictMovement(bool restrict)
        {
            _moveSpeed = restrict ? _restrictedSpeed : _baseSpeed;
        }
    }
}
