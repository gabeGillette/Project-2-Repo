using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mimicEnemy : MonoBehaviour
{
    public Transform player;             // Reference to the player's transform
    public float mimicDelay = 1.0f;      // Delay in seconds for the mimic effect

    private Queue<Vector3> playerPositions; // Queue to store player positions
    private float lastUpdateTime;           // To track when the last position was updated

    void Start()
    {
        playerPositions = new Queue<Vector3>();
        lastUpdateTime = Time.time;
    }

    void Update()
    {
        // Add the player's position to the queue
        if (Time.time - lastUpdateTime >= mimicDelay)
        {
            playerPositions.Enqueue(player.position);
            lastUpdateTime = Time.time;
        }

        // If the queue has elements, move the enemy to the delayed player position
        if (playerPositions.Count > 0)
        {
            transform.position = playerPositions.Peek(); // Get the first (oldest) position in the queue
        }
    }
}
