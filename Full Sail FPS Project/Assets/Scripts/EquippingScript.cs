using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippingScript : MonoBehaviour
{
    public GameObject Slot1;
    public GameObject Slot2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Eqiup1();
        }

        if (Input.GetKeyDown("2"))
        {
            Eqiup2();
        }
    }

    void Eqiup1()
    {
        Slot1.SetActive(true);
        Slot2.SetActive(false);
    }

    void Eqiup2()
    {
        Slot1.SetActive(false);
        Slot2.SetActive(true);
    }
}
