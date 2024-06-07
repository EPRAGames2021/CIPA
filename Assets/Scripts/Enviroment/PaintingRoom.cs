using UnityEngine;
using DG.Tweening;
using EPRA.Utilities;

namespace CIPA
{
    public class PaintingRoom : MonoBehaviour
    {
        [SerializeField] private ScreenTouchController _screenTouchController;
        [SerializeField] private MousePositionHandler _mousePositionHandler;
        [SerializeField] private FinishingUI _finishingUI;

        [SerializeField] private MeshRenderer _paintRenderer;
        [SerializeField] private GameObject _inkRoll;
        private float _inkRollX;
        private float _inkRollZ;
        [SerializeField] private float _inkRollMinHeight;
        [SerializeField] private float _inkRollMaxHeight;

        [SerializeField] private bool _painting;
        [SerializeField] private float _idleTime;
        [SerializeField] private float _idleTimeTolarance;

        [Header("Sound")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClipCollection _paintingSFX;


        private void OnEnable()
        {
            _inkRollX = _inkRoll.transform.position.x;
            _inkRollZ = _inkRoll.transform.position.z;

            Color color = new(_paintRenderer.material.color.r, _paintRenderer.material.color.g, _paintRenderer.material.color.b, 0.0f);

            _paintRenderer.material.color = color;
        }

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            DetermineTouch();

            UpdateColor();
            MoveInkRoll();
            PlaySound();
        }


        private void Init()
        {
            _painting = false;
        }


        private void DetermineTouch()
        {
            if (_screenTouchController.DetectHolding())
            {
                _painting = true;

                _idleTime = 0f;
            }
            else
            {
                _idleTime += Time.deltaTime;
            }

            if (_idleTime >= _idleTimeTolarance)
            {
                _painting = false;
            }
        }

        private void UpdateColor()
        {
            if (_finishingUI.PaintFinished) return;

            float alpha = Remap.RemapValue(_finishingUI.CompletionPercentage, 0, 100, 0, 1);

            Color newColor = new(_paintRenderer.material.color.r, _paintRenderer.material.color.g, _paintRenderer.material.color.b, alpha);

            _paintRenderer.material.color = newColor;
        }

        private void MoveInkRoll()
        {
            if (_finishingUI.PaintFinished) return;

            float height = Remap.RemapValue(_mousePositionHandler.HeightPercent, 0, 100, _inkRollMinHeight, _inkRollMaxHeight);

            _inkRoll.transform.DOMove(new Vector3(_inkRollX, height, _inkRollZ), 0.5f);
        }

        private void PlaySound()
        {
            if (_finishingUI.PaintFinished)
            {
                _audioSource.Stop();

                return;
            }

            if (_painting && !_audioSource.isPlaying)
            {
                AudioManager.Instance.PlayRandomSFX(_audioSource, _paintingSFX);
            }
            else if (!_painting && _audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }
}
