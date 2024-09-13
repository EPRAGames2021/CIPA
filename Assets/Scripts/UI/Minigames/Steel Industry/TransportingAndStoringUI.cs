using UnityEngine;
using TMPro;
using EPRA.Utilities;

namespace CIPA
{
    public class TransportingAndStoringUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _deliveriesText;

        [SerializeField] private ResourceVehicleTransportationController _controller;


        protected virtual void LateUpdate()
        {
            UpdateDeliveries();
        }


        private void UpdateDeliveries()
        {
            _deliveriesText.text = LanguageManager.GetTranslation("SteelIndustryDay5deliveries", _controller.CurrentDeliverySpotIndex, _controller.AmountOfDeliveries);
        }
    }
}
