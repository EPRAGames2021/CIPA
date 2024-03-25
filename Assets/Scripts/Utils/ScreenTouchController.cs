using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenTouchController : MonoBehaviour
{
    [SerializeField] private bool _firstPress;

    public bool FirstPress => _firstPress;

    private void OnEnable()
    {
        Input.simulateMouseWithTouches = true;

        _firstPress = false;
    }

    private void Update()
    {
        DetectFirstPress();
    }


    public void DetectFirstPress()
    {
        if (!IsPointerOverUIElement() && Input.GetMouseButtonDown(0))
        {
            _firstPress = true;
        }
        else
        {
            //Debug.Log("Is Pointer Over UI Element: " + IsPointerOverUIElement() + " | " + "Get Mouse Button Down: " + Input.GetMouseButtonDown(0));
        }
    }

    public bool DetectHolding()
    {
        if (Input.GetMouseButton(0) && _firstPress && !IsPointerOverUIElement())
        {
            //Debug.Log("Touching");

            return true;
        }
        else
        {
            //Debug.Log("Not touching");

            return false;
        }
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
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);

        eventData.position = Input.mousePosition;

        List<RaycastResult> raysastResults = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventData, raysastResults);

        return raysastResults;
    }
}