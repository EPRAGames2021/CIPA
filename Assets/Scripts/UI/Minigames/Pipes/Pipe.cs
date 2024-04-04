using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DraggableObject))]
public class Pipe : MonoBehaviour
{
    [SerializeField] private DraggableObject _draggableObject;

    [SerializeField] private bool _fullyConnected;

    [SerializeField] private List<PipeConnector> _pipeConnectors;

    public bool Attached => _draggableObject.Attached;
    public bool Static => _draggableObject.Static;
    public ObjectSlot Slot => _draggableObject.Slot;
    public ObjectSlot PreviousSlot => _draggableObject.PreviousSlot;

    public bool FullyConnected => _fullyConnected;


    private void OnValidate()
    {
        if (_draggableObject == null)
        {
            _draggableObject = GetComponent<DraggableObject>();
        }
    }

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
}