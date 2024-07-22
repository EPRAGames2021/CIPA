using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    [RequireComponent(typeof(AudioSource))]
    public class DumpTruck : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private ScreenTouchController _controller;

        [SerializeField] private bool _dumping;

        [Header("Sound")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClipCollection _dumpSFX;


        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (_controller == null)
            {
                return;
            }

            _dumping = _controller.DetectHolding();

            Dump();
            PlaySound();
        }


        private void Init()
        {
            _dumping = false;
        }

        private void Dump()
        {
            _animator.SetBool("IsDumping", _dumping);
        }

        private void PlaySound()
        {
            if (_dumping && !_audioSource.isPlaying) AudioManager.Instance.PlayRandomSFX(_dumpSFX, _audioSource);
            if (!_dumping && _audioSource.isPlaying) _audioSource.Stop();
        }
    }
}
