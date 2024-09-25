using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPRA.Utilities;

namespace CIPA
{
    public class ScrapTreatmentUI : MonoBehaviour
    {
        [Header("Dev area")]
        [SerializeField] private List<Button> _treadmillButtons;
        [SerializeField] private List<TMP_Text> _buttonsText;

        [SerializeField] private TMP_Text _colletedCount;
        [SerializeField] private TMP_Text _lostCount;

        [SerializeField] private ScrapTreatmentController _scrapTreatmentController;


        private void OnEnable()
        {
            Init();
        }

        private void LateUpdate()
        {
            UpdateScore();
        }

        private void OnDisable()
        {
            Finish();
        }


        private void Init()
        {
            List<Treadmill> treadmills = _scrapTreatmentController.Treadmills;

            // Initialize buttons
            for (int i = 0; i < _treadmillButtons.Count; i++)
            {
                UpdateButton(!treadmills[i].Active, i);
            }

            // Add functionality to buttons
            for (int i = 0; i < _treadmillButtons.Count; i++)
            {
                int index = i;

                _treadmillButtons[index].onClick.AddListener(() => InteractWithTreadmill(index));
            }
        }

        private void Finish()
        {
            for (int i = 0; i < _treadmillButtons.Count; i++)
            {
                _treadmillButtons[i].onClick.RemoveAllListeners();
            }
        }

        private void UpdateScore()
        {
            _colletedCount.text = LanguageManager.GetTranslation("SteelIndustryDay4collectedcargo", _scrapTreatmentController.Collected, _scrapTreatmentController.RequiredToWin);
            _lostCount.text = LanguageManager.GetTranslation("SteelIndustryDay4droppedcargo", _scrapTreatmentController.Lost, _scrapTreatmentController.RequiredToLose);
        }


        private void InteractWithTreadmill(int index)
        {
            bool isActive = _scrapTreatmentController.SwitchTreadmillOn(index);

            UpdateButton(isActive, index);
        }

        private void UpdateButton(bool isActive, int index)
        {
            if (isActive)
            {
                _buttonsText[index].text = LanguageManager.GetTranslation("turnOn");
            }
            else
            {
                _buttonsText[index].text = LanguageManager.GetTranslation("turnOff");
            }
        }
    }
}
