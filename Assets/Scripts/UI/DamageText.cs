
using UnityEngine;
using TMPro;

namespace CIPA
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _positive;
        [SerializeField] private TMP_Text _negative;


        private void LateUpdate()
        {
            transform.LookAt(Camera.main.transform.position);
        }

        public void Setup(string text, bool positive)
        {
            _positive.gameObject.SetActive(positive);
            _negative.gameObject.SetActive(!positive);

            _positive.text = "+" + text;
            _negative.text = "-" + text;
        }
    }
}
