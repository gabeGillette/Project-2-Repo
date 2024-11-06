using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraDoorScript
{
    public class CameraOpenDoor : MonoBehaviour
    {
        public float DistanceOpen = 3f; // Set this to the desired distance range
        public GameObject text;

        void Start()
        {
            // Optional: Initialize if needed
        }

        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, DistanceOpen))
            {
                if (hit.transform.GetComponent<DoorScript.Door>())
                {
                    // Calculate the distance between the camera and the door
                    float distanceToDoor = Vector3.Distance(transform.position, hit.transform.position);
                    Debug.Log("Distance to door: " + distanceToDoor);

                    // Display text if the door is within interaction range
                    if (distanceToDoor <= DistanceOpen)
                    {
                        text.SetActive(true);
                    }
                    else
                    {
                        text.SetActive(false);
                    }

                    // Check if 'E' is pressed and the player is within the distance to open
                    if (Input.GetKeyDown(KeyCode.E) && distanceToDoor <= DistanceOpen && distanceToDoor > 0)
                    {
                        hit.transform.GetComponent<DoorScript.Door>().OpenDoor();
                    }
                }
                else
                {
                    text.SetActive(false);
                }
            }
            else
            {
                text.SetActive(false);
            }
        }
    }
}
