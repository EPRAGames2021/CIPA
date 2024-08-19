using System.Collections;
using UnityEngine;

namespace CIPA
{
    public class ScoreDrainZone : MonoBehaviour
    {
        [SerializeField] private PlayerDetector _playerDetector;

        [SerializeField] private bool _drainIsActive;
        private Coroutine _drainCoroutine;


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
            _playerDetector.OnPlayerDetected += PlayerEntered;
            _playerDetector.OnPlayerLeft += PlayerLeft;

            _drainIsActive = false;
        }

        private void Finish()
        {
            _playerDetector.OnPlayerDetected -= PlayerEntered;
            _playerDetector.OnPlayerLeft -= PlayerLeft;

            StopDrain();
        }


        private void PlayerEntered(Player player)
        {
            StartDrain();
        }

        private void PlayerLeft(Player player)
        {
            StopDrain();
        }


        private void StartDrain()
        {
            _drainIsActive = true;

            _drainCoroutine = StartCoroutine(DrainCoroutine());
        }

        private void StopDrain()
        {
            _drainIsActive = false;

            if (_drainCoroutine != null)
            {
                StopCoroutine(DrainCoroutine());
            }

            _drainCoroutine = null;
        }


        private IEnumerator DrainCoroutine()
        {
            RewardAndPenaltyManager.Instance.PlayerHasWalkedOnStrictZone();

            yield return new WaitForSeconds(1);

            if (_drainIsActive)
            {
                _drainCoroutine = StartCoroutine(DrainCoroutine());
            }
            else
            {
                StopDrain();
            }
        }
    }
}
