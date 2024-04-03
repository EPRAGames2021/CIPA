using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGrid : MonoBehaviour
{
    [SerializeField] private bool _gridIsCorrect;

    [SerializeField] private List<PipeSlot> _slots = new();


    public bool GridIsCorrect => _gridIsCorrect;


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
        _gridIsCorrect = false;
    }

    private void Finish()
    {

    }
}
