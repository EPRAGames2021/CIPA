using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGrid : MonoBehaviour
{
    [SerializeField] private bool _gridIsCorrect;

    [SerializeField] private List<PipeSlot> _minigameTableSlots = new();

    [SerializeField] private List<PipeSlot> _initialDisplaySlots = new();


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
        foreach (PipeSlot slot in _initialDisplaySlots)
        {
            slot.ResetSlot();
        }
    }


    public bool CheckForCorrectGrid()
    {
        List<Pipe> pipes = new();

        foreach (PipeSlot slot in _minigameTableSlots)
        {
            if (slot.Full)
            {
                pipes.Add(slot.Pipe);
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
