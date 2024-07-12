using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EPRA.Utilities;
using System.Collections;

namespace CIPA
{
    public class BaseController : MonoBehaviour
    {
        [SerializeField] private Player _player;

        [SerializeField] private GameObject _minigameUI;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        [Tooltip("Optional. For objects that only show up when mini game starts")]
        [SerializeField] private List<GameObject> _minigameEnviromentObjects;


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
            _player = JobAreaManager.Instance.Player;

            _minigameUI.SetActive(false);
            _virtualCamera.gameObject.SetActive(false);

            for (int i = 0; i < _minigameEnviromentObjects.Count; i++)
            {
                _minigameEnviromentObjects[i].SetActive(false);
            }

            CanvasManager.Instance.EnableVirtualJoystick(true);
            CanvasManager.Instance.EnableHUD(true);

            CustomGameEvents.OnMinigameStarted += StartMiniGame;
            CustomGameEvents.OnMinigameEnded += EndMinigame;
        }

        private void Finish()
        {
            CustomGameEvents.OnMinigameStarted -= StartMiniGame;
            CustomGameEvents.OnMinigameEnded -= EndMinigame;
        }

        protected virtual void StartMiniGame()
        {
            _player.ArrowSystem.SetEnabled(false);

            StartCoroutine(OpenMenuDelay());
            IEnumerator OpenMenuDelay()
            {
                yield return new WaitForSeconds(0.5f);
                _minigameUI.SetActive(true);
            }

            _virtualCamera.gameObject.SetActive(true);
            _virtualCamera.Priority = 11;

            for (int i = 0; i < _minigameEnviromentObjects.Count; i++)
            {
                _minigameEnviromentObjects[i].SetActive(true);
            }

            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);
        }

        protected virtual void EndMinigame()
        {
            _minigameUI.SetActive(false);
            _virtualCamera.gameObject.SetActive(false);

            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);
        }
    }
}
