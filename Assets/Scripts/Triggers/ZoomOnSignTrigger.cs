using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class ZoomOnSignTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerDetector _playerDetector;

        [SerializeField] private GameObject _indicator;

        [SerializeField] private float _timeUntilZoomIn;
        private Coroutine _zoomInCoroutine;


        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            Finish();
        }


        private void Init()
        {
            _playerDetector.OnPlayerDetected += PlayerEnteredArea;
            _playerDetector.OnPlayerLeft += PlayerExitedArea;

            _zoomInCoroutine = null;
        }

        private void Finish()
        {
            _playerDetector.OnPlayerDetected -= PlayerEnteredArea;
            _playerDetector.OnPlayerLeft -= PlayerExitedArea;
        }


        private void PlayerEnteredArea(Player player)
        {
            _indicator.SetActive(true);

            _zoomInCoroutine = StartCoroutine(ZoomInCoroutine(player, _timeUntilZoomIn));
        }

        private IEnumerator ZoomInCoroutine(Player player, float time)
        {
            yield return new WaitForSeconds(time);

            ZoomIn(player);
        }

        private void PlayerExitedArea(Player player)
        {
            _indicator.SetActive(false);

            ZoomOut(player);

            StopAllCoroutines();
            _zoomInCoroutine = null;
        }


        private void ZoomIn(Player player)
        {
            player.MovementSystem.RestrictMovement(true);
            PlayerCameraHandler.Instance.ZoomOnSign(true);
        }

        private void ZoomOut(Player player)
        {
            player.MovementSystem.RestrictMovement(false);
            PlayerCameraHandler.Instance.ZoomOnSign(false);
        }
    }
}
