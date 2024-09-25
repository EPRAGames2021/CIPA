using UnityEngine;

namespace CIPA
{
    [System.Serializable]
    public class OffScreenIndicator
    {
        public Transform Target;
        public Transform IndicatorUI;
        public RectTransform RectTransform;
        public Sprite Sprite;
    }
}
