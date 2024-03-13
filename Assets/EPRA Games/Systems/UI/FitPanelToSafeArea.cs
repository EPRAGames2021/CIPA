using UnityEngine;

namespace EPRA.Utilities
{
    public class FitPanelToSafeArea : MonoBehaviour
    {
        [SerializeField] private RectTransform _panelToFit;

        private void OnEnable()
        {
            Init();

            FitPanel();
        }

        private void OnValidate()
        {
            Init();
        }

#if UNITY_EDITOR
        public void Update()
        {
            /*
             * This exists for testing purposes.
             * For instance, if a dev wants to test whether or not the interface fits properly in the screen of a different device.
             * 
             * OnValidate() doesn't work very well adjusting Rect Transforms if you're editing prefabs
            */

            FitPanel();
        }
#endif


        private void Init()
        {
            _panelToFit = GetComponent<RectTransform>();
        }

        private void FitPanel()
        {
            if (_panelToFit == null)
            {
                Debug.LogWarning(this + " has an empty safe area rectTransform.");

                return;
            }

            Rect safeArea = Screen.safeArea;
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            _panelToFit.anchorMin = anchorMin;
            _panelToFit.anchorMax = anchorMax;
        }
    }
}
