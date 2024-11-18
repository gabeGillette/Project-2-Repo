using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Pentagram : MonoBehaviour, IInteractable
{
    [SerializeField] Color skyColorChange = Color.red;  // Change to desired color
    [SerializeField] Material skyMaterial;
    //[SerializeField] float spawnRadius = 10f;           // Radius around the pentagram to spawn monsters
    //[SerializeField] GameObject monsterPrefab;          // Assign your monster prefab in the Inspector
    //[SerializeField] int numMonsters = 5;               // Number of monsters to spawn

    private bool hasBeenActivated = false;
    private GameObject escapeWall;
    private  GameObject exitSign;

    private TMP_Text taskTracker;

    AudioSource audioSource;



    private void Start()
    {
        escapeWall = GameObject.FindGameObjectWithTag("EscapeWall");
        exitSign = GameObject.FindGameObjectWithTag("ExitSign");
        taskTracker = GameManager.Instance.taskTrackerText;
        audioSource = GetComponent<AudioSource>();


    }

    public void OnInteract()
    {
       
    }

    private void ChangeSkyColor()
    {
        RenderSettings.ambientLight = skyColorChange;   // Simple sky color change
        RenderSettings.skybox = skyMaterial;
    }

    //private void StartMonstersInBasement()
    //{
    //    for (int i = 0; i < numMonsters; i++)
    //    {
    //        Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
    //        spawnPos.y = transform.position.y; // Keep monsters on the same level as the pentagram

    //        Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
    //    }
    //}




    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")))
        {
            if (!hasBeenActivated)
            {
                hasBeenActivated = true;
                GameManager.Instance.canHaveGun = true;
                taskTracker.text = "<s>" + taskTracker.text + "</s>\n";

                taskTracker.text += "Grab Gun!";
                ChangeSkyColor();
                PlayAudio();

                // StartMonstersInBasement();
                Destroy(escapeWall);

            }
        }
    }

    public void PlayAudio()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play(); // Play the audio
        }
    }

}
