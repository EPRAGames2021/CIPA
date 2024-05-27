using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EPRA.Utilities;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Movement")]
    [SerializeField] private Vector3 _inputDirection;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _turnSpeed;

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

    private void LateUpdate()
    {
        HandleRotationInput();
        HandleMovementInput();
    }

    private void FixedUpdate()
    {
        if (_movimentBlocked) return;
        if (!_canMove) return;

        _rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * _currentAngle));
        _rigidbody.velocity = new(_inputDirection.x * _moveSpeed, _rigidbody.velocity.y, _inputDirection.z * _moveSpeed);
    }


    private void Init()
    {
        _canMove = true;
    }


    private void HandleMovementInput()
    {
        if (_movimentBlocked) return;
        if (_animator == null) return;


        float inputMagnitude = Mathf.Abs(_inputDirection.x) + Mathf.Abs(_inputDirection.z);

        _animator.SetBool("IsWalking", inputMagnitude > 0.05f && inputMagnitude < 0.5f && _canMove);
        _animator.SetBool("IsRunning", inputMagnitude >= 0.5f && _canMove);
    }

    private void HandleRotationInput()
    {
        _targetAngle = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg;

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
}
