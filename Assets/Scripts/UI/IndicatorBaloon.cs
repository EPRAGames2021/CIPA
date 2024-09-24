using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class IndicatorBaloon : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void SetIcon(Image icon)
        {
            _icon = icon;
        }
    }
}
