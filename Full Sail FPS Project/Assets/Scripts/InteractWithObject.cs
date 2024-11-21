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
    private GameObject bedroomDoor;

    //private GlowObject journalGlow;
    //private GlowObject backpackGlow;
    //private GlowObject dogGlow;
    //private GlowObject noteGlow;
    //private GlowObject gunGlow;

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
        bedroomDoor = GameObject.FindGameObjectWithTag("BedroomDoor");

        // Set up glow effects
        //if (journal != null) journalGlow = journal.GetComponent<GlowObject>();
        //if (backpack != null) backpackGlow = backpack.GetComponent<GlowObject>();
        //if (dog != null) dogGlow = dog.GetComponent<GlowObject>();
        //if (note != null) noteGlow = note.GetComponent<GlowObject>();
        //if (gun != null) gunGlow = gun.GetComponent<GlowObject>();
    }

    private void Update()
    {
        // You can add any extra logic for updating the glow effect here if necessary.
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
        if (other.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Interact"))
            {
                // Journal interaction
                if (journal != null && journal.activeInHierarchy && !GameManager.Instance.hasJournal)
                {
                    GameManager.Instance.hasJournal = true;
                    Destroy(bedroomDoor);
                    GameManager.Instance.interactWindow.SetActive(false);
                    Destroy(gameObject);
                    taskTracker.text = "Show Mom Game Design Journal.";
                    //DisableGlowObjects();
                    return;
                }

                // Backpack interaction
                if (backpack != null && backpack.activeInHierarchy)
                {
                    GameManager.Instance.hasBackpack = true;
                    GameObject momsDoor = GameObject.FindGameObjectWithTag("Moms Door");
                    Destroy(momsDoor);
                    Destroy(gameObject);
                    GameManager.Instance.interactWindow.SetActive(false);
                    taskTracker.text = "<s>" + taskTracker.text + "</s>\n";
                    taskTracker.text += "Go Pickup Sparky!";
                   // DisableGlowObjects();
                    return;
                }

                // Dog interaction
                if (dog != null && dog.activeInHierarchy)
                {
                    GameManager.Instance.hasDog = true;
                    GameObject frontDoor = GameObject.FindGameObjectWithTag("FrontDoor");
                    Destroy (frontDoor);
                    Destroy(gameObject);
                    GameManager.Instance.interactWindow.SetActive(false);
                    note.SetActive(true);
                    taskTracker.text = "<s>" + taskTracker.text + "</s>\n";
                    taskTracker.text += "Go Find Mom";
                  //  DisableGlowObjects();
                    return;
                }

                // Note interaction
                if (note != null)
                {
                    StartCoroutine(noteRead());
                  //  DisableGlowObjects();
                    return;
                }

                // Gun interaction
                if (gun != null && gun.activeInHierarchy && GameManager.Instance.hasNote)
                {
                    if (GameManager.Instance.canHaveGun)
                    {
                        GameManager.Instance.hasGun = true;
                        Destroy(gameObject);
                        GameManager.Instance.interactWindow.SetActive(false);
                        taskTracker.text = "RUNNNNNN!!!!";
                    }
                    else
                    {
                        StartCoroutine(gunNoteRead());
                    }
                    //DisableGlowObjects();
                    return;
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

    //private void EnableGlowObjects(GameObject objectToGlow)
    //{
    //    // Enable the glow effect on the next object to pick up
    //    if (objectToGlow != null)
    //    {
    //        GlowObject GlowObject = objectToGlow.GetComponent<GlowObject>();
    //        if (GlowObject != null)
    //        {
    //            GlowObject.EnableGlow();
    //        }
    //    }
    //}

    //private void DisableGlowObjects()
    //{
    //    // Disable glow effects on all objects
    //    if (journalGlow != null) journalGlow.DisableGlow();
    //    if (backpackGlow != null) backpackGlow.DisableGlow();
    //    if (dogGlow != null) dogGlow.DisableGlow();
    //    if (noteGlow != null) noteGlow.DisableGlow();
    //    if (gunGlow != null) gunGlow.DisableGlow();
    //}

    IEnumerator noteRead()
    {
        GameManager.Instance.hasNote = true;
        GameManager.Instance.interactWindow.SetActive(false);
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
