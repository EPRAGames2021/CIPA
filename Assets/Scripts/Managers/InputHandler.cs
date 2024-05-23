using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    [SerializeField] private FloatingJoystick _joystick;

    [SerializeField] private Player _player;


    private void Awake()
    {
        InitSingleton();
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

        _player.MovementSystem.InputDirection = new(_joystick.Direction.x, 0, _joystick.Direction.y);
    }
}
