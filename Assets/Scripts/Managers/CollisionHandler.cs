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

            AnimationHandler animationHandler = player.GetComponentInChildren<AnimationHandler>();

            if (animationHandler != null)
            {
                animationHandler.CallCollisionVFX();
            }

            bumpableObject.TryGetComponent(out NPC npc);

            if (npc != null && !JobAreaManager.Instance.ArrivedAtMinigameLocation)
            {
                npc.UpdateState(CharacterState.Dying);
            }
        }
    }
}
