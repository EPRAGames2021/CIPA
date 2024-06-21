using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class FillableContainer : MonoBehaviour
    {
        [SerializeField] private float _value;
        [SerializeField] private float _targetValue;
        [SerializeField] private float _maxValue;

        [SerializeField] private float _tolerance;

        [SerializeField] private bool _ready;

        [SerializeField] private Slider _display;
        [SerializeField] private Image _displayImage;

        public bool IsReady => _ready;


        public event System.Action OnContainerIsFull;

        private void Awake()
        {
            Init();
        }


        private void Init()
        {
            _value = 0;
        }


        public void AddToValue(float value)
        {
            _value += value;

            if (_value >= _maxValue)
            {
                OnContainerIsFull?.Invoke();
            }

            _display.value = _value;

            EvaluateReady();
        }

        public void ResetValue()
        {
            _value = 0;

            _display.value = _value;
            _display.maxValue = _maxValue;

            EvaluateReady();
        }

        private void EvaluateReady()
        {
            float difference = Mathf.Abs(_targetValue - _value);

            if (difference <= _tolerance)
            {
                _ready = true;

                _displayImage.color = Color.green;
            }
            else if (difference <= _tolerance * 2)
            {
                _ready = false;

                _displayImage.color = Color.yellow;
            }
            else
            {
                _ready = false;

                _displayImage.color = Color.red;
            }
        }
    }
}
