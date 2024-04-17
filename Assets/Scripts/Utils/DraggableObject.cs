using UnityEngine;
using EPRA.Utilities;
using DG.Tweening;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] private int _rotation;
    [SerializeField] private bool _attached;
    [SerializeField] private bool _static;
    [SerializeField] private bool _lockedRotation;
    [SerializeField] private bool _locked;

    [SerializeField] private ObjectSlot _slot;
    [SerializeField] private ObjectSlot _previousSlot;

    [Header("Sound")]
    [SerializeField] private AudioClipCollection _attachSFX;
    [SerializeField] private AudioClipCollection _selectSFX;
    [SerializeField] private AudioClipCollection _rotateSFX;

    [Header("Debug")]
    [SerializeField] private float _timeUntilDrag;
    [SerializeField] private float _holdTime;
    [SerializeField] private Vector3 _touchPosition;
    [SerializeField] private bool _moveInZ;
    [SerializeField] private bool _rotateInY;

    public int Rotation => _rotation;
    public bool Attached { get { return _attached; } set { _attached = value; } }
    public bool Static => _static;
    public bool LockedRotation { get { return _lockedRotation; } set { _lockedRotation = value; } }
    public bool Locked { get { return _locked; } set { _locked = value; } }
    public ObjectSlot Slot { get { return _slot; } set { _slot = value; } }
    public ObjectSlot PreviousSlot { get { return _previousSlot; } set { _previousSlot = value; } }


    public event System.Action OnObjectDragged;
    public event System.Action OnObjectAttached;
    public event System.Action OnObjectRotated;


    private void Start()
    {
        Init();
    }

    private void OnMouseDrag()
    {
        if (_static || _locked) return;

        _holdTime += Time.deltaTime;

        if (_holdTime > _timeUntilDrag)
        {
            if (Attached)
            {
                OnObjectDragged?.Invoke();

                AudioManager.Instance.PlayRandomSFX(_selectSFX);

                transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), 0.25f);
            }

            _touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (!_moveInZ) transform.position = new Vector3(_touchPosition.x, _touchPosition.y, transform.position.z);
            else transform.position = new Vector3(_touchPosition.x, transform.position.y, _touchPosition.z);
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

        transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
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
        if (_lockedRotation || _locked) return;

        _rotation = (_rotation - 90) % 360;

        if (!_rotateInY) transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + _rotation), _timeUntilDrag * 0.9f);
        else transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + _rotation, transform.rotation.z), _timeUntilDrag * 0.9f);

        OnObjectRotated?.Invoke();

        Vibrator.Vibrate(100);
        AudioManager.Instance.PlayRandomSFX(_rotateSFX);
    }

    public void Attach(ObjectSlot objectSlot)
    {
        Attached = objectSlot != null;

        Slot = objectSlot;

        if (objectSlot != null) PreviousSlot = objectSlot;

        OnObjectAttached?.Invoke();

        Vibrator.Vibrate(100);
        AudioManager.Instance.PlayRandomSFX(_attachSFX);
    }
}
