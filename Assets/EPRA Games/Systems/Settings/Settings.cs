using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EPRA.Utilities
{
    public class Settings : MonoBehaviour
    {
        public static Settings Instance { get; private set; }

        [field: SerializeField] public Configuration Configuration { get; private set; }


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


        public void SetVibration(bool enabled)
        {
            Configuration.SetVibration(enabled);
        }

        public void SetSFX(bool enabled)
        {
            Configuration.EnableSFX(enabled);
        }

        public void SetSFXVolume(float volume)
        {
            Configuration.SetSFXVolume(volume);
        }

        public void SetMusic(bool enabled)
        {
            Configuration.EnableMusic(enabled);
        }

        public void SetMusicVolume(float volume)
        {
            Configuration.SetMusicVolume(volume);
        }

        public void SetFramerate(int target)
        {
            Configuration.SetFramerate(target);
        }

        public void SetLanguage(SystemLanguage systemLanguage)
        {
            Configuration.SetLanguage(systemLanguage);
        }
    }
}