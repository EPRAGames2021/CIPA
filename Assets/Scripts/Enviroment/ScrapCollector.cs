using UnityEngine;

namespace CIPA
{
    public class ScrapCollector : MonoBehaviour
    {
        // Meant to determine whether scrap was dropped on the floor on onto a truck
        [SerializeField] private bool _correctDisposal;

        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out Scrap scrap);

            if (scrap != null )
            {
                scrap.Collect(_correctDisposal);
            }
        }
    }
}
