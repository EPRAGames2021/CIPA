using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class DamageIndicatorSystem : MonoBehaviour
    {
        [SerializeField] private CurrencySO _currencySO;

        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DamageText _textIndicatorPrefab;

        private System.Action<int> OnValueIncreasedHandler;
        private System.Action<int> OnValueDecreasedHandler;


        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            Finish();
        }


        private void Init()
        {
            OnValueIncreasedHandler = value => SpawnTextIndicator(true, value);
            OnValueDecreasedHandler = value => SpawnTextIndicator(false, value);

            _currencySO.OnValueIncreased += OnValueIncreasedHandler;
            _currencySO.OnValueDecreased += OnValueDecreasedHandler;
        }

        private void Finish()
        {
            _currencySO.OnValueIncreased -= OnValueIncreasedHandler;
            _currencySO.OnValueDecreased -= OnValueDecreasedHandler;
        }


        private void SpawnTextIndicator(bool increased, int value)
        {
            DamageText damageText = Instantiate(_textIndicatorPrefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);

            damageText.Setup(value.ToString(), increased);

            Destroy(damageText.gameObject, 2f);
        }
    }
}
