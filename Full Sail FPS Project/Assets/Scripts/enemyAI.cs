using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamage
{
    [Header("Movement and Attack")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float attackRate = 2f; // Time between spits
    [SerializeField] float attackRange = 10f; // Range to spit at player
    [SerializeField] GameObject spitPrefab; // The spit prefab
    [SerializeField] Transform shootPos; // Where the spit should be fired from
    [SerializeField] Transform player; // The player transform
    [SerializeField] LayerMask ignoreMask; // Layer mask to ignore during attacks

    private bool isSpiting = false;
    private bool canSpit = true;
    private float lastSpitTime;

    [SerializeField] GameObject spit;
    [SerializeField] GameObject melee;

    [SerializeField] float spitRate;
    [SerializeField] float meleeAttackRate;
    [SerializeField] bool rangedAttacker;

    [SerializeField] float meleeAttackRange;

    //Fields for Random Wandering
    [SerializeField] bool canWander; //Check if you want the enemy to wander
    [SerializeField] float wanderSpeed;
    [SerializeField] float wanderRadius;
    [SerializeField] float wanderTime;

    private Vector3 wanderTargetPosition;
    private float timer;


    private GameObject player;

    Color colorOrig;

    bool playerInMovementRange; //See if the player is within the sphere collider to start moving towards player
    bool canSpit;  //Only is true if player is within movement range, will turn true for all characters


    bool isSpiting;
    bool isMeleeAttacking;
    bool playerInSpitRange;
    bool playerInMeleeRange;

    Vector3 playerDir;


    // Start is called before the first frame update
    void Start()
    {
        //Setting the original color so we can change it later to show damage
        colorOrig = model.material.color;

        //Start Wandering
    }

    private void Update()
    {
        if (player != null)
        {
            LookAtPlayer();
            TrySpit();
        }
    }

    // This method checks if the enemy can spit and if it's time to do so
    private void TrySpit()
    {
        if (canSpit && !isSpiting && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Ready to spit! Starting shoot coroutine...");
            StartCoroutine(Shoot());
        }
    }

    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // Keep the y-axis unaffected (keep enemy upright)
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    // Coroutine to handle the spit action
    private IEnumerator Shoot()
    {
        isSpiting = true;
        Debug.Log("Spitting at player.");

        // Instantiate the spit prefab at the shoot position with the correct rotation
        GameObject spitInstance = Instantiate(spitPrefab, shootPos.position, transform.rotation);
        Debug.Log($"Instantiating spit at position: {shootPos.position}");

        // Check if the spit has a Rigidbody for movement
        Rigidbody spitRb = spitInstance.GetComponent<Rigidbody>();
        if (spitRb != null)
        {
            spitRb.velocity = transform.forward * 10f; // Adjust speed here
            Debug.Log("Spit moving towards the player.");
        }

        // Wait for the specified rate before allowing another spit
        yield return new WaitForSeconds(attackRate);
        isSpiting = false;
        canSpit = true;
    }

    // This is the trigger detection for the spit hitting something
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInMovementRange = true;
            canWander = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInMovementRange = false;
            canSpit = false;
            canWander = true;
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

    public void takeDamage(int amount)
    {
        throw new System.NotImplementedException();
    }

    //A way to demo the enemy took damage. Change the Color.red to other colors if necessary
    IEnumerator damageFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = colorOrig;
    }

    //Process to start spitting/shooting
    IEnumerator shoot()
    {
        
        isSpiting = true;

        Instantiate(spit, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(spitRate);
        isSpiting = false;
    }

    //Allows enemy to do a melee attack if within range
    IEnumerator meleeAttack()
    {
        isMeleeAttacking = true;

        Instantiate(melee, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(meleeAttackRate);
        isMeleeAttacking = false;
    }

    //Makes the enemy always look towards the player smoothly
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    void moveTowardsPlayer()
    {
        if (playerInMovementRange)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerDir = player.transform.position - headPos.position;



            agent.SetDestination(player.transform.position);
            canSpit = true;
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            faceTarget();
            canSpit = false;
        }
        if(agent.remainingDistance >= meleeAttackRange && playerInMovementRange)
        {
            playerInMeleeRange = true;

        }


    }

  

   


}
