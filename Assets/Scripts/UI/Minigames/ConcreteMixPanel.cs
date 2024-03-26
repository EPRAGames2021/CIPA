using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConcreteMixPanel : MonoBehaviour
{
    [Header("Dev area")]
    [SerializeField] private Slider _mixSlider;

    [SerializeField] private float _averageSpeed;
    [SerializeField] private int _logsSize;
    [SerializeField] private List<float> _speedLogs = new List<float>();

    [SerializeField] private float _mixTimeWrongSpeed;
    [SerializeField] private float _mixTime;

    [SerializeField] private bool _mixFinished;

    [SerializeField] private GameObject _concreteBarrel;

    [Header("GD area")]
    [SerializeField] private float _mixMaxTime;
    [SerializeField] private float _mixMaxTimeWrongSpeed;
    [SerializeField] private float _idealSpeed;
    [Range(0, 100), SerializeField] private float _speedTolerancePercent;
    [SerializeField] private float _mixMinTolerableSpeed;
    [SerializeField] private float _mixMaxTolerableSpeed;

    [Header("Touch handler")]
    [SerializeField] private ScreenTouchController _screenTouchController;
    [SerializeField] private MouseDelta _mouseDelta;


    public event System.Action<bool> OnMixSucceeded;


    private void OnValidate()
    {
        _mixSlider.maxValue = _idealSpeed * 2;

        _mixMaxTolerableSpeed = _idealSpeed + ((_idealSpeed / 100) * _speedTolerancePercent);
        _mixMinTolerableSpeed = _idealSpeed - ((_idealSpeed / 100) * _speedTolerancePercent);
    }

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (_mixFinished) return;

        if (_screenTouchController.FirstPress && _screenTouchController.DetectHolding())
        {
            CalculateAverageSpeed();
            Mix();
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

        _mixFinished = false;
        _mixTimeWrongSpeed = 0.0f;
        _mixTime = 0.0f;

        _mixSlider.minValue = 0;
        _mixSlider.value = _idealSpeed;
        _mixSlider.maxValue = _idealSpeed * 2;
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

        _concreteBarrel.transform.Rotate(0, 0, _averageSpeed);
    }

    private void Mix()
    {
        bool tolerableSpeed = _averageSpeed > _mixMinTolerableSpeed && _averageSpeed < _mixMaxTolerableSpeed;

        if (tolerableSpeed)
        {
            _mixTime += Time.deltaTime;
        }
        else if (!tolerableSpeed && _mouseDelta.Distance > 0)
        {
            _mixTimeWrongSpeed += Time.deltaTime;
        }

        UpdateMixBar();
    }

    private void CheckForMiniGameCompletion()
    {
        if (_mixTime >= _mixMaxTime)
        {
            OnMixSucceeded?.Invoke(true);

            _mixFinished = true;
        }
        else if (_mixTimeWrongSpeed >= _mixMaxTimeWrongSpeed)
        {
            OnMixSucceeded?.Invoke(false);

            _mixFinished = true;
        }
    }

    private void UpdateMixBar()
    {
        _mixSlider.value = _averageSpeed;
    }
}
