using UnityEngine;
using Cinemachine;

namespace CIPA
{
    public class FoundationAndStructureController : BaseController
    {
        [SerializeField] private CinemachineVirtualCamera _secondaryCamera;

        [SerializeField] private FoundationAndStructureUI _foundationAndStructureUI;

        private void Update()
        {
            _secondaryCamera.gameObject.SetActive(_foundationAndStructureUI.StageIndex >= 3);
        }
    }
}
