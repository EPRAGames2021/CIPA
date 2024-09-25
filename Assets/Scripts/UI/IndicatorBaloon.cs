using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class IndicatorBaloon : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void SetIcon(Sprite icon)
        {
            if (icon != null)
            {
                _icon.sprite = icon;
            }
            else
            {
                _icon.sprite = null;
            }
        }
    }
}
