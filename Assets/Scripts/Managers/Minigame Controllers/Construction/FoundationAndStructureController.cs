using UnityEngine;
using Cinemachine;

namespace CIPA
{
    public class FoundationAndStructureController : BaseController
    {
        [SerializeField] private CinemachineVirtualCamera _secondaryCamera;

        [SerializeField] private FoundationAndStructureUI _foundationAndStructureUI;

        [SerializeField] private ParticleSystem _confetti;

        private void Update()
        {
            _secondaryCamera.gameObject.SetActive(_foundationAndStructureUI.StageIndex >= 3);
        }


        protected override void EndMinigame()
        {
            base.EndMinigame();

            _confetti.Play();
        }
    }
}
