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


        private void HandleCollision(BumpableObject bumpableObject)
        {
            if (bumpableObject.HitIsFatal)
            {
                JobAreaManager.Instance.FinishMinigame(false);
            }
            else
            {
                RewardAndPenaltyManager.Instance.PlayerHasBumpedIntoObject();
            }
        }
    }
}
