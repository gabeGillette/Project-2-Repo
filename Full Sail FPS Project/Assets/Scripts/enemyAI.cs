using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamage
{
    [Header("Collision Detection and Models")]
    [SerializeField] GameObject spitPrefab; // The spit prefab
    [SerializeField] Transform shootPos; // Where the spit should be fired from
    [SerializeField] NavMeshAgent agent; //MeshAgent to allow the enemy to move along/avoid terrain
    [SerializeField] Renderer model; //Allows the identification of the specific renderer model
    [SerializeField] Transform headPos; //Where the enemy will base it's 'look' from and turn towards player
    [SerializeField] Transform meleePos; //Where the melee should originate from
    [SerializeField] GameObject melee;


    [Header("Character Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float attackRate; // Time between spits
    [SerializeField] float attackRange; // Range to spit at player
    [SerializeField] int HP; //How many Hit Points the 'Character' has
    [SerializeField] bool rangedAttacker; //Check if ranged as well as melee attacker
    [SerializeField] bool stationaryAttacker; //Shows if it is a mine or stationary attack


    private bool isSpiting;
    private bool canSpit;
    private float lastSpitTime;
    private bool canExplode;

    private damage.damageType currentDamageType;
 //   private damage damageScript;


    Color colorOrig; //Placeholder to allow for color changes during damage

    bool playerInMovementRange; //See if the player is within the sphere collider to start moving towards player


    bool isMeleeAttacking; //Check if enemy is melee attacking
    bool playerInSpitRange; //Check if player is in spitRange
    bool playerInMeleeRange; //Check if player is in meleeRange

    Vector3 playerDir;


    // Start is called before the first frame update
    void Awake()
    {
      //  damageScript = GetComponent<damage>();
        colorOrig = model.material.color; //Setting the original color so we can change it later to show damage
        if (rangedAttacker)
        {
            canSpit = true;
        }

    }

    private void Update()
    {

          
       if (rangedAttacker)
       {
           moveTowardsPlayer();

            if (!isSpiting && canSpit)
           {

                StartCoroutine(Shoot());
           }
       }
       


    }

   

    private void LookAtPlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }

    // Coroutine to handle the spit action
    private IEnumerator Shoot()
    {
        isSpiting = true;
        Debug.Log("Spitting at player.");

        // Instantiate the spit prefab at the shoot position with the correct rotation
        GameObject spit = Instantiate(spitPrefab, shootPos.position, transform.rotation);
        damage damageScript = spit.GetComponent<damage>();
        damageScript.SetDamageType(damage.damageType.spit);


        yield return new WaitForSeconds(attackRate);
        isSpiting = false;
        //canSpit = true;
    }

    // This is the trigger detection for the spit hitting something
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (stationaryAttacker)
            {
                damage damageScript = this.GetComponent<damage>();
                damageScript.SetDamageType(damage.damageType.explosion);
               // StartCoroutine(explosionEvent());

            }
            else
            {
                playerInMovementRange = true;
            }
            
           
        }
    }
   

    // When the player is in range and can be attacked, the spit can be fired
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isSpiting)
        {
            if (Time.time - lastSpitTime >= attackRate)
            {
                canSpit = true;
            }
        }
    }

    // If the enemy stops seeing the player, reset
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSpit = false;
        }
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(damageFlash());
        agent.SetDestination(GameManager.Instance.Player.transform.position);
        if (HP <= 0)
        {
            Destroy(gameObject);
        }

    }

    //A way to demo the enemy took damage. Change the Color.red to other colors if necessary
    IEnumerator damageFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = colorOrig;
    }

    
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }

    //Process to move towards player and turn/face in the proper direction
    void moveTowardsPlayer()
    {
        if (playerInMovementRange)
        {
            //    player = GameObject.FindGameObjectWithTag("Player");

            faceTarget();
            playerDir = GameManager.Instance.Player.transform.position - headPos.position;

            agent.SetDestination(GameManager.Instance.Player.transform.position);


           
        }
    }

    IEnumerator explosionEvent()
    {
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
        
    }


}
