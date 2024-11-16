using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JournalMenu : GraphicRaycaster
{
    /// <summary>
    /// The camera used to render the journal menu
    /// </summary>
    [SerializeField] Camera JournalCamera;
    
    [SerializeField] GraphicRaycaster JuornalMenuCanvas; 

    // We need to translate our raycast into the space of the journal menu.
    public override void Raycast(PointerEventData data, List<RaycastResult> result)
    {
        Ray ray = eventCamera.ScreenPointToRay(data.position); // Mouse
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform == transform)
            {
                // determine the pointer position in the space of the journal menu
                Vector3 pos = new Vector3(hit.textureCoord.x, hit.textureCoord.y);
                pos.x *= JournalCamera.targetTexture.width;
                pos.y *= JournalCamera.targetTexture.height;

                data.position = pos;

                JuornalMenuCanvas.Raycast(data, result);
            }
        }
    }

}