using UnityEngine;
using EPRA.Utilities;

[RequireComponent(typeof(AudioSource))]

public class ConcreteForm : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private bool _shouldPlayAnimation;
    [SerializeField] private bool _isAnimationPlaying;
    [SerializeField] private string _animationParameterName;

    [Header("Sound")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClipCollection _clipSFX;


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!_shouldPlayAnimation) return;

        HandleAnimation();
        PlaySound();
    }


    private void Init()
    {
        _shouldPlayAnimation = false;
        _isAnimationPlaying = false;
    }

    public void SetActive(bool active)
    {
        _shouldPlayAnimation = active;
    }

    private void HandleAnimation()
    {
        if (_shouldPlayAnimation && !_isAnimationPlaying)
        {
            _animator.SetBool(_animationParameterName, true);

            _isAnimationPlaying = true;
        }
        else if (!_shouldPlayAnimation && _isAnimationPlaying)
        {
            _animator.speed = 0f;

            _isAnimationPlaying = false;
        }
    }

    private void PlaySound()
    {
        if (_shouldPlayAnimation && !_audioSource.isPlaying) AudioManager.Instance.PlayRandomSFX(_audioSource, _clipSFX);
        if (!_shouldPlayAnimation && _audioSource.isPlaying) _audioSource.Stop();
    }
}
