using UnityEngine;
using UnityEngine.UI;

public class PouringPanel : MonoBehaviour
{
    [Header("Dev area")]
    [SerializeField] private Slider _fillSlider;

    [SerializeField] private float _fillValue;
    [SerializeField] private float _maxFill;

    [SerializeField] private bool _pouringFinished;

    [Header("GD area")]
    [SerializeField] private float _fillIdealValue;
    [SerializeField] private float _fillOffsetTolerance;

    [Header("Touch handler")]
    [SerializeField] private ScreenTouchController _screenTouchController;


    public event System.Action<bool> OnPouringSucceeded;


    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (_pouringFinished) return;

        if (_screenTouchController.DetectHolding())
        {
            Fill();
        }
        else if (_screenTouchController.FirstPress)
        {
            _pouringFinished = true;

            InvokeOnPouring();
        }
    }


    private void Init()
    {
        _pouringFinished = false;

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

    private void InvokeOnPouring()
    {
        bool succeeded = Mathf.Abs(_fillValue - _fillIdealValue) < _fillOffsetTolerance;

        OnPouringSucceeded?.Invoke(succeeded);
    }
}
