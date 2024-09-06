using UnityEngine;
using EPRA.Utilities;
using System.Collections.Generic;

namespace CIPA
{
    public class TutorialSiderurgyController : MonoBehaviour
    {
        [SerializeField] private Player _player;

        [SerializeField] private LockerTrigger _lockerTrigger;

        [SerializeField] private TutorialConstructionUI _tutorialConstructionUI;

        [SerializeField] private int _currentTutorialID;

        [SerializeField] private List<string> _keys;

        [SerializeField] private PlayerDetector _lockerDetector;
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
            _currentTutorialID = 0;

            LoadingScreen.OnScreenHasBeenClosed += TriggerTutorialHandler;
            CustomGameEvents.OnPlayerWorePPEs += TriggerTutorialHandler;

            _lockerDetector.OnPlayerDetected += TriggerTutorialHandler;
            _finishDetector.OnPlayerDetected += TriggerTutorialHandler;
        }

        private void Finish()
        {
            LoadingScreen.OnScreenHasBeenClosed += TriggerTutorialHandler;
            CustomGameEvents.OnPlayerWorePPEs += TriggerTutorialHandler;

            _lockerDetector.OnPlayerDetected += TriggerTutorialHandler;
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
                JobAreaManager.Instance.FinishTutorial();
            }

            _currentTutorialID++;
        }
    }
}
