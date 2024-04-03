using Unity.VisualScripting;
using UnityEngine;

public class PipeConnector : MonoBehaviour
{
    [SerializeField] private ConnectionColor _connectionColor;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material[] _materials;

    [SerializeField] private PipeConnector _otherConnector;

    [SerializeField] private bool _connected;

    public ConnectionColor ConnectionColor => _connectionColor;
    public bool Connected => _connected;
    public bool FreeConnection => _connectionColor == ConnectionColor.None;


    public event System.Action OnConnected;


    private void OnValidate()
    {
        if (_connectionColor == ConnectionColor.None)
        {
            _meshRenderer.enabled = false;

            _meshRenderer.material = null;
        }
        else
        {
            _meshRenderer.enabled = true;

            _meshRenderer.material = _materials[(int)_connectionColor - 1];
        }
    }


    private void OnTriggerStay(Collider other)
    {
        other.TryGetComponent(out PipeConnector connector);

        if (connector != null && _connectionColor == connector.ConnectionColor)
        {
            _otherConnector = connector;

            UpdateConnection();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out PipeConnector connector);

        if (_otherConnector == connector)
        {
            _otherConnector = null;

            UpdateConnection();
        }
    }


    private void OnDrawGizmos()
    {
        Vector3 scale = transform.localScale;

        if (_connected) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, scale);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 scale = transform.localScale;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, scale);
    }


    private void UpdateConnection()
    {
        if (_otherConnector != null)
        {
            if (!_connected)
            {
                _connected = true;

                OnConnected?.Invoke();
            }
        }
        else
        {
            if (_connected)
            {
                _connected = false;

                OnConnected?.Invoke();
            }
        }
    }
}

public enum ConnectionColor
{
    None = 0,
    Red = 1,
    Green = 2,
    Blue = 3,
    Yellow = 4,
}
