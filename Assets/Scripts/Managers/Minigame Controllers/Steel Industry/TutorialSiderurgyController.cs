using UnityEngine;
using EPRA.Utilities;
using System.Collections.Generic;

namespace CIPA
{
    public class TutorialSiderurgyController : MonoBehaviour
    {
        [SerializeField] private Player _player;

        [SerializeField] private GameObject _factoryMap;
        [SerializeField] private GameObject _invisibleBarrier;
        [SerializeField] private LockerTrigger _lockerTrigger;

        [SerializeField] private TutorialConstructionUI _tutorialConstructionUI;

        [SerializeField] private int _currentTutorialID;

        [SerializeField] private List<string> _keys;

        [SerializeField] private PlayerDetector _lockerDetector;
        [SerializeField] private PlayerDetector _mapDetector;


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
            _currentTutorialID = 0;
            _lockerTrigger.SetAutomatic(false);

            _factoryMap.SetActive(false);
            _invisibleBarrier.SetActive(true);

            _tutorialConstructionUI.OnAdvanceTutorial += CloseTutorial;

            LoadingScreen.OnScreenHasBeenClosed += TriggerTutorialHandler;
            CustomGameEvents.OnPlayerWorePPEs += TriggerTutorialHandler;

            _lockerDetector.OnPlayerDetected += TriggerTutorialHandler;
            _mapDetector.OnPlayerDetected += TriggerTutorialHandler;
        }

        private void Finish()
        {
            _tutorialConstructionUI.OnAdvanceTutorial -= CloseTutorial;

            LoadingScreen.OnScreenHasBeenClosed -= TriggerTutorialHandler;
            CustomGameEvents.OnPlayerWorePPEs -= TriggerTutorialHandler;

            _lockerDetector.OnPlayerDetected -= TriggerTutorialHandler;
            _mapDetector.OnPlayerDetected -= TriggerTutorialHandler;
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

            if (_currentTutorialID != 1 && _currentTutorialID < 5)
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
                _invisibleBarrier.SetActive(false);
            }
            else if (_currentTutorialID == 3)
            {
                TriggerTutorial(4);
            }
            else if (_currentTutorialID == 4)
            {
                _factoryMap.SetActive(true);

                TriggerTutorial(4);
            }
            else if (_currentTutorialID == 5)
            {
                _factoryMap.SetActive(false);

                JobAreaManager.Instance.FinishTutorial();
            }

            _currentTutorialID++;
        }
    }
}
