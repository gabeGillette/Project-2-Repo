using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    enum damageType { spit, melee, stationary}
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int spitDamageAmount;
    [SerializeField] int meleeDamageAmount;
    [SerializeField] int meleeSpeed;
    [SerializeField] int spitSpeed;
    [SerializeField] float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        if (type == damageType.spit)
        {
            rb.velocity = transform.forward * spitSpeed;
            Destroy(gameObject, destroyTime);
        }
        if (type == damageType.melee)
        {
            rb.velocity = transform.forward * meleeSpeed;
            Destroy(gameObject, destroyTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if(dmg != null)
        {
            if (type == damageType.spit)
            {
                dmg.takeDamage(spitDamageAmount);
            }
            if (type == damageType.melee)
            {
                dmg.takeDamage(meleeDamageAmount);
            }
        }

        if(type == damageType.spit)
        {
            Destroy(gameObject);
        }
        if(type == damageType.melee)
        {
            Destroy(gameObject);
        }
    }
   
}
