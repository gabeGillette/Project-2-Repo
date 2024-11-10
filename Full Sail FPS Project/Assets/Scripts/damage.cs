using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    public enum damageType { initial, spit, melee, stationary, explosion}
   // [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int spitDamageAmount;
    [SerializeField] int meleeDamageAmount;
    [SerializeField] int explosionDamageAmount = 1;
    [SerializeField] int meleeSpeed;
    [SerializeField] int spitSpeed;
    [SerializeField] float destroyTime;

    [SerializeField] Renderer model; //Allows the identification of the specific renderer model
    Color colorOrig;

    private static damageType type;


    bool playerInExplosionArea = false;
    bool explode;

    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        Collider explosionCollider = GetComponent<Collider>();
        if (explosionCollider == null)
        {
            explosionCollider = gameObject.AddComponent<SphereCollider>();
            explosionCollider.isTrigger = true; // Make sure it's set as a trigger.
        }


    }

    // Update is called once per frame
    void Update()
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


    public void SetDamageType(damageType newType)
    {
        type = newType;
    }

    public damageType GetDamageType()
    {
        return type;
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
                dmg.TakeDamage(spitDamageAmount);
            }
            if (type == damageType.melee)
            {
                dmg.TakeDamage(meleeDamageAmount);
            }
            if(type == damageType.explosion)
            {
                if (other.CompareTag("Player"))
                {
                    playerInExplosionArea = true;  // Player enters the explosion area
                    StartCoroutine(explosion(other.GetComponent<IDamage>()));
                }

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

    private void OnTriggerExit(Collider other)
    {
        // Stop applying damage if the player exits the explosion area
        if (other.CompareTag("Player"))
        {
            playerInExplosionArea = false;
        }
    }


    IEnumerator explosion(IDamage dmg)
    {
        Collider explosionCollider = GetComponent<Collider>();


        // Initial flashing parameters
        float flashInterval = 0.2f; // Initial flash time interval
        float minInterval = 0.05f;  // Minimum interval, don't go faster than this
        int maxFlashes = 10;        // Maximum number of flashes

        for (int i = 0; i < maxFlashes; i++)
        {
            // Set the material color to red
            model.material.color = Color.red;

            // Wait for the current interval
            yield return new WaitForSeconds(flashInterval);

            // Reset the color to the original color
            model.material.color = colorOrig;

            // Wait for the current interval again
            yield return new WaitForSeconds(flashInterval);

            // Gradually reduce the time between flashes (increasing the speed)
            flashInterval = Mathf.Max(minInterval, flashInterval * 0.8f); // Reduces by 20% each time, but clamps at minInterval
        }
        

        if (playerInExplosionArea && dmg != null)
        {
            // Apply damage one more time after the flashing
            dmg.TakeDamage(explosionDamageAmount);
        }
        Destroy(gameObject);

    }


}
