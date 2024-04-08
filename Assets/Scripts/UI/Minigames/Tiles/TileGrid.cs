using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    [SerializeField] private bool _gridIsCorrect;

    [SerializeField] private List<TileSlot> _tileSlot;

    [SerializeField] private List<TileSlot> _displaySlots;


    private void Start()
    {
        Init();
    }


    private void Init()
    {
        _gridIsCorrect = false;
    }

    public void ResetGrid()
    {
        foreach (ObjectSlot slot in _tileSlot)
        {
            slot.ResetSlot();
        }

        foreach (ObjectSlot slot in _displaySlots)
        {
            slot.ResetSlot();
        }
    }

    public void LockGrid()
    {
        foreach (ObjectSlot slot in _tileSlot)
        {
            slot.LockSlot();
        }

        foreach (ObjectSlot slot in _displaySlots)
        {
            slot.LockSlot();
        }
    }


    public bool CheckForCorrectGrid()
    {
        foreach (TileSlot tile in _tileSlot)
        {
            if (!tile.Full)
            {
                return false;
            }
        }

        List<Tile> tiles = new();

        foreach (ObjectSlot slot in _tileSlot)
        {
            if (slot.Full)
            {
                tiles.Add(slot.Object.GetComponent<Tile>());
            }
        }

        _gridIsCorrect = true;

        foreach (Tile tile in tiles)
        {
            if (!tile.ValidPosition)
            {
                _gridIsCorrect = false;
            }
        }

        return _gridIsCorrect;
    }
}
