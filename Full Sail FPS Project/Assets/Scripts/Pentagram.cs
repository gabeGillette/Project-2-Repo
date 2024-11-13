using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagram : MonoBehaviour, IInteractable
{
    [SerializeField] Color skyColorChange = Color.red;  // Change to desired color
    [SerializeField] float spawnRadius = 10f;           // Radius around the pentagram to spawn monsters
    [SerializeField] GameObject monsterPrefab;          // Assign your monster prefab in the Inspector
    [SerializeField] int numMonsters = 5;               // Number of monsters to spawn

    private bool hasBeenActivated = false;

    public void OnInteract()
    {
        if (!hasBeenActivated)
        {
            hasBeenActivated = true;
            ChangeSkyColor();
            StartMonstersInBasement();
        }
    }

    private void ChangeSkyColor()
    {
        RenderSettings.ambientLight = skyColorChange;   // Simple sky color change
    }

    private void StartMonstersInBasement()
    {
        for (int i = 0; i < numMonsters; i++)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPos.y = transform.position.y; // Keep monsters on the same level as the pentagram

            Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
        }
    }
}
