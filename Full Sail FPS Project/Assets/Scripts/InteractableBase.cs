using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [Header("Interactable Settings")]
    public string interactableName;
    public bool isOneTimeUse = true;
    private bool isUsed = false;

    // Method called when the player interacts
    public void Interact()
    {
        if (!isUsed)
        {
            Activate();
            if (isOneTimeUse)
            {
                isUsed = true;
            }
        }
    }

    // Abstract method to define custom behavior for each interactable for the project
    protected abstract void Activate();
}
