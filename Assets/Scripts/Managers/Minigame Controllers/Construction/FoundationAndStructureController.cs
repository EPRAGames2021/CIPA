using UnityEngine;
using Cinemachine;

namespace CIPA
{
    public class FoundationAndStructureController : BaseController
    {
        [SerializeField] private CinemachineVirtualCamera _secondaryCamera;

        [SerializeField] private FoundationAndStructureUI _foundationAndStructureUI;

        [SerializeField] private ParticleSystem _confetti;

        private bool _ended;


        private void Update()
        {
            _secondaryCamera.gameObject.SetActive(_foundationAndStructureUI.StageIndex >= 3 && !_ended);
        }


        protected override void Init()
        {
            base.Init();

            CustomGameEvents.OnMinigameStarted += ResetEnded;
            CustomGameEvents.OnMinigameWon += FireConfetti;
        }

        protected override void Finish()
        {
            base.Finish();

            CustomGameEvents.OnMinigameStarted -= ResetEnded;
            CustomGameEvents.OnMinigameWon -= FireConfetti;
        }


        protected override void EndMinigame()
        {
            base.EndMinigame();

            _ended = true;
        }

        private void ResetEnded()
        {
            _ended = false;
        }

        private void FireConfetti()
        {
            _confetti.Play();
        }
    }
}
