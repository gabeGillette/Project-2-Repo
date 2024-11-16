using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIInputHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                if (result.gameObject.GetComponent<UnityEngine.UI.Button>() != null)
                {
                    result.gameObject.GetComponent<UnityEngine.UI.Button>().OnPointerClick(pointerData);
                    //Debug.Log("gyuguiyg");
                }
            }
        }
    }
}
