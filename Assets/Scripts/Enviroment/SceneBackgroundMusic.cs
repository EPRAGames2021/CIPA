using UnityEngine;
using EPRA.Utilities;

namespace CIPA
{
    [RequireComponent(typeof(AudioSource))]
    public class SceneBackgroundMusic : MonoBehaviour
    {
        [SerializeField] private AudioClipCollection _musicClipCollection;

        [SerializeField] private AudioSource _audioSource;


        private void OnValidate()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
        }

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
            AudioManager.Instance.SetMusic(_musicClipCollection, true);
        }

        private void Finish()
        {
            AudioManager.Instance.StopMusic();
        }
    }
}
