using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSlot : MonoBehaviour
{
    [SerializeField] private Pipe _pipe;


    private void Start()
    {

    }

    private void OnDestroy()
    {
        
    }

    private void OnMouseDown()
    {
        if (_pipe != null)
        {
            _pipe.Rotate();
        }
        else
        {
            Debug.Log("Pipe is null");
        }
    }


    public void AttachPipe(Pipe pipe)
    {
        _pipe = pipe;

        _pipe.Attached = true;

        _pipe.transform.parent = transform;
        _pipe.transform.position = transform.position;
    }

}
