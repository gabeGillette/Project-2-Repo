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
           // gun.ammoCur = gun.ammoMax;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlayerScript.getGunStats(gun);
            Destroy(gameObject);
        }
    }
}
