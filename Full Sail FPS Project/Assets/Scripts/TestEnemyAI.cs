using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] int health;

    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    [SerializeField] NavMeshAgent agent;

    bool isShooting;

    [SerializeField] Renderer model;
    Color colorOrig;

    [SerializeField] int rotationSpeed;
    Vector3 playerDir;

    [SerializeField] int viewAngle;

    bool playerInRange;

    float angleToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        //GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        //
        if (playerInRange && canSeePlayer())
        {

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    bool canSeePlayer()
    {
        // I had to inverse the players y to get this to work, no idea why.
        playerDir = new Vector3(GameManager.instance.Player.transform.position.x, -GameManager.instance.Player.transform.position.y, GameManager.instance.Player.transform.position.z) - transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        //Debug.DrawLine(headPos.position, GameManager.instance.player.transform.position);
        Debug.DrawRay(headPos.position, playerDir, Color.yellow);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.destination = GameManager.instance.Player.transform.position;

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }


                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }

                return true;
            }
        }

        return false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        agent.destination = GameManager.instance.Player.transform.position;
        StartCoroutine(flashColor());
        if (health <= 0)
        {
            //GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}

