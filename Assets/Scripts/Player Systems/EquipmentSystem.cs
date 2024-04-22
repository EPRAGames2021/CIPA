using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private bool _wearingEquipment;

    [SerializeField] private List<GameObject> _equipment;

    public bool WearingEquipment => _wearingEquipment;

    public event System.Action<bool> OnEquipped;


    private void OnValidate()
    {
        if (_player == null) _player = GetComponent<Player>();
    }


    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Finish();
    }


    private void Init()
    {
        EquipPlayer(false);
    }

    private void Finish()
    {
        
    }


    public void EquipPlayer(bool equip)
    {
        foreach (GameObject equipment in _equipment)
        {
            equipment.SetActive(equip);
        }

        _wearingEquipment = equip;

        OnEquipped?.Invoke(_wearingEquipment);
    }
}
