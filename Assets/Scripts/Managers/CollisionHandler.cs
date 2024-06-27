using UnityEngine;

namespace CIPA
{
    public class CollisionHandler : MonoBehaviour
    {
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
            BumpableObject.OnHasBeenHitByPlayer += HandleCollision;
        }

        private void Finish()
        {
            BumpableObject.OnHasBeenHitByPlayer -= HandleCollision;
        }


        private void HandleCollision(BumpableObject bumpableObject, Player player)
        {
            if (bumpableObject.HitIsFatal)
            {
                player.HealthSystem.TakeDamage(int.MaxValue);
            }
            else
            {
                RewardAndPenaltyManager.Instance.PlayerHasBumpedIntoObject();
            }
        }
    }
}
