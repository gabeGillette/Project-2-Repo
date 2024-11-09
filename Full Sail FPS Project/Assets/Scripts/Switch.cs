using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : InteractableBase
{
    public GameObject[] objectsToToggle;

    protected override void Activate()
    {
        foreach (var obj in objectsToToggle)
        {
            obj.SetActive(!obj.activeSelf);
        }
        Debug.Log($"{interactableName} toggled objects.");
    }
}
