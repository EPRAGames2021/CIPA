using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    public class LanguageManager : MonoBehaviour
    {
        public static LanguageManager Instance { get; private set; }

        [SerializeField] private static bool _languagesLoaded;

        [SerializeField] private Configuration _configuration;

        [SerializeField] private SystemLanguage _currentLanguage;

        private static Dictionary<string, string> _fields;

        [SerializeField] private TextAsset _defaultLanguage;


        public static bool LanguagesLoaded => _languagesLoaded;


        public static event System.Action<SystemLanguage> OnLanguageChanged;


        private void Awake()
        {
            _languagesLoaded = false;

            InitSingleton();

            if (_configuration == null)
            {
                Debug.LogError("Configuration is null");

                return;
            }

            _currentLanguage = _configuration.TargetLanguage;

            ChangeLanguage(_currentLanguage);

            _languagesLoaded = true;
        }

        private void OnValidate()
        {
            if (_configuration == null)
            {
                Debug.LogError("Configuration is null");

                return;
            }

            ChangeLanguage(_currentLanguage);
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


        private void LoadLanguage()
        {
            if (_fields == null)
            {
                Debug.LogWarning("Language fields were empty.");
                _fields = new Dictionary<string, string>();
            }

            _fields.Clear();

            string currentLang = _currentLanguage.ToString();
            string translationStrings;

            if (Resources.Load(@"Languages/" + currentLang) as TextAsset == null)
            {
                Debug.LogWarning("File for selected language " + currentLang + " was not found. Using default language instead.");
                translationStrings = _defaultLanguage.text;
            }
            else
            {
                translationStrings = (Resources.Load(@"Languages/" + currentLang) as TextAsset).text;
            }

            string[] lines = translationStrings.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf("=") >= 0 && !lines[i].StartsWith("//"))
                {
                    string[] parts = lines[i].Split('=');

                    string key = parts[0].Trim();
                    string value = parts[1].Trim().Replace("\\n", Environment.NewLine);

                    _fields.Add(key, value);
                }
            }
        }

        public static string GetTranslation(string key, params object[] args)
        {
            if (!_fields.ContainsKey(key))
            {
                Debug.LogError("There is no key with name: [" + key + "] in your text files.");
                return "MISSING_STRING";
            }

            string translation = _fields[key];

            // Replace placeholders with actual values
            if (args != null && args.Length > 0)
            {
                translation = string.Format(translation, args);
            }

            return translation;
        }


        public void ChangeLanguage(SystemLanguage newLanguage)
        {
            _currentLanguage = newLanguage;

            LoadLanguage();

            OnLanguageChanged?.Invoke(_currentLanguage);
        }
    }
}