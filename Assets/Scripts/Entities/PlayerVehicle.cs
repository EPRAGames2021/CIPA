using CIPA;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicle : MonoBehaviour
{
    [SerializeField] private CargoType _cargoType;

    [SerializeField] private List<GameObject> _trunkContent;

    public bool IsCarrying => _cargoType != CargoType.None;
    public CargoType CargoType => _cargoType;


    public event System.Action<bool> OnCarryingChanged;


    private void OnValidate()
    {
        SetCarrying(_cargoType, false);
    }

    private void OnEnable()
    {
        SetCarrying(_cargoType, false);
    }


    public void SetCarrying(CargoType cargo, bool invokeEvent = true)
    {
        if (_cargoType == cargo) return;

        _cargoType = cargo;

        if (invokeEvent)
        {
            OnCarryingChanged?.Invoke(IsCarrying);
        }

        for (int i = 0; i < _trunkContent.Count; i++)
        {
            _trunkContent[i].SetActive(IsCarrying);
        }
    }
}
