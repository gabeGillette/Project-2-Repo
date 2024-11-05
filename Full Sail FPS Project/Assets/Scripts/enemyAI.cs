using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Transform meleePos;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] GameObject spit;
    [SerializeField] GameObject melee;

    [SerializeField] float spitRate;
    [SerializeField] float meleeAttackRate;
    [SerializeField] bool rangedAttacker;

    [SerializeField] float meleeAttackRange;

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
    }

    // Update is called once per frame
    void Update()
    {

        //Move enemy Towards player
        moveTowardsPlayer();

        //Move enemy towards player and if ranged attacker start ranged attack
        if (canSpit && rangedAttacker)
        {
            if (!isSpiting)
            {
                StartCoroutine(shoot());
            }
        }

        else if (playerInMeleeRange)
        {
            if(!isMeleeAttacking)
            {
                StartCoroutine(meleeAttack());
            }
        }
            //else if (agent.remainingDistance <= meleeAttackRange)
            //{
            //    canSpit = false;
            //    playerInMeleeRange = true;
            //    StartCoroutine(meleeAttack());
            //}
            //else if (agent.remainingDistance >= agent.stoppingDistance)
            //{
            //    playerInMeleeRange = false;
            //    canSpit = true;
            //}

            //If within melee range stops spit attack but allows melee attack
           

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInMovementRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInMovementRange = false;
            canSpit = false;
        }
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(damageFlash());

        //If the enemy takes damage outside of normal agro range head towards the player's last attack position
        agent.SetDestination(GameManager.instance.GetPlayer().transform.position);

        //Destroy the object
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
