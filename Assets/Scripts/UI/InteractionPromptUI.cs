using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EPRA.Utilities
{
    public class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] private GameObject uiPanel;

        [SerializeField] private Image promptImage;
        [SerializeField] private TextMeshProUGUI promptText;

        [SerializeField] private bool _displayed = false;

        public bool Displayed => _displayed;

        private void Start()
        {
            mainCam = Camera.main;

            _displayed = false;

            uiPanel.SetActive(_displayed);
        }

        private void LateUpdate()
        {
            if (mainCam == null)
            {
                mainCam = Camera.main;
            }

            Quaternion rotation = mainCam.transform.rotation;

            transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }

        public void OpenPanel()
        {
            _displayed = true;

            uiPanel.SetActive(_displayed);
        }

        public void ClosePanel()
        {
            _displayed = false;

            uiPanel.SetActive(_displayed);
        }
    }
}
