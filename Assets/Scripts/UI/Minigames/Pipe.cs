using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Pipe : MonoBehaviour
{
    [SerializeField] private PipeType _type;

    [SerializeField] private int _rotation;
    [SerializeField] private bool _attached;
    [SerializeField] private PipeSlot _slot;
    [SerializeField] private PipeSlot _previousSlot;

    [Header("Debug")]
    [SerializeField] private bool _clicked;
    [SerializeField] private float _holdTime;
    [SerializeField] private Vector3 _touchPosition;

    public bool Attached { get { return _attached; } set { _attached = value; } }
    public PipeSlot Slot { get { return _slot; } set { _slot = value; } }
    public PipeSlot PreviousSlot { get { return _previousSlot; } set { _previousSlot = value; } }


    public event System.Action OnPipeDragged;
    public event System.Action<Pipe> OnPipeDropped;


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
        _clicked = true;
    }

    private void OnMouseDrag()
    {
        _holdTime += Time.deltaTime;

        if (_holdTime > 0.2f)
        {
            if (Attached)
            {
                OnPipeDragged?.Invoke();
            }

            _touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(_touchPosition.x, _touchPosition.y, transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        if (Attached && _holdTime <= 0.2f)
        {
            Rotate();
        }

        if (_holdTime > 0.2f)
        {
            if (_slot != null)
            {
                _slot.AttachPipe(this);
            }
            else
            {
                _previousSlot.AttachPipe(this);
            }
        }

        _clicked = false;
        _holdTime = 0.0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent(out PipeSlot slot);

        if (slot != null && !slot.Full)
        {
            _slot = slot;
        }
    }


    private void Init()
    {
        _clicked = false;
        _holdTime = 0.0f;
    }

    private void Finish()
    {

    }


    public void Rotate()
    {
        _rotation = (_rotation - 90) % 360;

        transform.eulerAngles = new(transform.rotation.x, transform.rotation.y, transform.rotation.z + _rotation);
    }
}

public enum PipeType
{
    Starting = 0,
    Ending = 1,
    Straight = 2,
    LShapedLeft = 3,
    LShapedRight = 4,
}