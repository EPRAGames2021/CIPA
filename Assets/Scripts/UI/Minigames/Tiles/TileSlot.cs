using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSlot : ObjectSlot
{
    [Tooltip("Starts at 1 and goes clockwise")]
    [SerializeField] private int _quadrant;

    [Tooltip("This goes from center to border, left to right within a given quadrant in the shape of a diamond")]
    [SerializeField] private int _proximityToCenter;

    public int Quadrant => _quadrant;
    public int ProximityToCenter => _proximityToCenter;

    public override void AttachObject(DraggableObject draggableObject)
    {
        base.AttachObject(draggableObject);
                
        if (Object.TryGetComponent(out Tile tile))
        {
            bool correctProximity = tile.ProximityToCenter == _proximityToCenter;

            Debug.Log("Is proximity correct? " + correctProximity);

            bool correctOrientation = ((tile.Rotation % 90) + 1) == Quadrant;

            Debug.Log("Is orientation correct? " + correctOrientation);

            tile.ValidPosition = correctProximity && correctOrientation;
        }
    }

    public override void DetachObject()
    {
        if (Object.TryGetComponent(out Tile tile))
        {
            tile.ValidPosition = false;
        }

        base.DetachObject();
    }
}
