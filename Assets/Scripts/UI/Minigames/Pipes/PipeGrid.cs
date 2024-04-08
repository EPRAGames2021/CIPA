using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGrid : MonoBehaviour
{
    [SerializeField] private bool _gridIsCorrect;

    [SerializeField] private List<ObjectSlot> _minigameTableSlots = new();

    [SerializeField] private List<ObjectSlot> _initialDisplaySlots = new();


    public bool GridIsCorrect => _gridIsCorrect;


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
        foreach (ObjectSlot slot in _minigameTableSlots)
        {
            slot.ResetSlot();
        }

        foreach (ObjectSlot slot in _initialDisplaySlots)
        {
            slot.ResetSlot();
        }
    }

    public void LockGrid()
    {
        foreach (ObjectSlot slot in _minigameTableSlots)
        {
            slot.LockSlot();
        }

        foreach (ObjectSlot slot in _initialDisplaySlots)
        {
            slot.LockSlot();
        }
    }


    public bool CheckForCorrectGrid()
    {
        List<Pipe> pipes = new();

        foreach (ObjectSlot slot in _minigameTableSlots)
        {
            if (slot.Full)
            {
                pipes.Add(slot.Object.GetComponent<Pipe>());
            }
        }

        _gridIsCorrect = true;

        foreach (Pipe pipe in pipes)
        {
            if (!pipe.FullyConnected)
            {
                _gridIsCorrect = false;
            }
        }

        return _gridIsCorrect;
    }
}
