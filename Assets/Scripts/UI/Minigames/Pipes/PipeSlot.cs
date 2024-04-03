using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSlot : MonoBehaviour
{
    [SerializeField] private Pipe _startingPipe;

    [SerializeField] private Pipe _pipe;

    public bool Full => _pipe != null;
    public Pipe Pipe => _pipe;

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
        Pipe pipe = GetComponentInChildren<Pipe>();

        if (pipe != null)
        {
            _pipe = pipe;

            _startingPipe = pipe;

            AttachPipe(_pipe);
        }
    }

    private void Finish()
    {
        if (_pipe != null) _pipe.OnPipeDragged -= DeattachPipe;
    }


    public void AttachPipe(Pipe pipe)
    {
        _pipe = pipe;

        _pipe.Attach(this);

        _pipe.transform.SetParent(transform);
        _pipe.transform.position = new(transform.position.x, transform.position.y, transform.position.z - 0.1f);

        _pipe.OnPipeDragged += DeattachPipe;
    }

    public void DeattachPipe()
    {
        if (_pipe == null) return;

        _pipe.OnPipeDragged -= DeattachPipe;

        _pipe.Attach(null);

        _pipe = null;
    }

    public void ResetSlot()
    {
        if (_startingPipe != null)
        {
            AttachPipe(_startingPipe);
        }
    }
}
