using UnityEngine;

namespace CIPA
{
    public class IndicatorRotationFix : MonoBehaviour
    {
        [SerializeField] private GameObject _icon;

        private void Update()
        {
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);
        }

        private void LateUpdate()
        {
            _icon.transform.rotation = Quaternion.identity;
        }
    }
}
