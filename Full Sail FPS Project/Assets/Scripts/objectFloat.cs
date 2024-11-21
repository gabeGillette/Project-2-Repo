using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectFloat : MonoBehaviour
{
    // Variables to control the floating behavior
    public float amplitude = 1f;  // How far up and down the object moves
    public float speed = 1f;      // How fast the object floats
    public float rotationSpeed = 30f;  // Rotation speed (degrees per second)


    // Initial position of the object
    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * amplitude;

        // Apply the new Y position to the object while keeping the original X and Z
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotate the object around the Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
