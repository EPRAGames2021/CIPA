using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class VehicleCargoLoader : MonoBehaviour
    {
        [SerializeField] private PlayerVehicleDetector _vehicleDetector;
        [SerializeField] private bool _isDeliveryPoint;
        [SerializeField] private CargoType _cargoType;

        public bool IsDeliveryPoint => _isDeliveryPoint;
        public CargoType CargoType => _cargoType;


        public event System.Action<bool> OnDeliveryMade;


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
            _vehicleDetector.OnPlayerVehicleDetected += ChangeVehicleCargo;
        }

        private void Finish()
        {
            _vehicleDetector.OnPlayerVehicleDetected -= ChangeVehicleCargo;
        }


        private void ChangeVehicleCargo(PlayerVehicle playerVehicle)
        {
            if (playerVehicle.IsCarrying && IsDeliveryPoint)
            {
                if (_cargoType == CargoType.Any)
                {
                    OnDeliveryMade?.Invoke(true);
                }
                else
                {
                    OnDeliveryMade?.Invoke(playerVehicle.CargoType == _cargoType);
                }

                playerVehicle.SetCarrying(CargoType.None);
            }
            else if (!playerVehicle.IsCarrying && !IsDeliveryPoint)
            {
                playerVehicle.SetCarrying(_cargoType);
            }

        }

        public void SetCargoType(CargoType cargoType)
        {
            _cargoType = cargoType;
        }

        public void RandomizeCargoType()
        {
            _cargoType = GetRandomEnumValue<CargoType>();
        }

        private T GetRandomEnumValue<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            int randomIndex = UnityEngine.Random.Range(1, values.Length);
            return (T)values.GetValue(randomIndex);
        }
    }

    public enum CargoType
    {
        None = -1,
        Any = 0,
        Truck = 1,
        Pickup = 2,
        Train = 3,
        Docks = 4,
        Airplane = 5,
    }
}
