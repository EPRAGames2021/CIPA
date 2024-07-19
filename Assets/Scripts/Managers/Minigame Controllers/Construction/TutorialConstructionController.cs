using System.Collections.Generic;
using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    public class TutorialConstructionController : MonoBehaviour
    {
        [SerializeField] private Player _player;

        [SerializeField] private GameObject _invisbleBarrier;
        [SerializeField] private LockerTrigger _lockerTrigger;

        [SerializeField] private TutorialConstructionUI _tutorialConstructionUI;

        [SerializeField] private int _currentTutorialID;

        [SerializeField] private List<string> _keys;

        [SerializeField] private PlayerDetector _lockerDetector;
        [SerializeField] private PlayerDetector _conesDetector;
        [SerializeField] private PlayerDetector _colleaguesDetector;
        [SerializeField] private PlayerDetector _vehiclesDetector;
        [SerializeField] private PlayerDetector _spotDetector;
        [SerializeField] private PlayerDetector _finishDetector;


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
            _invisbleBarrier.SetActive(true);
            _currentTutorialID = 0;

            LoadingScreen.OnScreenHasBeenClosed += TriggerTutorialHandler;
            _lockerDetector.OnPlayerDetected += TriggerTutorialHandler;
            CustomGameEvents.OnPlayerWorePPEs += TriggerTutorialHandler;
            _conesDetector.OnPlayerDetected += TriggerTutorialHandler;
            _colleaguesDetector.OnPlayerDetected += TriggerTutorialHandler;
            _vehiclesDetector.OnPlayerDetected += TriggerTutorialHandler;
            _spotDetector.OnPlayerDetected += TriggerTutorialHandler;
            _finishDetector.OnPlayerDetected += TriggerTutorialHandler;
        }

        private void Finish()
        {
            LoadingScreen.OnScreenHasBeenClosed -= TriggerTutorialHandler;
            _lockerDetector.OnPlayerDetected -= TriggerTutorialHandler;
            CustomGameEvents.OnPlayerWorePPEs -= TriggerTutorialHandler;
            _conesDetector.OnPlayerDetected -= TriggerTutorialHandler;
            _colleaguesDetector.OnPlayerDetected -= TriggerTutorialHandler;
            _vehiclesDetector.OnPlayerDetected -= TriggerTutorialHandler;
            _spotDetector.OnPlayerDetected -= TriggerTutorialHandler;
            _finishDetector.OnPlayerDetected -= TriggerTutorialHandler;
        }


        private void TriggerTutorialHandler()
        {
            TriggerTutorial(_currentTutorialID);
        }

        private void TriggerTutorialHandler(Player player)
        {
            TriggerTutorial(_currentTutorialID);
        }


        private void TriggerTutorial(int index)
        {
            _tutorialConstructionUI.gameObject.SetActive(true);

            CanvasManager.Instance.EnableVirtualJoystick(false);
            CanvasManager.Instance.EnableHUD(false);

            _tutorialConstructionUI.ShowDescription(_keys[index]);    
            
            _tutorialConstructionUI.ShowHand(_currentTutorialID == 0);
        }

        public void CloseTutorial()
        {
            _tutorialConstructionUI.gameObject.SetActive(false);

            if (_currentTutorialID != 1 && _currentTutorialID < 7)
            {
                CanvasManager.Instance.EnableVirtualJoystick(true);
                CanvasManager.Instance.EnableHUD(true);
            }

            if (_currentTutorialID == 0)
            {
                _tutorialConstructionUI.ShowHand(false);
            }
            else if (_currentTutorialID == 1)
            {
                _lockerTrigger.HandlePlayerDetection(_player);
            }
            else if (_currentTutorialID == 2)
            {
                _invisbleBarrier.SetActive(false);
            }
            else if (_currentTutorialID == 7)
            {
                TriggerTutorial(8);
            }
            else if (_currentTutorialID == 8)
            {
                TriggerTutorial(9);
            }
            else if (_currentTutorialID == 9)
            {
                TriggerTutorial(10);
            }
            else if (_currentTutorialID == 10)
            {
                JobAreaManager.Instance.FinishTutorial();
            }

            _currentTutorialID++;
        }
    }
}
