using UnityEngine;
using TMPro;
using EPRA.Utilities;

namespace CIPA
{
    public class LogisticsAndDistributionUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _deliveriesText;
        [SerializeField] private TMP_Text _timeLeftText;

        [SerializeField] private LogisticsAndDistributionController _controller;


        protected virtual void LateUpdate()
        {
            UpdateDeliveries();
            UpdateTime();
        }


        private void UpdateDeliveries()
        {
            _deliveriesText.text = LanguageManager.GetTranslation("SteelIndustryDay5deliveries", _controller.CurrentDeliverySpotIndex, _controller.AmountOfDeliveries);
        }

        private void UpdateTime()
        {
            int minutes = (int)_controller.TimeLeft / 60;
            int seconds = (int)_controller.TimeLeft % 60;

            _timeLeftText.text = minutes + ":" + seconds.ToString("D2");
        }
    }
}
