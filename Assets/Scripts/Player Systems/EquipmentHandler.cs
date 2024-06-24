using UnityEngine;

public enum EquipmentType
{
    None = 0,
    Gloves = 1,
    Helmet = 2,
    Jacket = 3,
    Headphones = 4,
    Glasses = 5,
    Mask = 6,
    EarPlug = 7,
    AntiStaticBoots = 8,
    FaceShield= 9,
    GasMask = 10,
    PVCJacket = 11,
    SteelTipBoots = 12,
}

[System.Serializable]
public class EquipmentHandler
{
    [SerializeField] private bool _equipped;
    [SerializeField] private GameObject _object;
    [SerializeField] private EquipmentType _equipmentType;

    public bool Equipped => _equipped;
    public GameObject Object => _object;
    public EquipmentType EquipmentType => _equipmentType;


    public void Equip(bool equip)
    {
        _equipped = equip;

        _object.SetActive(_equipped);
    }
}
