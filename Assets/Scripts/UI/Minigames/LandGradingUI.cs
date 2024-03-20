using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(ScreenTouchController))]
public class LandGradingUI : MonoBehaviour
{
    [Header("Dev area")]
    [SerializeField] private Slider _fillSlider;

    [SerializeField] private float _fillValue;
    [SerializeField] private float _maxFill;

    [SerializeField] private bool _minigameFinished;

    [SerializeField] private JobSectorAreaSO _jobSectorAreaSO;

    [Header("GD area")]
    [SerializeField] private float _fillIdealValue;
    [SerializeField] private float _fillOffsetTolerance;

    [Header("Touch handler")]
    [SerializeField] private ScreenTouchController _screenTouchController;

    private void OnValidate()
    {
        if (_screenTouchController == null)
        {
            _screenTouchController = GetComponent<ScreenTouchController>();
        }
    }

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (_minigameFinished) return;

        if (_screenTouchController.DetectHolding())
        {
            Fill();
        }
        else if (_screenTouchController.FirstPress)
        {
            _minigameFinished = true;

            CalculateScore();
        }
    }


    private void Init()
    {
        _minigameFinished = false;

        _fillValue = 0;

        _fillSlider.minValue = 0;
        _fillSlider.value = _fillValue;
        _fillSlider.maxValue = _maxFill;
    }

    private void Fill()
    {
        _fillValue += Time.deltaTime;

        UpdateFillBar();
    }

    private void UpdateFillBar()
    {
        _fillSlider.value = _fillValue;
    }

    private void CalculateScore()
    {
        if (Mathf.Abs(_fillValue - _fillIdealValue) < _fillOffsetTolerance)
        {
            Debug.Log("Congrats");

            GameManager.Instance.UpdateGameState(GameState.PausedState);
            CanvasManager.Instance.OpenMenu(MenuType.VictoryMenu);

            _jobSectorAreaSO.FinishDay();
        }
        else
        {
            Debug.Log("Sorry bro");

            GameManager.Instance.UpdateGameState(GameState.PausedState);
            CanvasManager.Instance.OpenMenu(MenuType.GameOverMenu);
        }

        gameObject.SetActive(false);
    }
}
