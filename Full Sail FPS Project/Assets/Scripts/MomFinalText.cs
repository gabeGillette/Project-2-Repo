using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MomFinalText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI momsFinal;
    [SerializeField][Range(0, 10)] float textTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(speech());

        }
    }

    IEnumerator speech()
    {
        momsFinal.gameObject.SetActive(true);
        yield return new WaitForSeconds(textTimer);
        momsFinal.gameObject.SetActive(false);
        

    }
}
