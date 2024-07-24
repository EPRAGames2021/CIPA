using System.Collections;
using UnityEngine;

namespace CIPA
{
    public class RevestingController : BaseController
    {
        [SerializeField] private RevestingUI _revestingUI;

        [SerializeField] private TileGrid _tileGrid;

        [SerializeField] private Transform _playerTeleportDestination;


        protected override void StartMiniGame()
        {
            base.StartMiniGame();

            _virtualCamera.m_Lens.Orthographic = true;
            _virtualCamera.m_Lens.OrthographicSize = 25f;

            _revestingUI.OnConfirm += CheckGrid;
        }

        protected override void EndMinigame()
        {
            base.EndMinigame();

            _virtualCamera.m_Lens.OrthographicSize = 5;
            _virtualCamera.m_Lens.Orthographic = false;

            _revestingUI.OnConfirm -= CheckGrid;
        }

        private void CheckGrid()
        {
            JobAreaManager.Instance.FinishMinigame(_tileGrid.CheckForCorrectGrid());

            _player.MovementSystem.CanMove = false;
            _player.transform.SetLocalPositionAndRotation(_playerTeleportDestination.position, _playerTeleportDestination.rotation);

            _tileGrid.LockGrid();

            StartCoroutine(AnimationDelay());
        }

        private IEnumerator AnimationDelay()
        {
            yield return new WaitForSeconds(2);

            _player.LookDown();
        }
    }
}
