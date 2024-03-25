using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenTouchController))]
[RequireComponent(typeof(MouseDelta))]
public class FoundationAndStructureUI : MonoBehaviour
{
    [Header("Dev area")]
    [SerializeField] private Slider _mixSlider;

    [SerializeField] private float _averageSpeed;
    [SerializeField] private int _logsSize;
    [SerializeField] private List<float> _speedLogs = new List<float>();

    [SerializeField] private float _mixTimeWrongSpeed;
    [SerializeField] private float _mixTime;

    [SerializeField] private bool _minigameFinished;

    [Header("GD area")]
    [SerializeField] private float _mixMaxTime;
    [SerializeField] private float _mixMaxTimeWrongSpeed;
    [SerializeField] private float _mixMinTolerableSpeed;
    [SerializeField] private float _mixMaxTolerableSpeed;

    [Header("Touch handler")]
    [SerializeField] private ScreenTouchController _screenTouchController;
    [SerializeField] private MouseDelta _mouseDelta;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI _mixTimeText;
    [SerializeField] private TextMeshProUGUI _mixTimeWrongText;

    private void OnValidate()
    {
        if (_screenTouchController == null)
        {
            _screenTouchController = GetComponent<ScreenTouchController>();
        }

        if (_mouseDelta == null)
        {
            _mouseDelta = GetComponent<MouseDelta>();
        }

        _mixSlider.value = _mixSlider.maxValue / 2;
    }

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (_minigameFinished) return;

        if (_screenTouchController.FirstPress && _screenTouchController.DetectHolding())
        {
            CalculateAverageSpeed();
            Mix();
        }

        CheckForMiniGameCompletion();

        _mixTimeText.text = "Mix Time Right: " + _mixTime;
        _mixTimeWrongText.text = "Mix Time Wrong: " + _mixTimeWrongSpeed;
    }


    private void Init()
    {
        _averageSpeed = 0;
        _speedLogs.Clear();

        _minigameFinished = false;
        _mixTimeWrongSpeed = 0.0f;
        _mixTime = 0.0f;

        _mixSlider.minValue = 0;
        _mixSlider.value = _mixSlider.maxValue / 2;
        _mixSlider.maxValue = 1;
    }

    private void CalculateAverageSpeed()
    {
        _speedLogs.Add(_mouseDelta.Speed);
        
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
            //Debug.Log("you win");
            JobAreaManager.Instance.MinigameSuccessed();

            _minigameFinished = true;
        }
        else if (_mixTimeWrongSpeed >= _mixMaxTimeWrongSpeed)
        {
            //Debug.Log("you lost");
            JobAreaManager.Instance.MinigameFailed();

            _minigameFinished = true;
        }

        if (_minigameFinished)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateMixBar()
    {
        _mixSlider.value = _averageSpeed;
    }
}
