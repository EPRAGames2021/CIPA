using CIPA;
using UnityEngine;

namespace EPRA.Utilities
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
    
        [SerializeField] private Configuration _configuration;

        [SerializeField] private AudioSource _fxAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;

        [SerializeField] private AudioListener _currentListener;

        [Header("Currently playing")]
        [SerializeField] private AudioClipCollection _musicCollection;
        [SerializeField] private AudioClip _music;

        [Header("Defaults")]
        [SerializeField] private AudioClipCollection _defaultMenuMusicCollection;
        [SerializeField] private AudioClipCollection _defaultGameplayMusicCollection;
    
        [Tooltip("Not required but can come in handy")]
        public AudioClipCollection DefaultMainMenuMusicCollection => _defaultMenuMusicCollection;
    
        [Tooltip("Not required but can come in handy")]
        public AudioClipCollection DefaultGameplayMenuCollection => _defaultGameplayMusicCollection;
    
    
        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            Finish();
        }

        private void Update()
        {
            PlayMusic();
        }


        private void InitSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Init()
        {
            _configuration.InitializeMixers();

            InputHandler.Instance.OnMovementSystemChanged += ChangeListener;
        }

        private void Finish()
        {
            InputHandler.Instance.OnMovementSystemChanged -= ChangeListener;
        }


        private void ChangeListener(GameObject gameObject)
        {
            if (_currentListener != null)
            {
                AudioListener audioListener = _currentListener.GetComponent<AudioListener>();

                if (audioListener != null)
                {
                    Destroy(audioListener);
                }
            }

            gameObject.AddComponent<AudioListener>();
            _currentListener = gameObject.GetComponent<AudioListener>();
        }
    
    
        public void SetMusic(AudioClipCollection musicCollection, bool playRightAway)
        {
            SetMusicPlaylist(musicCollection);
    
            if (playRightAway)
            {
                PlayMusic();
            }
        }
    
        public void SetMusicPlaylist(AudioClipCollection musicCollection)
        {
            if (!CollectionIsValid(musicCollection)) return;
    
            _musicCollection = musicCollection;
    
            AudioClip music = _musicCollection.GetRandomClip();
    
            if (!AudioClipIsValid(music)) return;
    
            _music = music;
    
            _musicAudioSource.clip = _music;
            _musicAudioSource.volume = musicCollection.Volume;
        }
    
    
        public void PlaySFX(AudioClipCollection clipCollection, int index = 0, AudioSource audioSource = null)
        {
            if (!CollectionIsValid(clipCollection)) return;

            if (index > clipCollection.Count) index = clipCollection.Count;

            PlayClip(clipCollection.GetClip(index), clipCollection.Volume, audioSource);
        }
    
        public void PlayRandomSFX(AudioClipCollection clipCollection, AudioSource audioSource = null)
        {
            if (!CollectionIsValid(clipCollection)) return;

            int random = Random.Range(0, clipCollection.Count);

            PlaySFX(clipCollection, random, audioSource);
        }
    
    
        public void PlayMusic()
        {
            if (_music == null || !MainAudioIsValid()) return;
    
            if (!_musicAudioSource.isPlaying) _musicAudioSource.Play();
        }
    
        public void StopMusic()
        {
            if (_music == null || !MainAudioIsValid()) return;
    
            if (_musicAudioSource.isPlaying) _musicAudioSource.Stop();
        }
    
    
        private bool MainAudioIsValid()
        {
            bool isAudioSourceNull = _fxAudioSource == null;

            if (isAudioSourceNull) Debug.LogWarning("Audio Source is null", this);
    
            return !isAudioSourceNull;
        }
    
        
        private bool CollectionIsValid(AudioClipCollection clipCollection)
        {
            bool isClipCollectionValid = clipCollection != null;

            if (!isClipCollectionValid)
            {
                Debug.LogWarning($"{clipCollection} is null", this);
            }
    
            return isClipCollectionValid;
        }
    
        private bool AudioClipIsValid(AudioClip clip)
        {
            bool isClipValid = clip != null;

            if (!isClipValid)
            {
                Debug.LogWarning($"{clip} is null", this);
            }

            return isClipValid;
        }
    
        private void PlayClip(AudioClip clip, float volume, AudioSource audioSource)
        {
            if (!AudioClipIsValid(clip)) return;
    
            if (audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.volume = volume;
                audioSource.Play();
            }
            else
            {
                //Debug.LogWarning("AudioSource is null. Using default AudioSource instead");

                _fxAudioSource.PlayOneShot(clip, volume);
            }
        }
    }
}