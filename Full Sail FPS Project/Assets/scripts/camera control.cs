using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 100f; // Mouse sensitivity
    [SerializeField] private float lockVertMin = -60f; // Minimum vertical angle
    [SerializeField] private float lockVertMax = 60f; // Maximum vertical angle
    [SerializeField] private bool invertY = false; // Invert Y-axis

    private float rotX = 0f; // Current rotation on X-axis

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse input
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        // Adjust X rotation based on Y mouse movement
        rotX += invertY ? mouseY : -mouseY;

        // Clamp the X rotation to prevent flipping
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        // Apply rotation to the camera
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        // Rotate the player on the Y-axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
