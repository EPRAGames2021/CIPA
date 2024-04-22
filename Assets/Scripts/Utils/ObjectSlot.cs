using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSlot : MonoBehaviour
{
    [SerializeField] private DraggableObject _startingObject;

    [SerializeField] private DraggableObject _draggableObject;

    public bool Full => _draggableObject != null;
    public DraggableObject Object => _draggableObject;

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Finish();
    }


    protected virtual void Init()
    {
        DraggableObject draggableObject = GetComponentInChildren<DraggableObject>();

        if (draggableObject != null)
        {
            _draggableObject = draggableObject;

            _startingObject = draggableObject;

            AttachObject(_draggableObject, false);
        }
    }

    protected virtual void Finish()
    {
        if (_draggableObject != null) _draggableObject.OnObjectDragged -= DetachObject;
    }


    public virtual void AttachObject(DraggableObject draggableObject)
    {
        AttachObject(draggableObject, true);
    }

    public virtual void AttachObject(DraggableObject draggableObject, bool provideFeedback)
    {
        _draggableObject = draggableObject;

        _draggableObject.Attach(this, provideFeedback);

        _draggableObject.transform.SetParent(transform);
        _draggableObject.transform.position = new(transform.position.x, transform.position.y, transform.position.z);

        _draggableObject.OnObjectDragged += DetachObject;
    }

    public virtual void DetachObject()
    {
        if (_draggableObject == null) return;

        _draggableObject.OnObjectDragged -= DetachObject;

        _draggableObject.Attach(null);

        _draggableObject = null;
    }

    public virtual void ResetSlot()
    {
        DetachObject();

        if (_startingObject != null)
        {
            AttachObject(_startingObject, false);
        }

        if (Full) _draggableObject.Locked = false;
    }

    public virtual void LockSlot()
    {
        if (Full) _draggableObject.Locked = true;
    }
}
