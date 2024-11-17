using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MomText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI momsFirst;
    [SerializeField] TextMeshProUGUI momsSecond;
    [SerializeField][Range(0, 10)] float textTimer;

    [SerializeField] GameObject parentsDoor;

    // Start is called before the first frame update
    void Start()
    {
        momsFirst.gameObject.SetActive(false);
        momsSecond.gameObject.SetActive(false);
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
