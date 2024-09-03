using UnityEngine;

namespace CIPA
{
    public class ConcreteFormHandler : MonoBehaviour
    {
        [SerializeField] private ConcreteForm _concreteForm;

        [SerializeField] private FoundationAndStructureUI _foundation;
        [SerializeField] private ScreenTouchController _controller;

        private bool _minigameBeaten = false;


        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (!_minigameBeaten)
            {
                _concreteForm.SetActive(_controller.DetectHolding() && _foundation.StageIndex == 3);
            }
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void Init()
        {
            CustomGameEvents.OnMinigameWon += DisableSounds;
            _foundation.OnMinigameFailed += ResetConcrete;

            _minigameBeaten = false;
        }

        private void Finish()
        {
            CustomGameEvents.OnMinigameWon -= DisableSounds;
            _foundation.OnMinigameFailed -= ResetConcrete;
        }


        private void DisableSounds()
        {
            _minigameBeaten = true;
        }

        private void ResetConcrete()
        {
            _concreteForm.Restart();
        }
    }
}
