using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

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

    //MeshAgent to allow the enemy to move along/avoid terrain
    [SerializeField] NavMeshAgent agent;
    //Allows the identification of the specific renderer model
    [SerializeField] Renderer model;
    //Where the shoot object should start from
  //  [SerializeField] Transform shootPos;
    //Where the enemy will base it's 'look' from and turn towards player
    [SerializeField] Transform headPos;
    //Where the melee should originate from
    [SerializeField] Transform meleePos;

    //How many Hit Points the enemy will have
    [SerializeField] int HP;
    //How quickly the enemy will turn towards the player
    [SerializeField] int faceTargetSpeed;

    //Which object and physics for the spit mechanic
    [SerializeField] GameObject spit;
    //Which object and physics for the melee mechanic
    [SerializeField] GameObject melee;

    //How fast the enemy can spit/shoot
    [SerializeField] float spitRate;
    //How fast teh enemy can melee attack
    [SerializeField] float meleeAttackRate;
    //Option to make any enemy ranged or just melee
    [SerializeField] bool rangedAttacker;
    //Range at which enemy will attempt a melee attack
    [SerializeField] float meleeAttackRange;

    ////Fields for Random Wandering Not implemented yet////
    //[SerializeField] bool canWander; //Check if you want the enemy to wander
    //[SerializeField] float wanderSpeed;
    //[SerializeField] float wanderRadius;
    //[SerializeField] float wanderTime;

    //private Vector3 wanderTargetPosition;
    //private float timer;


   // private GameObject player;

    Color colorOrig;

    bool playerInMovementRange; //See if the player is within the sphere collider to start moving towards player
   // bool canSpit;  //Only is true if player is within movement range, will turn true for all characters


   // bool isSpiting; //Check is enemy is ranged attacking
    bool isMeleeAttacking; //Check if enemy is melee attacking
    bool playerInSpitRange; //Check if player is in spitRange
    bool playerInMeleeRange; //Check if player is in meleeRange

    Vector3 playerDir;


    // Start is called before the first frame update
    void Start()
    {
        //Setting the original color so we can change it later to show damage
        colorOrig = model.material.color;

    }

    private void Update()
    {
        if (player != null)
        {
            LookAtPlayer();
            TrySpit();
        }

        else if (playerInMeleeRange)
        {
            if(!isMeleeAttacking)
            {
                StartCoroutine(meleeAttack());
            }
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
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        playerInMovementRange = false;
    //        canSpit = false;
    //    }
    //}

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

    //Process to move towards player and turn/face in the proper direction
    void moveTowardsPlayer()
    {
        if (playerInMovementRange)
        {
        //    player = GameObject.FindGameObjectWithTag("Player");
            playerDir = player.transform.position - headPos.position;



            //agent.SetDestination(player.transform.position);
            canSpit = true;
        }
     //   if (agent.remainingDistance <= agent.stoppingDistance)
        {
            faceTarget();
            canSpit = false;
        }
     //   if(agent.remainingDistance >= meleeAttackRange && playerInMovementRange)
        {
            playerInMeleeRange = true;

        }


    }

  

   


}
