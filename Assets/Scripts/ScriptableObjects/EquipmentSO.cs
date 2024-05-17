using UnityEngine;
using EPRA.Utilities;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Objects/Equipment", order = 1)]

public class EquipmentSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _nameKey;

    [SerializeField] private Sprite _icon;

    [SerializeField] private EquipmentType _type;

    public string Name => _name;
    public string NameKey => _nameKey;
    public Sprite Icon => _icon;
    public EquipmentType Type => _type;
}
