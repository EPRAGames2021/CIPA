using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    [SerializeField] private List<TileSlot> _tileSlot;


    [Header("Debug")]
    [SerializeField] private int _gridSize;
}
