using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pipe : MonoBehaviour
{
    [SerializeField] private int _rotation;
    [SerializeField] private bool _attached;
    [SerializeField] private bool _fullyConnected;
    [SerializeField] private bool _static;
    [SerializeField] private PipeSlot _slot;
    [SerializeField] private PipeSlot _previousSlot;
    [SerializeField] private List<PipeConnector> _pipeConnectors;

    [Header("Debug")]
    [SerializeField] private float _timeUntilDrag;
    [SerializeField] private float _holdTime;
    [SerializeField] private Vector3 _touchPosition;

    public bool Attached { get { return _attached; } set { _attached = value; } }
    public bool FullyConnected => _fullyConnected;
    public bool Static => _static;
    public PipeSlot Slot { get { return _slot; } set { _slot = value; } }
    public PipeSlot PreviousSlot { get { return _previousSlot; } set { _previousSlot = value; } }


    public event System.Action OnPipeDragged;


    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Finish();
    }

    private void OnMouseDrag()
    {
        if (_static) return;

        _holdTime += Time.deltaTime;

        if (_holdTime > _timeUntilDrag)
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
        if (_static) return;

        if (Attached && _holdTime <= _timeUntilDrag)
        {
            Rotate();
        }

        if (_holdTime > _timeUntilDrag)
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

        _holdTime = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out PipeSlot slot);

        if (slot != null && !slot.Full)
        {
            _slot = slot;
        }
    }


    private void Init()
    {
        _holdTime = 0.0f;

        for (int i = 0; i < _pipeConnectors.Count; i++)
        {
            _pipeConnectors[i].OnConnected += CheckConnection;
        }
    }

    private void Finish()
    {
        for (int i = 0; i < _pipeConnectors.Count; i++)
        {
            _pipeConnectors[i].OnConnected -= CheckConnection;
        }
    }

    private void CheckConnection()
    {
        for (int i = 0; i < _pipeConnectors.Count; i++)
        {
            if (!_pipeConnectors[i].Connected)
            {
                _fullyConnected = false;

                return;
            }
        }

        _fullyConnected = true;
    }


    public void Rotate()
    {
        _rotation = (_rotation - 90) % 360;

        transform.eulerAngles = new(transform.rotation.x, transform.rotation.y, transform.rotation.z + _rotation);

        //CheckConnection();
    }

    public void Attach(PipeSlot pipeSlot)
    {
        Attached = pipeSlot != null;

        Slot = pipeSlot;

        if (pipeSlot != null) PreviousSlot = pipeSlot;

        //CheckConnection();
    }

}