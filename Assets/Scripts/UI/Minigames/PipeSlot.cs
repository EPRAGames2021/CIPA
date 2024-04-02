using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSlot : MonoBehaviour
{
    [SerializeField] private Pipe _pipe;

    public bool Full => _pipe != null;

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Finish();
    }

    private void OnMouseDown()
    {
        //if (_pipe != null)
        //{
        //    _pipe.Rotate();
        //}
        //else
        //{
        //    Debug.Log("Pipe is null");
        //}
    }


    private void Init()
    {
        if (_pipe != null) AttachPipe(_pipe);
    }

    private void Finish()
    {
        if (_pipe != null) _pipe.OnPipeDragged -= DeattachPipe;
    }


    public void AttachPipe(Pipe pipe)
    {
        _pipe = pipe;

        _pipe.Attached = true;
        _pipe.Slot = this;
        _pipe.PreviousSlot = this;

        _pipe.transform.SetParent(transform);
        _pipe.transform.position = new(transform.position.x, transform.position.y, transform.position.z - 0.1f);

        _pipe.OnPipeDragged += DeattachPipe;
    }

    public void DeattachPipe()
    {
        _pipe.OnPipeDragged -= DeattachPipe;

        _pipe.Attached = false;
        _pipe.Slot = null;

        _pipe = null;
    }

}
