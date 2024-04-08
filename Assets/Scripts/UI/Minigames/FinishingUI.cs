using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenTouchController))]
[RequireComponent(typeof(MouseDelta))]
public class FinishingUI : MonoBehaviour
{
    [Header("Dev area")]
    [SerializeField] private Slider _paintSlider;

    [SerializeField] private float _averageSpeed;
    [SerializeField] private int _logsSize;
    [SerializeField] private List<float> _speedLogs = new();

    [SerializeField] private float _paintTimeWrongSpeed;
    [SerializeField] private float _paintTime;

    [SerializeField] private bool _paintFinished;

    [Header("GD area")]
    [SerializeField] private float _paintMaxTime;
    [SerializeField] private float _paintMaxTimeWrongSpeed;
    [SerializeField] private float _idealSpeed;
    [Range(0, 100), SerializeField] private float _speedTolerancePercent;
    [SerializeField] private float _paintMinTolerableSpeed;
    [SerializeField] private float _paintMaxTolerableSpeed;

    [Header("Touch handler")]
    [SerializeField] private ScreenTouchController _screenTouchController;
    [SerializeField] private MouseDelta _mouseDelta;

    public float AverageSpeed => _averageSpeed;
    public float CompletionPercentage => (_paintTime / _paintMaxTime) * 100;

    public event System.Action<bool> OnPaintSucceeded;


    private void OnValidate()
    {
        _paintSlider.maxValue = _idealSpeed * 2;

        _paintMaxTolerableSpeed = _idealSpeed + ((_idealSpeed / 100) * _speedTolerancePercent);
        _paintMinTolerableSpeed = _idealSpeed - ((_idealSpeed / 100) * _speedTolerancePercent);
    }

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (_paintFinished) return;

        if (_screenTouchController.FirstPress && _screenTouchController.DetectHolding())
        {
            CalculateAverageSpeed();
            Paint();
        }

        CheckForMiniGameCompletion();
    }


    private void Init()
    {
        _averageSpeed = 0;

        _speedLogs.Clear();
        for (int i = 0; i < _logsSize; i++)
        {
            _speedLogs.Add(_idealSpeed);
        }

        _paintFinished = false;
        _paintTimeWrongSpeed = 0.0f;
        _paintTime = 0.0f;

        _paintSlider.minValue = 0;
        _paintSlider.value = _idealSpeed;
        _paintSlider.maxValue = _idealSpeed * 2;
    }

    private void CalculateAverageSpeed()
    {
        _speedLogs.Add(_mouseDelta.Speed / 1000);

        if (_speedLogs.Count > _logsSize)
        {
            _speedLogs.RemoveAt(0);
        }

        float total = 0;

        foreach (float speed in _speedLogs)
        {
            total += speed;
        }

        _averageSpeed = total / _speedLogs.Count;
    }

    private void Paint()
    {
        bool tolerableSpeed = _averageSpeed > _paintMinTolerableSpeed && _averageSpeed < _paintMaxTolerableSpeed;

        if (tolerableSpeed)
        {
            _paintTime += Time.deltaTime;
        }
        else if (!tolerableSpeed && _mouseDelta.Distance > 0)
        {
            _paintTimeWrongSpeed += Time.deltaTime;
        }

        UpdatePaintBar();
    }

    private void CheckForMiniGameCompletion()
    {
        if (_paintTime >= _paintMaxTime)
        {
            OnPaintSucceeded?.Invoke(true);

            _paintFinished = true;
        }
        else if (_paintTimeWrongSpeed >= _paintMaxTimeWrongSpeed)
        {
            OnPaintSucceeded?.Invoke(false);

            _paintFinished = true;
        }
    }

    private void UpdatePaintBar()
    {
        _paintSlider.value = _averageSpeed;
    }
}
