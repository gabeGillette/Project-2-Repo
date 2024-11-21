using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class RiflePickup : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject mainUI;               // Reference to the main UI (health bar, rifle, reticle)
    [SerializeField] float spawnRadius = 10f;         // Radius for ground level spawns
    [SerializeField] GameObject monsterPrefab;        // Assign the monster prefab for ground level
    [SerializeField] int numMonstersGround = 5;       // Number of monsters to spawn on ground level

    private bool hasBeenPickedUp = false;

    private TMP_Text taskTracker;

    void Start()
    {
        taskTracker = GameManager.Instance.taskTrackerText;

    }

    public void OnInteract()
    {
        if (!hasBeenPickedUp)
        {
            hasBeenPickedUp = true;
            ActivateMainUI();
            taskTracker.text = "RUN TO THE FOREST TO ESCAPE!";

            //SpawnMonstersOnGroundLevel();
            Destroy(gameObject); // Remove the rifle pickup after interaction
        }
    }

    private void ActivateMainUI()
    {
        if (mainUI != null)
        {
            mainUI.SetActive(true);  // Show health bar, rifle, and reticle
        }
    }

    private void SpawnMonstersOnGroundLevel()
    {
        for (int i = 0; i < numMonstersGround; i++)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPos.y = transform.position.y; // Keep monsters on ground level

            Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
        }
    }
}