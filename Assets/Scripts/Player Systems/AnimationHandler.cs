using UnityEngine;
using EPRA.Utilities;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClipCollection _stepSFX;

    public void CallSFX()
    {
        AudioManager.Instance.PlayRandomSFX(_audioSource, _stepSFX);
    }
}
