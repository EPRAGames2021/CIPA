using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private MovementSystem _movementSystem;

    [SerializeField] private bool _wearingEquipment;

    [SerializeField] private List<EquipmentHandler> _equipmentHandlers;

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


    private void Init()
    {
        //EquipPlayer(false);
        foreach (EquipmentHandler equipmentHandler in _equipmentHandlers)
        {
            equipmentHandler.Equip(false);
        }
    }


    public void EquipPlayer(List<EquipmentType> equipTypes, bool enable)
    {
        foreach (EquipmentType equipmentType in equipTypes)
        {
            EquipEquipment(equipmentType, enable);
        }

        _wearingEquipment = enable;

        if (_wearingEquipment)
        {
            _particleSystem.Play();
            _animator.SetTrigger("Spin");
            _movementSystem.StandStill();
            _movementSystem.TemporarilyDisableMovement(4);
        }

        OnEquipped?.Invoke(_wearingEquipment);
    }

    private void EquipEquipment(EquipmentType type, bool enable)
    {
        _equipmentHandlers.FirstOrDefault(equipment => equipment.EquipmentType == type)?.Equip(enable);
    }
}