using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;


public class playerController : MonoBehaviour, IDamage
{
    //character controller
    [SerializeField] CharacterController controller;
    //mask to ignore player
    [SerializeField] LayerMask ignoreMask;

    //variables that manipulate player--

    //speed the player goes
    [SerializeField] int speed;
    //multiplier for speed
    [SerializeField] int sprintMod;
    //max amount of jumps
    [SerializeField] int max_Amount_Of_Jumps;
    //how high we jump
    [SerializeField] int jumpHeight;
    //gravity to pull us down
    [SerializeField] int gravity;
    // health
    [SerializeField] int healthPoints;
    // amount of shooting damage dealt
    [SerializeField] int shootDmg;
    // rate of fire
    [SerializeField] float fireRate;
    // range of bullet
    [SerializeField] float fireRange;

    //current amount of jumps
    int jumpCount;
    //direction we're moving
    Vector3 moveDir;
    //our velocity
    Vector3 playerVel;
    // is the player currently shooting?
    private bool isShooting;
    // Player's initial health amount
    private int initHealth;

    // This is for the reticle color changing
    public Image rifleReticle;
    public Image unarmReticle;
    public Color defaultColor = Color.white;
    public Color enemyColor = Color.red;
    public Color friendColor = Color.green;
    public float maxRaycastDistance = 100f;

    // UI prompt to interact
    [SerializeField] GameObject interactPromptUI;
    // Max distance to interact
    [SerializeField] float interactDistance = 2f;

    // to track if an interactable object is within range
    private bool canInteract = false;
    // The interactable object in focus
    private GameObject currentInteractable = null;

    public int Health {  get { return healthPoints; } set { healthPoints = value; } }

    public bool Shooting { get { return isShooting; } }
    
    void Start()
    {
        initHealth = healthPoints;

        // update the health bar
        GameManager.instance.updatePlayerHealth(healthPoints, initHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // Draw ray for debugging
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * fireRange, Color.red);

        //constantly check how we're moving
        movement();
        sprint();

        // update reticle
        UpdateReticle();

        // Attempt to interact if the interact button is presses
        if (canInteract && Input.GetButtonDown("Interact"))
        {
            Interact();
        }
    }

    void movement()
    {
        //if player is grounded
        if (controller.isGrounded)
        {
            //set players velocity to -1f because they're on the ground and to help the model fully ground it's self
            playerVel.y = -1f;
            jumpCount = 0;

            //handle jump
            jump();
        }

        //get players vertical and horizontal movement-- vertical is .forward, horizontal is .right
        moveDir = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));

        // we use the moveDir variable we just set along with speed and delta time to move the character around
        controller.Move(moveDir * speed * Time.deltaTime);

        //we constantly apply gravity to player velocity
        playerVel.y -= gravity * Time.deltaTime;

        //we move the player in the air based on velocity
        controller.Move(playerVel * Time.deltaTime);

        // If the "shoot" button is trigged and the player can shoot, then shoot.
        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(Shoot());
        }
    }



    //how we handle jump
    void jump()
    {
        //check if player is jumping
        if (Input.GetButtonDown("Jump") && jumpCount < max_Amount_Of_Jumps)
        {
            //set player velocity based on jump height
            playerVel.y = jumpHeight;
            jumpCount++;
        }

    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            //multiply speed by modifier
            speed *= sprintMod;

        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }



    }

    public void TakeDamage(int amount)
    {
        // take damage
        healthPoints -= amount;
        StartCoroutine(flashDamage());
        // update the health bar
        GameManager.instance.updatePlayerHealth(healthPoints, initHealth);

        // death event
        if (healthPoints <= 0)
        {
            //GameManager.instance.youLose();
            GameManager.instance.RespawnPlayer();
        }
    }

    IEnumerator Shoot()
    {
        // so the player can shoot only once at a time.
        isShooting = true;

        // Raycast test for shooting.
        // shooting from the camera.
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position,
          Camera.main.transform.forward, out hit, fireRange, ~ignoreMask))
        {

            // log the name of the object hit.
            Debug.Log(hit.collider.name);

            // run the damage script of the object hit if there is one.
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(shootDmg);
            }
        }

        // pause for shootRate seconds
        yield return new WaitForSeconds(fireRate);

        isShooting = false;
    }

    // turn reticle red upon aiming at player
    void UpdateReticle()
    {
        if (rifleReticle != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRaycastDistance))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    rifleReticle.color = enemyColor; // reticle changes color (red)
                }
                else
                {
                    rifleReticle.color = defaultColor;
                }
            }
            else
            {
                rifleReticle.color = defaultColor;
            }

            if (Physics.Raycast(ray,out hit, maxRaycastDistance))
            {
                if (hit.collider.CompareTag("Friend"))
                {
                    rifleReticle.color = friendColor; // reticle changes color (green)
                }
                else
                {
                    rifleReticle.color = defaultColor;
                }
            }
        }
    }

    void Interact()
    {
        if (currentInteractable != null)
        {
            Debug.Log("Interacting with " +  currentInteractable.name);

            var item = currentInteractable.GetComponent<IInteractable>();
            if (item != null)
            {
                item.OnInteract();
            }
        }
    }

    IEnumerator flashDamage()
    {
        GameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageScreen.SetActive(false);
    }

    IEnumerator flashPoison()
    {
        GameManager.instance.playerPoisonScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameManager.instance.playerPoisonScreen.SetActive(false);
    }
}
