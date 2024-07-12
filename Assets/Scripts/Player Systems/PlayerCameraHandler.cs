using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace CIPA
{
    public class PlayerCameraHandler : MonoBehaviour
    {
        public static PlayerCameraHandler Instance;

        [SerializeField] private EquipmentSystem _equipmentSystem;

        [SerializeField] private CinemachineVirtualCamera _playerVirtualCamera;

        [SerializeField] private List<CinemachineVirtualCamera> _PPEvirtualCameras;


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
            CustomGameEvents.OnPlayerArrivedAtMinigameLocation += DisablePlayerCamera;

            _equipmentSystem.OnEquipped += AnimateCamera;

            FocusOnPPEBoard(false);
        }

        private void Finish()
        {
            CustomGameEvents.OnPlayerArrivedAtMinigameLocation -= DisablePlayerCamera;

            _equipmentSystem.OnEquipped -= AnimateCamera;
        }


        private void DisablePlayerCamera()
        {
            _playerVirtualCamera.Priority = 9;
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
            for (int i = 0; i < _PPEvirtualCameras.Count; i++)
            {
                _PPEvirtualCameras[i].Priority = focusOnPPEBoard ? 11 : -1;
            }
        }

        public void ZoomOnSign(bool zoomInOnSign)
        {
            _playerVirtualCamera.m_Lens.FieldOfView = zoomInOnSign ? 45 : 60;
        }
    }
}
