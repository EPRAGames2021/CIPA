using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EPRA.Utilities;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _joystick;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _smoothSpeed;

    [SerializeField] private Vector3 _inputDirection;

    [Header("Rotation")]
    [SerializeField] private float _turnSpeed;

    private float _currentAngle;
    private float _targetAngle;

    private bool _canMove;
    private bool _movimentBlocked;

    private Animator _animator;
    [HideInInspector] public Rigidbody _rb;

    private void Start()
    {
        InitializeVariables();
        InitializeComponents();
    }

    private void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    private void FixedUpdate()
    {
        if (_movimentBlocked) return;

        if (!_canMove) return;


        _rb.MoveRotation(Quaternion.Euler(Vector3.up * _currentAngle));
        //_rb.MovePosition(_positionToMove);
        //_rb.velocity = _inputDirection * _moveSpeed;
        _rb.velocity = new(_inputDirection.x * _moveSpeed, _inputDirection.y, _inputDirection.z * _moveSpeed);
    }


    private void InitializeVariables()
    {
        _canMove = true;
    }

    private void InitializeComponents()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }


    private void HandleMovement()
    {
        if (_movimentBlocked) return;

        float yVelocity = _rb.velocity.y;

        _inputDirection.Set(_joystick.Direction.x, yVelocity, _joystick.Direction.y);

        if (_animator != null)
        {
            float inputMagnitude = Mathf.Abs(_inputDirection.x) + Mathf.Abs(_inputDirection.z);

            _animator.SetBool("IsWalking", inputMagnitude > 0.05f && inputMagnitude < 0.5f && _canMove);
            _animator.SetBool("IsRunning", inputMagnitude >= 0.5f && _canMove);
        }
    }

    private void HandleRotation()
    {
        _targetAngle = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg;
        _currentAngle = Mathf.LerpAngle(_currentAngle, _targetAngle, _turnSpeed * _inputDirection.magnitude * Time.deltaTime);
    }


    public void StandStill()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

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
            _animator.SetBool("InAir", true);

            yield return new WaitForSeconds(disableTime);

            _canMove = true;
            _animator.SetBool("InAir", false);
        }
    }

    public void Initialize()
    {
        InitializeVariables();
        StandStill();
    }

    public void SetCanMove(bool value)
    {
        _canMove = value;
    }
}
