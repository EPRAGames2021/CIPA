using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DraggableObject))]
public class Tile : MonoBehaviour
{
    [SerializeField] private DraggableObject _draggableObject;

    [SerializeField] private bool _validPosition;
    
    [Tooltip("This goes from center to border, left to right within a given quadrant in the shape of a diamond")]
    [SerializeField] private int _proximityToCenter;

    public int Rotation => _draggableObject.Rotation;
    public bool Attached => _draggableObject.Attached;
    public bool Static => _draggableObject.Static;
    public ObjectSlot Slot => _draggableObject.Slot;
    public ObjectSlot PreviousSlot => _draggableObject.PreviousSlot;

    public bool ValidPosition { get { return _validPosition; } set { _validPosition = value; } }
    public int ProximityToCenter => _proximityToCenter;


    private void OnValidate()
    {
        if (_draggableObject == null)
        {
            _draggableObject = GetComponent<DraggableObject>();
        }
    }
}
