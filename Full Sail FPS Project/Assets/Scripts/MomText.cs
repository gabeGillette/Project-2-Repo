using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MomText : MonoBehaviour
{

    [Header("-----Text Object References-----")]
    [SerializeField] TextMeshProUGUI momsFirst;
    [SerializeField] TextMeshProUGUI momsSecond;
    [SerializeField][Range(0, 10)] float textTimer; //How long to display the text within the coroutine allowing for adjustments to how long on screen.

    [SerializeField] GameObject parentsDoor; //Object blocking way to backpack

    private TMP_Text taskTracker;


    // Start is called before the first frame update
    void Start()
    {
        momsFirst.gameObject.SetActive(false);
        momsSecond.gameObject.SetActive(false);
        taskTracker = GameManager.Instance.taskTrackerText;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(speech());
            taskTracker.text = "<s>" + taskTracker.text + "</s>\n";
            taskTracker.text += "Get the backpack from Mom's Room.";

            parentsDoor.SetActive(false);
        }
    }

    IEnumerator speech()
    {
        momsFirst.gameObject.SetActive(true);
        yield return new WaitForSeconds(textTimer);
        momsFirst.gameObject.SetActive(false);
        momsSecond.gameObject.SetActive(true);
        yield return new WaitForSeconds(textTimer);
        momsSecond.gameObject.SetActive(false);

    }
}
