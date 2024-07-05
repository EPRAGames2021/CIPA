using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    [RequireComponent(typeof(AudioSource))]
    public class SceneBackgroundSFX : MonoBehaviour
    {
        [SerializeField] private AudioClipCollection _backgroundSFXClipCollection;

        [SerializeField] private AudioSource _audioSource;


        private void OnValidate()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
        }

        private void Update()
        {
            PlaySFX();
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void PlaySFX()
        {
            if (!_audioSource.isPlaying)
            {
                AudioManager.Instance.PlayRandomSFX(_backgroundSFXClipCollection, _audioSource);
            }
        }

        private void Finish()
        {
            _audioSource.Stop();
        }
    }
}
