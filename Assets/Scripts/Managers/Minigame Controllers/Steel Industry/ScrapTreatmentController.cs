using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EPRA.Utilities;
using System.Collections;

namespace CIPA
{
    public class ScrapTreatmentController : MonoBehaviour
    {
        [Header("GD area")]
        [SerializeField] private int _requiredScrapsToWin;
        [SerializeField] private int _requiredScrapsToLose;

        [SerializeField] private int _scrapsDisposedCorrectly;
        [SerializeField] private int _scrapsDisposedIncorrectly;

        [Header("Dev area")]
        [SerializeField] private GameObject _minigameUI;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        [SerializeField] private List<Treadmill> _treadmills;

        [SerializeField] private Player _player;

        public List<Treadmill> Treadmills => _treadmills;

        public int RequiredToWin => _requiredScrapsToWin;
        public int RequiredToLose => _requiredScrapsToLose;
        public int Collected => _scrapsDisposedCorrectly;
        public int Lost => _scrapsDisposedIncorrectly;


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
            _minigameUI.SetActive(false);
            _virtualCamera.gameObject.SetActive(false);

            for (int i = 0; i < _treadmills.Count; i++)
            {
                _treadmills[i].Active = false;
            }

            CanvasManager.Instance.EnableHUD(true);
            CanvasManager.Instance.EnableVirtualJoystick(true);

            CustomGameEvents.OnMinigameStarted += StartMiniGame;
            CustomGameEvents.OnMinigameEnded += EndMiniGame;
        }

        private void Finish()
        {
            for (int i = 0; i < _treadmills.Count; i++)
            {
                _treadmills[i].OnScrapSpawned -= SubToNewScrap;
            }

            CustomGameEvents.OnMinigameStarted -= StartMiniGame;
            CustomGameEvents.OnMinigameEnded -= EndMiniGame;
        }

        private void StartMiniGame()
        {
            //_minigameUI.SetActive(true);
            StartCoroutine(OpenMenuDelay());
            IEnumerator OpenMenuDelay()
            {
                yield return new WaitForSeconds(0.5f);
                _minigameUI.SetActive(true);
            }

            _virtualCamera.gameObject.SetActive(true);

            _scrapsDisposedCorrectly = 0;
            _scrapsDisposedIncorrectly = 0;

            for (int i = 0; i < _treadmills.Count; i++)
            {
                _treadmills[i].Refresh();

                _treadmills[i].Active = true;

                _treadmills[i].OnScrapSpawned += SubToNewScrap;
            }

            _player.ArrowSystem.SetEnabled(false);

            CanvasManager.Instance.EnableHUD(false);
            CanvasManager.Instance.EnableVirtualJoystick(false);
        }

        private void EndMiniGame()
        {
            _minigameUI.SetActive(false);

            for (int i = 0; i < _treadmills.Count; i++)
            {
                _treadmills[i].Active = false;

                _treadmills[i].OnScrapSpawned -= SubToNewScrap;
            }
        }


        private void SubToNewScrap(Scrap newScrap)
        {
            newScrap.OnCollected += AddScore;
        }

        private void AddScore(Scrap scrap)
        {
            if (scrap.MarkedToBeDestroyed)
            {
                return;
            }

            if (scrap.ProperlyCollected)
            {
                _scrapsDisposedCorrectly++;
            }
            else
            {
                _scrapsDisposedIncorrectly++;
            }

            CheckScores();
        }

        private void CheckScores()
        {
            if (_scrapsDisposedCorrectly >= _requiredScrapsToWin)
            {
                JobAreaManager.Instance.FinishMinigame(true);
            }
            else if (_scrapsDisposedIncorrectly >= _requiredScrapsToLose)
            {
                JobAreaManager.Instance.FinishMinigame(false);
            }
        }

        public bool SwitchTreadmillOn(int index)
        {
            _treadmills[index].Active = !_treadmills[index].Active;

            return _treadmills[index].Active;
        }
    }
}
