using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Objects/Equipment", order = 1)]

public class EquipmentSO : ResourceSO
{
    [SerializeField] private EquipmentType _type;

    public EquipmentType Type => _type;
}
