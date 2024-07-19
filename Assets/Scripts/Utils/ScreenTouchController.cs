using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CIPA
{
    public class ScreenTouchController : MonoBehaviour
    {
        [SerializeField] private bool _firstPress;

        public bool FirstPress => _firstPress;


        public event System.Action OnPressed;


        private void OnEnable()
        {
            Input.simulateMouseWithTouches = true;

            ReInit();
        }

        private void Update()
        {
            DetectFirstPress();
        }


        public void DetectFirstPress()
        {
            if (!IsPointerOverUIElement() && Input.GetMouseButtonDown(0))
            {
                OnPressed?.Invoke();

                SetFirstPress(true);
            }
        }

        public bool DetectHolding()
        {
            if (Input.GetMouseButton(0) && _firstPress && !IsPointerOverUIElement())
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void ReInit()
        {
            SetFirstPress(false);
        }

        private void SetFirstPress(bool firstPress)
        {
            _firstPress = firstPress;
        }


        public bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }


        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];

                //print(curRaysastResult.gameObject.name);

                if (curRaysastResult.gameObject.layer == 5)
                {
                    return true;
                }
            }

            return false;
        }

        //Gets all event system raycast results of current mouse or touch position.
        private static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);

            eventData.position = Input.mousePosition;

            List<RaycastResult> raysastResults = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventData, raysastResults);

            return raysastResults;
        }
    }
}