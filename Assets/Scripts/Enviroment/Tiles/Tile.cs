using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DraggableObject))]
public class Tile : MonoBehaviour
{
    [SerializeField] private DraggableObject _draggableObject;

    [SerializeField] private bool _validPosition;

    [SerializeField] private int _id;

    public ObjectSlot Slot => _draggableObject.Slot;

    [Header("Debug")]
    [SerializeField] private int _originalQuadrant;
    [SerializeField] private int _quadrant;
    [SerializeField] private int _rotationsAntiClockWise;

    public bool ValidPosition { get { return _validPosition; } set { _validPosition = value; } }
    public int ID => _id;


    private void OnValidate()
    {
        if (_draggableObject == null)
        {
            _draggableObject = GetComponent<DraggableObject>();
        }
    }

    private void Start()
    {
        _draggableObject.OnObjectAttached += CheckValidPosition;
        _draggableObject.OnObjectRotated += CheckValidPosition;
    }

    private void OnDestroy()
    {
        _draggableObject.OnObjectAttached -= CheckValidPosition;
        _draggableObject.OnObjectRotated -= CheckValidPosition;
    }


    private void CheckValidPosition()
    {
        if (Slot == null) return;

        Slot.TryGetComponent(out TileSlot tileSlot);

        if (tileSlot == null) return;

        ValidPosition = _id == tileSlot.ID;
    }
}
