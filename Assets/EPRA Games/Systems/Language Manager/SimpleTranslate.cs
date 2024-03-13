using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EPRA.Utilities
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SimpleTranslate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtMesh;

        [SerializeField] private string _key;


        private void OnValidate()
        {
            _txtMesh = GetComponent<TextMeshProUGUI>();
        }

        
        private void OnEnable()
        {
            GetTranslation();

            LanguageManager.OnLanguageChanged += ChangeLanguage;
        }

        private void OnDisable()
        {
            LanguageManager.OnLanguageChanged -= ChangeLanguage;
        }



        private void ChangeLanguage(SystemLanguage t_targetLanguage)
        {
            GetTranslation();
        }

        private void GetTranslation()
        {
            _txtMesh.text = LanguageManager.GetTranslation(_key);
        }
    }
}

