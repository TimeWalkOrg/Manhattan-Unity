using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerOverUI
{
    public static bool IsPointerOverUIObject(Vector2 touchPosition)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = touchPosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        return raycastResults.Count > 0;
    }
}

