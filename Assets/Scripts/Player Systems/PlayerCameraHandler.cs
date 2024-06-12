using System.Collections;
using UnityEngine;
using Cinemachine;

namespace CIPA
{
    public class PlayerCameraHandler : MonoBehaviour
    {
        public static PlayerCameraHandler Instance;

        [SerializeField] private EquipmentSystem _equipmentSystem;
        [SerializeField] private CinemachineVirtualCamera _playerVirtualCamera;

        [SerializeField] private CinemachineVirtualCamera _PPEVirtualCamera;


        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void InitSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Init()
        {
            _equipmentSystem.OnEquipped += AnimateCamera;

            FocusOnPPEBoard(false);
        }

        private void Finish()
        {
            _equipmentSystem.OnEquipped -= AnimateCamera;
        }


        private void AnimateCamera(bool animate)
        {
            if (animate)
            {
                SetCameraDistance(8);

                StartCoroutine(ReturnZoomLevel(3.5f));
            }
        }


        private void SetCameraDistance(float distance)
        {
            CinemachineFramingTransposer framingTransposer = _playerVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            framingTransposer.m_CameraDistance = distance;
        }

        private IEnumerator ReturnZoomLevel(float time)
        {
            yield return new WaitForSeconds(time);

            SetCameraDistance(15);
        }

        public void FocusOnPPEBoard(bool focusOnPPEBoard)
        {
            _PPEVirtualCamera.gameObject.SetActive(focusOnPPEBoard);
        }
    }
}
