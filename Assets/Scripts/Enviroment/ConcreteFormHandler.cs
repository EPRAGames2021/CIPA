using UnityEngine;

namespace CIPA
{
    public class ConcreteFormHandler : MonoBehaviour
    {
        [SerializeField] private ConcreteForm _concreteForm;

        [SerializeField] private FoundationAndStructureUI _foundation;
        [SerializeField] private ScreenTouchController _controller;


        private void Start()
        {
            Init();
        }

        private void Update()
        {
            _concreteForm.SetActive(_controller.DetectHolding() && _foundation.StageIndex == 3);
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void Init()
        {
            _foundation.OnMinigameFailed += ResetConcrete;
        }

        private void Finish()
        {
            _foundation.OnMinigameFailed -= ResetConcrete;
        }


        private void ResetConcrete()
        {
            _concreteForm.Restart();
        }
    }
}
