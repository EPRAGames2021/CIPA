using System.Collections;
using UnityEngine;
using TMPro;

namespace EPRA.Utilities
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SimpleTranslate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtMesh;

        [Tooltip("If this field is null, the translation probably comes from another script")]
        [SerializeField] private string _key;


        public string Key { get { return _key; } set { _key = value; } }
        public string Text => _txtMesh.text;


        private void OnValidate()
        {
            _txtMesh = GetComponent<TextMeshProUGUI>();
        }

        
        private void OnEnable()
        {
            Translate();

            LanguageManager.OnLanguageChanged += ChangeLanguage;
        }

        private void OnDisable()
        {
            LanguageManager.OnLanguageChanged -= ChangeLanguage;
        }



        private void ChangeLanguage(SystemLanguage t_targetLanguage)
        {
            Translate();
        }

        private void Translate()
        {
            if (_key != string.Empty)
            {             
                if (LanguageManager.LanguagesLoaded)
                {
                    GetTranslation();
                }
                else
                {
                    StartCoroutine(GetTranslationDelayed());
                }
            }
        }


        private void GetTranslation()
        {
            _txtMesh.text = LanguageManager.GetTranslation(_key);
        }

        private IEnumerator GetTranslationDelayed()
        {
            _txtMesh.text = string.Empty;

            yield return new WaitUntil(() => LanguageManager.LanguagesLoaded);

            GetTranslation();
        }


        public void SetKey(string key)
        {
            _key = key;

            Translate();
        }

        public void Clear()
        {
            _txtMesh.text = string.Empty;
        }
    }
}