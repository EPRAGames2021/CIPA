using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    [CreateAssetMenu(fileName = "Currency", menuName = "Scriptable Objects/Currency", order = 1)]
    public class CurrencySO : ScriptableObject
    {
        [Header("Value")]
        [SerializeField] private int _value;
        [SerializeField] private Sprite _spriteIcon;

        [SerializeField] private string _saveName;
        [SerializeField] private bool _canGoNegative;

        public int Value => _value;
        public Sprite Icon => _spriteIcon;

        public event System.Action<int> OnChangeValue;

        private void OnEnable()
        {
#if !UNITY_EDITOR
        LoadValue();
#endif
        }


        public void AddToCurrency(int value)
        {
            _value += value;

            OnChangeValue?.Invoke(_value);

            SaveValue();
        }

        public void RemoveFromCurrency(int value)
        {
            _value -= value;

            if (!_canGoNegative && _value < 0)
            {
                _value = 0;
            }

            OnChangeValue?.Invoke(_value);

            SaveValue();
        }

        public void SetCurrencyValue(int value)
        {
            _value = value;

            OnChangeValue?.Invoke(_value);

            SaveValue();
        }

        public bool AttemptPurchase(int value)
        {
            if (_value - value >= 0)
            {
                RemoveFromCurrency(value);

                return true;
            }
            else
            {
                return false;
            }
        }


        private void LoadValue()
        {
            _value = DataManager.HasData(_saveName) ? DataManager.LoadData<int>(_saveName) : 0;
        }

        private void SaveValue()
        {
            DataManager.SaveData<int>(_saveName, _value);
        }
    }
}
