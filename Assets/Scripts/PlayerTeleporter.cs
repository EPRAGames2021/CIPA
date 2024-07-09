using UnityEngine;

namespace CIPA
{
    public class PlayerTeleporter : MonoBehaviour
    {
        [SerializeField] private Player _player;

        [SerializeField] private Transform _destination;


        private void OnEnable()
        {
            TeleportPlayer();
        }


        private void TeleportPlayer()
        {
            _player.transform.position = _destination.position;
        }


        private void OnDrawGizmosSelected()
        {
            if (_destination == null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_destination.position, 1f);
        }
    }
}
