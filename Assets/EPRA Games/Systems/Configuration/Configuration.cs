using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

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
        [SerializeField] private int _languageIndex;

        [Header("Default values and debug")]
        [SerializeField] private List<SystemLanguage> _languages;
        [SerializeField] private SystemLanguage _defaultLanguage;

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
        public int LanguageIndex => _languageIndex;


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

            _languageIndex = GetLanguageIndex();

            //PlayerSettings.iOS.hideHomeButton = _hideHomeButtonOniPhoneX;
            //PlayerSettings.iOS.deferSystemGesturesMode = _gestureDeferMode;

            //For some reason, this option is marked as internal rather than public so we must enable it manually for the time being
            //PlayerSettings.iOS.appleEnableProMotion = true
        }

        private void LoadData()
        {
            _canVibrate = DataManager.HasData("CanVibrate") ? DataManager.LoadData<bool>("CanVibrate") : true;

            _canPlaySFX = DataManager.HasData("CanPlaySFX") ? DataManager.LoadData<bool>("CanPlaySFX") : true;
            _SFXVolume = DataManager.HasData("SFXVolume") ? DataManager.LoadData<float>("SFXVolume") : 1f;

            _canPlayMusic = DataManager.HasData("CanPlayMusic") ? DataManager.LoadData<bool>("CanPlayMusic") : true;
            _musicVolume = DataManager.HasData("MusicVolume") ? DataManager.LoadData<float>("MusicVolume") : 1f;

            _targetFramerate = DataManager.HasData("TargetFrameRate") ? DataManager.LoadData<int>("TargetFrameRate") : 60;

            _targetLanguage = DataManager.HasData("TargetLanguage") ? DataManager.LoadData<SystemLanguage>("TargetLanguage") : _defaultLanguage;
            //_languageIndex = DataManager.HasData("LanguageIndex") ? DataManager.LoadData<int>("LanguageIndex") : GetLanguageIndex();
        }

        private void SaveData()
        {
            DataManager.SaveData("CanVibrate", _canVibrate);

            DataManager.SaveData("CanPlaySFX", _canPlaySFX);
            DataManager.SaveData("SFXVolume", _SFXVolume);

            DataManager.SaveData("CanPlayMusic", _canPlayMusic);
            DataManager.SaveData("MusicVolume", _musicVolume);

            DataManager.SaveData("TargetFrameRate", _targetFramerate);

            DataManager.SaveData("TargetLanguage", _targetLanguage);
            DataManager.SaveData("LanguageIndex", _languageIndex);
        }


        public void SetVibration(bool canVibrate)
        {
            _canVibrate = canVibrate;

            Vibrator.CanVibrate = _canVibrate;

            SaveData();
        }

        public void EnableSFX(bool canPlaySound)
        {
            _canPlaySFX = canPlaySound;

            SaveData();

            UpdateMixer(_SFXMixer, _canPlaySFX);
        }

        public void SetSFXVolume(float volume)
        {
            _SFXVolume = volume;

            SaveData();

            UpdateMixer(_SFXMixer, _SFXVolume);
        }

        public void EnableMusic(bool canPlayMusic)
        {
            _canPlayMusic = canPlayMusic;

            SaveData();

            UpdateMixer(_musicMixer, _canPlayMusic);
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = volume;

            SaveData();

            UpdateMixer(_musicMixer, _musicVolume);
        }

        public void SetFramerate(int target)
        {
            _targetFramerate = target;

            SaveData();

            Application.targetFrameRate = target;
        }

        public void SetLanguage(SystemLanguage systemLanguage)
        {
            _targetLanguage = systemLanguage;

            SaveData();

            LanguageManager.Instance.ChangeLanguage(_targetLanguage);
        }

        public void SetLanguageIndex(int index)
        {
            _languageIndex = index;

            SaveData();
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

        private int GetLanguageIndex()
        {
            for (int i = 0; i < _languages.Count; i++)
            {
                if (_languages[i] == _defaultLanguage)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}

