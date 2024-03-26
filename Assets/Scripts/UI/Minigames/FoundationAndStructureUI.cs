using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenTouchController))]
[RequireComponent(typeof(MouseDelta))]
public class FoundationAndStructureUI : MonoBehaviour
{
    [Header("Dev area")]
    [SerializeField] private GameObject _ingredientsPanel;
    [SerializeField] private GameObject _mixPanel;

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
    [SerializeField] private float _idealSpeed;
    [Range(0, 100), SerializeField] private float _speedTolerancePercent;
    [SerializeField] private float _mixMinTolerableSpeed;
    [SerializeField] private float _mixMaxTolerableSpeed;

    [Header("Touch handler")]
    [SerializeField] private ScreenTouchController _screenTouchController;
    [SerializeField] private MouseDelta _mouseDelta;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI _mixTimeText;
    [SerializeField] private TextMeshProUGUI _mixTimeWrongText;
    [SerializeField] private TextMeshProUGUI _averageSpeedText;
    [SerializeField] private TextMeshProUGUI _logsSizeText;

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

        //_mixSlider.value = _mixSlider.maxValue / 2;
        _mixMaxTolerableSpeed = _idealSpeed + ((_idealSpeed / 100) * _speedTolerancePercent);
        _mixMinTolerableSpeed = _idealSpeed - ((_idealSpeed / 100) * _speedTolerancePercent);
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
        _averageSpeedText.text = "Average Speed: " + _averageSpeed;
        _logsSizeText.text = "Speed Logs Size: " + _speedLogs.Count;
    }


    private void Init()
    {
        //_ingredientsPanel.SetActive(true);
        //_mixPanel.SetActive(false);

        _averageSpeed = 0;
        //_logsSize = Application.targetFrameRate;

        _speedLogs.Clear();
        for (int i = 0; i < _logsSize; i++)
        {
            _speedLogs.Add(_idealSpeed);
        }

        _minigameFinished = false;
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
