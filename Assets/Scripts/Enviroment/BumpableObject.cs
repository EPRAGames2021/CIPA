using UnityEngine;
using EPRA.Utilities;

[RequireComponent(typeof(Collider))]
public class BumpableObject : MonoBehaviour
{
    [SerializeField] private bool _hasBeenHit;
    [SerializeField] private bool _hitIsFatal;
    [SerializeField] private Collider _collider;

    public bool HitIsFatal => _hitIsFatal;

    public static event System.Action<BumpableObject, Player> OnHasBeenHitByPlayer;


    private void OnValidate()
    {
        if (_collider == null) _collider = GetComponent<Collider>();
    }

    private void Awake()
    {
        Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null && !_hasBeenHit)
        {
            Player player = other.GetComponent<Player>();

            OnHasBeenHitByPlayer?.Invoke(this, player);

            _hasBeenHit = true;
        }
    }


    private void Init()
    {
        _hasBeenHit = false;

        _collider.isTrigger = true;
    }

    public void Refresh()
    {
        Init();
    }
}
