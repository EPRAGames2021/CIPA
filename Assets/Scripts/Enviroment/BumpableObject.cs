using UnityEngine;
using EPRA.Utilities;

[RequireComponent(typeof(BoxCollider))]
public class BumpableObject : MonoBehaviour
{
    [SerializeField] private bool _hasBeenHit;
    [SerializeField] private bool _hitIsFatal;
    [SerializeField] private BoxCollider _boxColliderTrigger;

    [SerializeField] private CurrencySO _dayScore;
    [SerializeField] private int _bumpPenalty;


    private void Awake()
    {
        Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null && !_hasBeenHit)
        {
            Player player = other.GetComponent<Player>();

            if (!_hitIsFatal)
            {
                _dayScore.RemoveFromCurrency(_bumpPenalty);
            }
            else
            {
                player.HealthSystem.TakeDamage(int.MaxValue);
            }

            _hasBeenHit = true;
        }
    }


    private void Init()
    {
        _hasBeenHit = false;

        _boxColliderTrigger.isTrigger = true;
    }

    public void Refresh()
    {
        Init();
    }
}
