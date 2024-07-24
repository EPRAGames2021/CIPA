using UnityEngine;
using EPRA.Utilities;
using DG.Tweening;

public class DraggableObject : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private bool _attached;
    [SerializeField] private bool _lockedMovement;

    [Header("Rotation")]
    [SerializeField] private int _rotationIndex;
    [SerializeField] private int[] _rotations;
    [SerializeField] private bool _lockedRotation;

    [Header("Other")]
    [SerializeField] private bool _static;

    [SerializeField] private ObjectSlot _slot;
    [SerializeField] private ObjectSlot _previousSlot;

    [Header("Sound")]
    [SerializeField] private AudioClipCollection _attachSFX;
    [SerializeField] private AudioClipCollection _selectSFX;
    [SerializeField] private AudioClipCollection _rotateSFX;

    [Header("Debug")]
    private Vector3 _initialScale;
    [SerializeField] private float _timeUntilDrag;
    [SerializeField] private float _holdTime;
    [SerializeField] private Vector3 _touchPosition;
    [SerializeField] private bool _moveInZ;
    [SerializeField] private bool _rotateInY;


    public int RotationIndex => _rotationIndex;
    public bool Attached { get { return _attached; } set { _attached = value; } }
    public bool Locked { set { _lockedMovement = value; _lockedRotation = value; } }
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
        if (_static || _lockedMovement) return;

        _holdTime += Time.deltaTime;

        if (_holdTime > _timeUntilDrag)
        {
            if (Attached)
            {
                OnObjectDragged?.Invoke();

                AudioManager.Instance.PlayRandomSFX(_selectSFX);

                transform.DOScale(new Vector3(_initialScale.x * 0.75f, _initialScale.y * 0.75f, _initialScale.z * 0.75f), 0.25f);
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

        transform.DOScale(new Vector3(_initialScale.x * 1f, _initialScale.y * 1f, _initialScale.z * 1f), 0.25f);
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
        _initialScale = transform.localScale;

        _holdTime = 0.0f;
    }

    private void Rotate()
    {
        if (_lockedRotation || _static) return;

        _rotationIndex--;
        
        if ( _rotationIndex < 0)
        {
            _rotationIndex = _rotations.Length - 1;
        }

        if (!_rotateInY)
        {
            transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + _rotations[_rotationIndex]), _timeUntilDrag * 0.9f);
        }
        else
        {
            transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + _rotations[_rotationIndex], transform.rotation.z), _timeUntilDrag * 0.9f);
        }

        OnObjectRotated?.Invoke();

        Vibrator.Vibrate(100);
        AudioManager.Instance.PlayRandomSFX(_rotateSFX);
    }

    public void Attach(ObjectSlot objectSlot)
    {
        Attach(objectSlot, true);
    }

    public void Attach(ObjectSlot objectSlot, bool provideFeedback)
    {
        Attached = objectSlot != null;

        Slot = objectSlot;

        if (objectSlot != null) PreviousSlot = objectSlot;

        OnObjectAttached?.Invoke();

        if (Attached && provideFeedback)
        {
            Vibrator.Vibrate(100);
            AudioManager.Instance.PlayRandomSFX(_attachSFX);
        }
    }
}
