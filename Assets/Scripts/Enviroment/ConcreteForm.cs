using UnityEngine;
using EPRA.Utilities;

[RequireComponent(typeof(AudioSource))]

public class ConcreteForm : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private bool _shouldPlayAnimation;
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
        SetActive(false);
    }


    public void SetActive(bool active)
    {
        _shouldPlayAnimation = active;
    }

    public void Restart()
    {
        SetActive(false);
        _animator.SetBool(_animationParameterName, _shouldPlayAnimation);
        _animator.SetTrigger("Reset");
    }

    private void HandleAnimation()
    {
        _animator.SetBool(_animationParameterName, _shouldPlayAnimation);
    }

    private void PlaySound()
    {
        if (_shouldPlayAnimation && !_audioSource.isPlaying) AudioManager.Instance.PlayRandomSFX(_clipSFX, _audioSource);
        if (!_shouldPlayAnimation && _audioSource.isPlaying) _audioSource.Stop();
    }
}
