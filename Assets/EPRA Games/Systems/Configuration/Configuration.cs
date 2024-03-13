using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace EPRA.Utilities
{
    [CreateAssetMenu(fileName = "Configuration", menuName = "Scriptable Objects/Configuration", order = 1)]
    public class Configuration : ScriptableObject
    {
        [Header("Game settings")]
        [SerializeField] private AudioMixer _SFXMixer;
        [SerializeField] private AudioMixer _musicMixer;

        [SerializeField] private bool _canVibrate;
        [SerializeField] private bool _canPlaySFX;
        [Range(-80, 0), SerializeField] private float _SFXVolume;
        [SerializeField] private bool _canPlayMusic;
        [Range(-80, 0), SerializeField] private float _musicVolume;
        [Range(1, 240), SerializeField] private int _targetFramerate;
        [SerializeField] private SystemLanguage _targetLanguage;

        [Header("Build Settings")]
        [Header("iOS")]
        [SerializeField] private bool _hideHomeButtonOniPhoneX = true;
        //[SerializeField] private UnityEngine.iOS.SystemGestureDeferMode _gestureDeferMode = UnityEngine.iOS.SystemGestureDeferMode.All;

        public bool CanVibrate => _canVibrate;
        public bool CanPlaySFX => _canPlaySFX;
        public float SFXVolume => _SFXVolume;
        public bool CanPlayMusic => _canPlayMusic;
        public float MusicVolume => _musicVolume;
        public int TargetFramerate => _targetFramerate;
        public SystemLanguage TargetLanguage => _targetLanguage;


        private void OnEnable()
        {
            LoadData();

            UpdateMixer(_SFXMixer, _canPlaySFX);
            UpdateMixer(_musicMixer, _canPlayMusic);

            Vibrator.CanVibrate = _canVibrate;
        }

        private void OnValidate()
        {
            Application.targetFrameRate = _targetFramerate;

            //PlayerSettings.iOS.hideHomeButton = _hideHomeButtonOniPhoneX;
            //PlayerSettings.iOS.deferSystemGesturesMode = _gestureDeferMode;

            //For some reason, this option is marked as internal rather than public so we must enable it manually for the time being
            //PlayerSettings.iOS.appleEnableProMotion = true
        }

        private void LoadData()
        {
            _canVibrate = DataManager.HasData("CanVibrate") ? DataManager.LoadData<bool>("CanVibrate") : true;

            _canPlaySFX = DataManager.HasData("CanSound") ? DataManager.LoadData<bool>("CanSound") : true;
            _SFXVolume = DataManager.HasData("SFXVolume") ? DataManager.LoadData<float>("SFXVolume") : 1f;

            _canPlayMusic = DataManager.HasData("CanMusic") ? DataManager.LoadData<bool>("CanMusic") : true;
            _musicVolume = DataManager.HasData("MusicVolume") ? DataManager.LoadData<float>("MusicVolume") : 1f;

            _targetFramerate = DataManager.HasData("TargetFrameRate") ? DataManager.LoadData<int>("TargetFrameRate") : 60;

            _targetLanguage = DataManager.HasData("TargetLanguage") ? DataManager.LoadData<SystemLanguage>("TargetLanguage") : SystemLanguage.English;
        }


        public void SetVibration(bool canVibrate)
        {
            _canVibrate = canVibrate;

            Vibrator.CanVibrate = _canVibrate;

            DataManager.SaveData("CanVibrate", _canVibrate);
        }

        public void EnableSFX(bool canPlaySound)
        {
            _canPlaySFX = canPlaySound;

            DataManager.SaveData("CanSound", _canPlaySFX);

            UpdateMixer(_SFXMixer, _canPlaySFX);
        }

        public void SetSFXVolume(float volume)
        {
            _SFXVolume = volume;

            DataManager.SaveData("SFXVolume", _SFXVolume);

            UpdateMixer(_SFXMixer, _SFXVolume);
        }

        public void EnableMusic(bool canPlayMusic)
        {
            _canPlayMusic = canPlayMusic;

            DataManager.SaveData("CanMusic", _canPlayMusic);

            UpdateMixer(_musicMixer, _canPlayMusic);
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = volume;

            DataManager.SaveData("MusicVolume", _musicVolume);

            UpdateMixer(_musicMixer, _musicVolume);
        }

        public void SetFramerate(int target)
        {
            _targetFramerate = target;

            DataManager.SaveData("TargetFrameRate", _targetFramerate);

            Application.targetFrameRate = target;
        }

        public void SetLanguage(SystemLanguage systemLanguage)
        {
            _targetLanguage = systemLanguage;

            DataManager.SaveData("TargetLanguage", _targetLanguage);

            LanguageManager.Instance.ChangeLanguage(_targetLanguage);
        }


        private void UpdateMixer(AudioMixer _mixer, bool enabled)
        {
            if (_mixer == null)
            {
                Debug.LogWarning("Audio Mixer is null", this);

                return;
            }

            if (enabled)
            {
                _mixer.SetFloat("MixerVolume", 0);
            }
            else
            {
                _mixer.SetFloat("MixerVolume", -80);
            }
        }

        private void UpdateMixer(AudioMixer _mixer, float volume)
        {
            if (_mixer == null)
            {
                Debug.LogWarning("Audio Mixer is null", this);

                return;
            }

            _mixer.SetFloat("MixerVolume", volume);
        }
    }
}

