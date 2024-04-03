using Unity.VisualScripting;
using UnityEngine;

public class PipeConnector : MonoBehaviour
{
    [SerializeField] private PipeConnector _otherConnector;

    [SerializeField] private bool _connected;

    public bool Connected => _connected;

    public event System.Action OnConnected;


    private void OnTriggerStay(Collider other)
    {
        other.TryGetComponent(out PipeConnector connector);

        if (connector != null)
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
