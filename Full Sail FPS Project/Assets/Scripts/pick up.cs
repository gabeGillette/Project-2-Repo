// pick up.cs
// Desc: pickups
// Authors: Jacquell Frazier, Gabriel Gillette
// Last Modified: Nov, 16 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    enum pickupType { gun, HP, stamina }
    [SerializeField] pickupType type;
    [SerializeField] gunStats gun;

    // Start is called before the first frame update
    void Start()
    {
        if(type == pickupType.gun)
        {
            gun._ammoCur = gun._ammoMax;
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gun.RefillAmmo();
            GameManager.Instance.PlayerScript.addGun(gun);
           // GameObject.FindGameObjectWithTag("Main Gun").SetActive(true);
            Destroy(gameObject);
        }
    }
}
