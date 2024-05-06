using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] private EquipmentSystem _equipmentSystem;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Finish();
    }


    private void Init()
    {
        _equipmentSystem.OnEquipped += AnimateCamera;
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
        CinemachineFramingTransposer framingTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        framingTransposer.m_CameraDistance = distance;
    }

    private IEnumerator ReturnZoomLevel(float time)
    {
        yield return new WaitForSeconds(time);

        SetCameraDistance(15);
    }


}
