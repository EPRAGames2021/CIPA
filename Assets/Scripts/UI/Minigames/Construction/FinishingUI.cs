using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class FinishingUI : MonoBehaviour
    {
        [SerializeField] private Slider _paintSlider;

        [SerializeField] private ScreenTouchController _screenTouchController;
        [SerializeField] private MouseDelta _mouseDelta;
        [SerializeField] private MousePositionHandler _mousePositionHandler;

        public Slider PaintSlider => _paintSlider;

        public ScreenTouchController ScreenTouchController => _screenTouchController;
        public MouseDelta MouseDelta => _mouseDelta;
        public MousePositionHandler MousePositionHandler => _mousePositionHandler;
    }
}
