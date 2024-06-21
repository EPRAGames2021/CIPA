using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSlot : ObjectSlot
{
    [SerializeField] private int _id;

    public int ID => _id;
}
