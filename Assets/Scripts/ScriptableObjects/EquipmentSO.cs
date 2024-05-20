using UnityEngine;
using EPRA.Utilities;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Objects/Equipment", order = 1)]

public class EquipmentSO : ResourceSO
{
    [SerializeField] private EquipmentType _type;

    public EquipmentType Type => _type;
}
