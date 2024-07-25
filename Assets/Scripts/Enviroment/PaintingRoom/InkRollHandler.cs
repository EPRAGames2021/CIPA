using UnityEngine;
using DG.Tweening;
using EPRA.Utilities;

namespace CIPA
{
    public class InkRollHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _inkRoll;
        [SerializeField] private MeshRenderer _paintRenderer;

        private float _inkRollX;
        private float _inkRollZ;

        [SerializeField] private float _inkRollMinHeight;
        [SerializeField] private float _inkRollMaxHeight;

        [SerializeField] private float _completionPercentage;

        [SerializeField] private bool _completed;
        [SerializeField] private bool _painting;
        [SerializeField] private float _idleTime;
        [SerializeField] private float _idleTimeTolerance;

        [Header("Sound")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClipCollection _paintingSFX;


        private void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            PaintingBehaviour();
            UpdateColor();
            PlaySound();
        }


        private void Init()
        {
            SetPainting(false);
            SetCompleted(false);
            SetCompletion(0f);

            _inkRollX = _inkRoll.transform.position.x;
            _inkRollZ = _inkRoll.transform.position.z;


            Color color = new(_paintRenderer.material.color.r, _paintRenderer.material.color.g, _paintRenderer.material.color.b, 0.0f);
            _paintRenderer.material.color = color;
        }


        private void PaintingBehaviour()
        {
            if (_painting)
            {
                _idleTime = 0f;
            }
            else
            {
                _idleTime += Time.deltaTime;
            }

            if (_idleTime >= _idleTimeTolerance)
            {
                _painting = false;
            }
        }

        private void UpdateColor()
        {
            if (_completed) return;

            float alpha = Remap.RemapValue(_completionPercentage, 0, 100, 0, 1);

            Color newColor = new(_paintRenderer.material.color.r, _paintRenderer.material.color.g, _paintRenderer.material.color.b, alpha);
            _paintRenderer.material.color = newColor;
        }

        private void PlaySound()
        {
            if (_completed)
            {
                _audioSource.Stop();

                return;
            }

            if (_painting && !_audioSource.isPlaying)
            {
                AudioManager.Instance.PlayRandomSFX(_paintingSFX, _audioSource);
            }
            else if (!_painting && _audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }

        public void Refresh()
        {
            Init();
        }

        public void SetPainting(bool painting)
        {
            _painting = painting;
        }

        public void SetCompletion(float completion)
        {
            _completionPercentage = completion;
        }

        public void SetCompleted(bool completed)
        {
            _completed = completed;

            _inkRoll.SetActive(!_completed);
        }

        public void UpdateRollPosition(float heightPercentage)
        {
            if (_completed) return;

            float height = Remap.RemapValue(heightPercentage, 0, 100, _inkRollMinHeight, _inkRollMaxHeight);

            _inkRoll.transform.DOMove(new Vector3(_inkRollX, height, _inkRollZ), 0.5f);
        }
    }
}
