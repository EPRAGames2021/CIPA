using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class LogisticsAndDistributionController : ResourceVehicleTransportationController
    {
        [Header("GD Area")]
        [Tooltip("In seconds")]
        [SerializeField] private int _totalTime;

        [Header("Dev Area")]
        [Tooltip("In seconds")]
        [SerializeField] private float _timeLeft;
        [SerializeField] private bool _timerTrigerred;

        [SerializeField] private VehicleCargoLoader _vehicleCargoLoader;

        [SerializeField] private List<VehicleCargoLoader> _cargoLoaderList;

        [SerializeField] private List<GameObject> _cargoSpriteList;

        [SerializeField] private OffScreenIndicatorManager _offScreenIndicatorManager;
        [SerializeField] private List<GameObject> _offScreenIndicatorList;


        public float TimeLeft => _timeLeft;


        private void Update()
        {
            TimeBehaviour();   
        }

        protected override void Init()
        {
            base.Init();

            SetNextCargo();

            for (int i = 0; i < _cargoLoaderList.Count; i++)
            {
                _cargoLoaderList[i].OnDeliveryMade += CheckForCorrectCargo;
            }
        }

        protected override void Finish()
        {
            base.Finish();

            for (int i = 0; i < _cargoLoaderList.Count; i++)
            {
                _cargoLoaderList[i].OnDeliveryMade -= CheckForCorrectCargo;
            }
        }

        protected override void InitiateMiniGame()
        {
            base.InitiateMiniGame();

            _timeLeft = _totalTime;
            _timerTrigerred = true;

            //_offScreenIndicatorManager.SetupIndicators(_offScreenIndicatorList);

            for (int i = 0; i < _offScreenIndicatorList.Count; i++)
            {
                _offScreenIndicatorManager.AddTarget(_offScreenIndicatorList[i]);

            }

            SetNextCargo();
        }

        protected override void SetupVehicle()
        {
            base.SetupVehicle();

            _vehicleArrowSystem.SetEnabled(false);
        }

        protected override void CollectCargo()
        {
            base.CollectCargo();

            SetAllDeliverySubsActive(true);
        }

        protected override void DeliverCargo()
        {
            base.DeliverCargo();

            SetAllDeliverySubsActive(false);

            SetNextCargo();
        }

        private void SetNextCargo()
        {
            GameObject currentCargo = _deliveringSpotsList[CurrentDeliverySpotIndex].gameObject;
            currentCargo.TryGetComponent(out VehicleCargoLoader vehicleCargoLoader);

            if (vehicleCargoLoader == null)
            {
                Debug.LogError("COULDN'T GET VEHICLE CARGO LOADER");
            }

            _vehicleCargoLoader.SetCargoType(vehicleCargoLoader.CargoType);

            UpdateCurrentCargoSprite();
        }

        private void UpdateCurrentCargoSprite()
        {
            Debug.Log((int)_vehicleCargoLoader.CargoType - 1);
            
            for (int i = 0; i < _cargoSpriteList.Count; i++)
            {
                _cargoSpriteList[i].gameObject.SetActive(i == (int)_vehicleCargoLoader.CargoType - 1); // - 1 because 0 is any
            }
        }


        private void CheckForCorrectCargo(bool correct)
        {
            if (!correct)
            {
                JobAreaManager.Instance.FinishMinigame(false);
            }
        }


        private void TimeBehaviour()
        {
            if (!_timerTrigerred) return;

            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
            }
            else if (_timeLeft <= 0)
            {
                JobAreaManager.Instance.FinishMinigame(false);
            }
        }
    }
}
