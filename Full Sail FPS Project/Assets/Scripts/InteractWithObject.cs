using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractWithObject : MonoBehaviour
{
    private GameObject journal;
    private GameObject backpack;
    private GameObject dog;
    private GameObject note;
    private GameObject gun;


    [Header("-----Text Object References-----")]
    [SerializeField] TextMeshProUGUI momsNote;
    [SerializeField] TextMeshProUGUI gunNote;

    private TMP_Text taskTracker;


    private void Start()
    {
        journal = GameManager.Instance._journal;
        dog = GameManager.Instance._dog;
        backpack = GameManager.Instance._backpack;
        note = GameManager.Instance._note;
        gun = GameManager.Instance._gun;
        taskTracker = GameManager.Instance.taskTrackerText;



    }

    private void Update()
    {
        



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.interactWindow.SetActive(true);
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (journal != null && journal.activeInHierarchy)
                {
                    GameManager.Instance.hasJournal = true;
                   // this.gameObject.SetActive(false);
                    Destroy(gameObject); // Destroy the object after interaction
                    GameManager.Instance.interactWindow.SetActive(false);
                    
                    taskTracker.text = "Show Mom Game Design Journal.";
                    return;
                }

                if (backpack != null && backpack.activeInHierarchy)
                {
                    GameManager.Instance.hasBackpack = true;
                    Destroy(gameObject);
                    GameManager.Instance.interactWindow.SetActive(false);
                    taskTracker.text = "<s>" + taskTracker.text + "</s>\n";

                    taskTracker.text += "Go Pickup Sparky!";
                    return;
                }

                if (dog != null && dog.activeInHierarchy)
                {
                    GameManager.Instance.hasDog = true;
                    Destroy(gameObject);
                    GameManager.Instance.interactWindow.SetActive(false);
                    note.SetActive(true);
                    taskTracker.text = "<s>" + taskTracker.text + "</s>\n";

                    taskTracker.text += "Go Find Mom";

                    return;

                }
                if (note != null)
                {
                   
                   
                    StartCoroutine(noteRead());
                    return;

                }
                if (gun != null && gun.activeInHierarchy && GameManager.Instance.hasNote)
                {
                    if (GameManager.Instance.canHaveGun)
                    {
                        GameManager.Instance.hasGun = true;
                        Destroy(gameObject);
                        GameManager.Instance.interactWindow.SetActive(false);
                        taskTracker.text = "RUNNNNNN!!!!";

                        return;
                    }
                    else
                    {
                        StartCoroutine(gunNoteRead());
                      
                    }
                }
                



            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.interactWindow.SetActive(false);
        }
    }

    IEnumerator noteRead()
    {

        GameManager.Instance.hasNote = true;
        GameManager.Instance.interactWindow.SetActive(false);
        if (GameObject.FindGameObjectWithTag("FrontDoor") != null)
        {
            GameObject.FindGameObjectWithTag("FrontDoor").SetActive(false);
        }
        momsNote.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        momsNote.gameObject.SetActive(false);
        Destroy(gameObject);
        yield break;
    }
    IEnumerator gunNoteRead()
    {
        gunNote.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        gunNote.gameObject.SetActive(false);

    }
}
