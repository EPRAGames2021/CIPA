using UnityEngine;
using TMPro;
using EPRA.Utilities;

namespace CIPA
{
    public class TransportingAndStoringUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _deliveriesText;

        [SerializeField] private ResourceVehicleTransportationController _controller;


        private void LateUpdate()
        {
            UpdateDeliveries();
        }


        private void UpdateDeliveries()
        {
            _deliveriesText.text = LanguageManager.GetTranslation("SteelIndustryDay4deliveries", _controller.CurrentDeliverySpotIndex, _controller.AmountOfDeliveries);
        }
    }
}
