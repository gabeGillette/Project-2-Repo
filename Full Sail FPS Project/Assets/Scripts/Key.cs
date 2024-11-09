using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : InteractableBase
{
    public int keyID;

    protected override void Activate()
    {
        PlayerInventory.instance.AddKey(keyID);
        Debug.Log($"{interactableName} collected with Key ID: {keyID}");
    }
}
