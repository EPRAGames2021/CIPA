using UnityEngine;

public class PipeConnector : MonoBehaviour
{
    [SerializeField] private bool _connected;
    [SerializeField] private bool _connectedInternal;

    public bool Connected => _connected;

    public event System.Action OnConnected;


    private void OnTriggerStay(Collider other)
    {
        other.TryGetComponent(out PipeConnector connector);

        _connectedInternal = connector != null;

        if (connector != null)
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


    private void OnDrawGizmos()
    {
        Vector3 scale = transform.localScale;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, scale);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 scale = transform.localScale;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, scale);
    }
}
