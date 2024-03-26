using ES3Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDelta : MonoBehaviour
{
    [SerializeField] private Vector2 _lastMousePosition;
    [SerializeField] private Vector2 _mousePositionDelta;
    [SerializeField] private float _distance;
    [SerializeField] private float _speed;

    public Vector2 LastMousePosition => _lastMousePosition;
    public Vector2 MousePositionDelta => _mousePositionDelta;
    public float Distance => _distance;
    public float Speed => _speed;

    private void Start()
    {
        Init();
    }

    void Update()
    {
        Vector2 currentMousePosition = Input.mousePosition;

        _mousePositionDelta = currentMousePosition - _lastMousePosition;

        _distance = Vector2.Distance(currentMousePosition, _lastMousePosition);

        _lastMousePosition = currentMousePosition;

        _speed = _distance / Time.deltaTime;
    }


    private void Init()
    {
        _lastMousePosition = Input.mousePosition;
    }
}
