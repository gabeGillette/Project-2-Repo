using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractWithObject : MonoBehaviour
{
    private GameObject journal;
    private GameObject backpack;
    private GameObject dog;
    private GameObject note;

    [SerializeField] TextMeshProUGUI momsNote;


    private void Start()
    {
        journal = GameManager.Instance._journal;
        dog = GameManager.Instance._dog;
        backpack = GameManager.Instance._backpack;
        note = GameManager.Instance._note;

        
    }

    private void Update()
    {
        if (GameManager.Instance.hasJournal && GameManager.Instance.hasBackpack && GameManager.Instance.hasDog)
        {
            note.SetActive(true);
        }



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
                    Destroy(gameObject); // Destroy the object after interaction
                    GameManager.Instance.interactWindow.SetActive(false);
                }

                if (backpack != null && backpack.activeInHierarchy)
                {
                    GameManager.Instance.hasBackpack = true;
                    Destroy(gameObject);
                    GameManager.Instance.interactWindow.SetActive(false);
                }

                if (dog != null && dog.activeInHierarchy)
                {
                    GameManager.Instance.hasDog = true;
                    Destroy(gameObject);
                    GameManager.Instance.interactWindow.SetActive(false);
                }
                //if(note)
                //{
                //    StartCoroutine(noteRead());
                //    Destroy(gameObject);
                //}



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
        momsNote.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        momsNote.gameObject.SetActive(false);
    }
}
