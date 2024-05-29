using EPRA.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private InputAction _movementAction;

    [SerializeField] private Player _player;

    [SerializeField] private float x;
    [SerializeField] private float z;

    [SerializeField] private InputAction _openSettingsAction;

    private float X
    { 
        get
        { 
            return x; 
        } 
        set 
        {
            if (value > 1f) value = 1f;
            else if (value < -1f) value = -1f;

            x = value; 
        }
    }

    private float Z
    {
        get
        {
            return z;
        }
        set
        {
            if (value > 1f) value = 1f;
            else if (value < -1f) value = -1f;

            z = value;
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


    public void SetPlayer(Player player)
    {
        _player = player;
    }

    private void HandleMovement()
    {
        if (_player == null) return;
        if (_player.MovementSystem == null) return;

        X = _joystick.Direction.x + _movementAction.ReadValue<Vector2>().x;
        Z = _joystick.Direction.y + _movementAction.ReadValue<Vector2>().y;

        _player.MovementSystem.InputDirection = new(x, 0, z);
    }

    private void OpenSettings(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.State.Equals(GameState.GameState)) return;

        CanvasManager.Instance.SwitchSettings();
    }
}
