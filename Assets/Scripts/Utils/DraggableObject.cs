using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] private int _rotation;
    [SerializeField] private bool _attached;
    [SerializeField] private bool _static;

    [SerializeField] private ObjectSlot _slot;
    [SerializeField] private ObjectSlot _previousSlot;

    [Header("Debug")]
    [SerializeField] private float _timeUntilDrag;
    [SerializeField] private float _holdTime;
    [SerializeField] private Vector3 _touchPosition;

    public int Rotation => _rotation;
    public bool Attached { get { return _attached; } set { _attached = value; } }
    public bool Static => _static;
    public ObjectSlot Slot { get { return _slot; } set { _slot = value; } }
    public ObjectSlot PreviousSlot { get { return _previousSlot; } set { _previousSlot = value; } }


    public event System.Action OnObjectDragged;


    private void Start()
    {
        Init();
    }

    private void OnMouseDrag()
    {
        if (_static) return;

        _holdTime += Time.deltaTime;

        if (_holdTime > _timeUntilDrag)
        {
            if (Attached)
            {
                OnObjectDragged?.Invoke();
            }

            _touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(_touchPosition.x, _touchPosition.y, transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        if (_static) return;

        if (Attached && _holdTime <= _timeUntilDrag)
        {
            Rotate();
        }

        if (_holdTime > _timeUntilDrag)
        {
            if (_slot != null)
            {
                _slot.AttachObject(this);
            }
            else
            {
                _previousSlot.AttachObject(this);
            }
        }

        _holdTime = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out ObjectSlot slot);

        if (slot != null && !slot.Full)
        {
            _slot = slot;
        }
    }


    private void Init()
    {
        _holdTime = 0.0f;
    }

    public void Rotate()
    {
        _rotation = (_rotation - 90) % 360;

        transform.eulerAngles = new(transform.rotation.x, transform.rotation.y, transform.rotation.z + _rotation);
    }

    public void Attach(ObjectSlot objectSlot)
    {
        Attached = objectSlot != null;

        Slot = objectSlot;

        if (objectSlot != null) PreviousSlot = objectSlot;
    }
}
