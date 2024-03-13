using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
    
        [SerializeField] private Configuration _configuration;
    
        [SerializeField] private AudioSource FXAudio;
        [SerializeField] private AudioSource MusicAudio;
    
        [SerializeField] private AudioClipCollection _musicCollection;
        [SerializeField] private AudioClip _music;
    
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
    
    
        public void SetMusic(AudioClipCollection musicCollection, bool playRightAway)
        {
            SetMusic(musicCollection);
    
            if (playRightAway)
            {
                PlayMusic();
            }
        }
    
        public void SetMusic(AudioClipCollection musicCollection)
        {
            if (!CollectionIsValid(musicCollection)) return;
    
            _musicCollection = musicCollection;
    
            AudioClip music = _musicCollection.GetRandomClip();
    
            if (!AudioClipIsValid(music)) return;
    
            _music = music;
    
            MusicAudio.clip = _music;
            MusicAudio.volume = musicCollection.Volume;
        }
    
    
        public void PlaySFX(AudioSource audioSource, AudioClipCollection clipCollection)
        {
            if (!CollectionIsValid(clipCollection)) return;
    
            PlayClip(audioSource, clipCollection.GetFirstClip(), clipCollection.Volume);
        }
    
        public void PlaySFX(AudioClipCollection clipCollection)
        {
            if (!CollectionIsValid(clipCollection)) return;
    
            PlayClip(clipCollection.GetFirstClip(), clipCollection.Volume);
        }
    
        public void PlayRandomSFX(AudioSource audioSource, AudioClipCollection clipCollection)
        {
            if (!CollectionIsValid(clipCollection)) return;
    
            PlayClip(audioSource, clipCollection.GetRandomClip(), clipCollection.Volume);
        }
    
        public void PlayRandomSFX(AudioClipCollection clipCollection)
        {
            if (!CollectionIsValid(clipCollection)) return;
    
            PlayClip(clipCollection.GetRandomClip(), clipCollection.Volume);
        }
    
    
        public void PlayMusic()
        {
            if (_music == null || !MainAudioIsValid()) return;
    
            if (!MusicAudio.isPlaying) MusicAudio.Play();
        }
    
        public void StopMusic()
        {
            if (_music == null || !MainAudioIsValid()) return;
    
            if (MusicAudio.isPlaying) MusicAudio.Stop();
        }
    
    
        private bool MainAudioIsValid()
        {
            bool isAudioSourceNull = FXAudio == null;
            if (isAudioSourceNull) Debug.LogWarning("Audio Source is null", this);
    
            return !isAudioSourceNull;
        }
    
        private bool CollectionIsValid(AudioClipCollection clipCollection)
        {
            bool isClipCollectionNull = clipCollection == null;
            if (isClipCollectionNull) Debug.LogWarning("ClipCollection is null", this);
    
            return !isClipCollectionNull;
        }
    
        private bool AudioClipIsValid(AudioClip clip)
        {
            bool isClipNull = clip == null;
            if (isClipNull) Debug.LogWarning("Audio Clip is null", this);
    
            return !isClipNull;
        }
    
        private void PlayClip(AudioSource audiosource, AudioClip clip, float volume)
        {
            if (!AudioClipIsValid(clip)) return;
    
            if (audiosource != null)
            {
                audiosource.clip = clip;
                audiosource.volume = volume;
                audiosource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource is null. Using default AudioSource instead");
                PlayClip(clip, volume);
            }
        }
    
        private void PlayClip(AudioClip clip, float volume)
        {
            if (!AudioClipIsValid(clip)) return;
    
            FXAudio.PlayOneShot(clip, volume);
        }
    }
}