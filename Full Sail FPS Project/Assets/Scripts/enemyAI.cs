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


    private bool isSpiting;
    private bool canSpit = true;
    private float lastSpitTime;

    public damage.damageType currentDamageType = damage.damageType.melee;


    Color colorOrig; //Placeholder to allow for color changes during damage

    bool playerInMovementRange; //See if the player is within the sphere collider to start moving towards player


    // bool isSpiting; //Check is enemy is ranged attacking
    bool isMeleeAttacking; //Check if enemy is melee attacking
    bool playerInSpitRange; //Check if player is in spitRange
    bool playerInMeleeRange; //Check if player is in meleeRange

    Vector3 playerDir;


    // Start is called before the first frame update
    void Start()
    {
        
        colorOrig = model.material.color; //Setting the original color so we can change it later to show damage


    }

    private void Update()
    {
        if (playerInMovementRange)
        {
            moveTowardsPlayer();
            
            //LookAtPlayer();
            //TrySpit();

            if (!isSpiting)
            {
                StartCoroutine(Shoot());
            }
        }

        //else if (playerInMeleeRange)
        //{
        //    if (!isMeleeAttacking)
        //    {
        //        StartCoroutine(meleeAttack());
        //    }
        //}




    }


     public damage.damageType GetDamageType() { return currentDamageType; }
     public void SetDamageType(damage.damageType newDamageType) { currentDamageType = newDamageType; }

    private void LookAtPlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);

        //Vector3 directionToPlayer = GameManager.instance.player.transform.position - transform.position;
        //directionToPlayer.y = 0; // Keep the y-axis unaffected (keep enemy upright)
        //Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    // Coroutine to handle the spit action
    private IEnumerator Shoot()
    {
        isSpiting = true;
        SetDamageType(damage.damageType.spit);
        Debug.Log("Spitting at player.");

        // Instantiate the spit prefab at the shoot position with the correct rotation
        Instantiate(spitPrefab, shootPos.position, transform.rotation);
        //Debug.Log($"Instantiating spit at position: {shootPos.position}");

        //// Check if the spit has a Rigidbody for movement
        //Rigidbody spitRb = spitInstance.GetComponent<Rigidbody>();
        //if (spitRb != null)
        //{
        //    spitRb.velocity = transform.forward * 10f; // Adjust speed here
        //    Debug.Log("Spit moving towards the player.");
        //}

        // Wait for the specified rate before allowing another spit
        yield return new WaitForSeconds(attackRate);
        isSpiting = false;
        //canSpit = true;
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
        HP -= amount;
        StartCoroutine(damageFlash());
        agent.SetDestination(GameManager.instance.Player.transform.position);
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

    //Process to start spitting/shooting
    //IEnumerator shoot()
    //{

    //    isSpiting = true;

    //    Instantiate(spitPrefab, shootPos.position, transform.rotation);

    //    yield return new WaitForSeconds(spitRate);
    //    isSpiting = false;
    //}

    //Allows enemy to do a melee attack if within range
    //IEnumerator meleeAttack()
    //{
    //    isMeleeAttacking = true;

    //    Instantiate(melee, shootPos.position, transform.rotation);

    //    yield return new WaitForSeconds(meleeAttackRate);
    //    isMeleeAttacking = false;
    //}

    //Makes the enemy always look towards the player smoothly
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
            playerDir = GameManager.instance.Player.transform.position - headPos.position;

            agent.SetDestination(GameManager.instance.Player.transform.position);

            //TrySpit();
           
        }
        //   if (agent.remainingDistance <= agent.stoppingDistance)
        //{
        //    faceTarget();
        //    canSpit = false;
        //}
        //   if(agent.remainingDistance >= meleeAttackRange && playerInMovementRange)
        //{
        //    playerInMeleeRange = true;

        //}


    }

  






}
