using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviours : MonoBehaviour
{
    private GameObject[] targetObjects;

    // Start is called before the first frame update
    void Start()
    {
        targetObjects = GameObject.FindGameObjectsWithTag("Label");

        foreach (GameObject target in targetObjects)
        {
            if (target != null)
            {
                target.SetActive(false); // Deactivate each GameObject
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            if (targetObjects == null)
            {
                Debug.LogError("targetObject is null. Make sure an object with the tag 'Label' exists in the scene.");
                return;
            }

            // Check the tag of the object this script is attached to
            if (this.CompareTag("LabelOn"))
            {
                foreach (GameObject targetObject in targetObjects)
                {
                    targetObject.SetActive(true);
                    Debug.Log("Object is now active.");
                }
            }
            else if (this.CompareTag("LabelOff"))
            {
                foreach (GameObject targetObject in targetObjects)
                {
                    targetObject.SetActive(false);
                    Debug.Log("Object is now inactive.");
                }
            }
        }
    }

   
}
