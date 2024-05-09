using UnityEngine;
using EPRA.Utilities;

[RequireComponent(typeof(AudioSource))]
public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClipCollection _stepSFX;


    private void OnValidate()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

    public void CallSFX()
    {
        AudioManager.Instance.PlayRandomSFX(_audioSource, _stepSFX);
    }
}
