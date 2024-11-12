using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviours : MonoBehaviour
{
    private GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.FindWithTag("Label");


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            if (targetObject == null)
            {
                Debug.LogError("targetObject is null. Make sure an object with the tag 'Label' exists in the scene.");
                return;
            }

            // Check the tag of the object this script is attached to
            if (this.CompareTag("LabelOn"))
            {
                targetObject.SetActive(true);
                Debug.Log("Object is now active.");
            }
            else if (this.CompareTag("LabelOff"))
            {
                targetObject.SetActive(false);
                Debug.Log("Object is now inactive.");
            }
        }
    }

   
}
