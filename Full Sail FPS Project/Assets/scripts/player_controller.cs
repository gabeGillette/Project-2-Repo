using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static damage;


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
   // [SerializeField] int shootDmg;
    // rate of fire
    //[SerializeField] float fireRate;
    // range of bullet
    //[SerializeField] float fireRange;

    [SerializeField] GameObject gunViewModel;
    GameObject currentViewModel;
    Animator viewModelAnimator;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();

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

    int selectedGun;

    public static damage.damageType enemyDamageType;



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
        GameManager.Instance.updateHealthDisplay(healthPoints, initHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // Draw ray for debugging
        if (gunList.Count > 0)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * gunList[selectedGun].Range, Color.red);
        }

        //constantly check how we're moving
        movement();
        sprint();

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

        if (gunList.Count > 0)
        {
            Gunselect();
            switch (gunList[selectedGun].WeaponType)
            {
                case gunStats.WEAPON_TYPE.FLASHLIGHT:
                    if (Input.GetButtonDown("Fire1") && !isShooting)
                    {
                        StartCoroutine(Shoot());
                    }
                    break;
                case gunStats.WEAPON_TYPE.HITSCAN:
                    if (Input.GetButton("Fire1") && !isShooting)
                    {
                        StartCoroutine(Shoot());
                    }
                    break;
            }
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
        //If poison Damage
        if (enemyDamageType == damage.damageType.spit)
        {
            StartCoroutine(flashPoison());
           // GameManager.instance.FadeOutPoisonScreen(1.5f);

        }
        else
        {
            StartCoroutine(flashDamage());
        }
        // update the health bar
        GameManager.Instance.updateHealthDisplay(healthPoints, initHealth);

        // death event
        if (healthPoints <= 0)
        {
            //GameManager.instance.youLose();
            GameManager.Instance.RespawnPlayer();
        }
    }

    IEnumerator Shoot()
    {
        // so the player can shoot only once at a time.
        isShooting = true;

        switch (gunList[selectedGun].WeaponType)
        {
            case gunStats.WEAPON_TYPE.HITSCAN:
                viewModelAnimator.SetTrigger("fire");
                // Raycast test for shooting.
                // shooting from the camera.
                RaycastHit hit;
                    if (Physics.Raycast(Camera.main.transform.position,
                      Camera.main.transform.forward, out hit, gunList[selectedGun].Range, ~ignoreMask))
                    {

                        // log the name of the object hit.
                        Debug.Log(hit.collider.name);

                        // run the damage script of the object hit if there is one.
                        IDamage dmg = hit.collider.GetComponent<IDamage>();
                        if (dmg != null)
                        {
                            dmg.TakeDamage(gunList[selectedGun].Damage);
                        }
                    }

                    
                break;
            case gunStats.WEAPON_TYPE.FLASHLIGHT:
                viewModelAnimator.SetTrigger("fire");
                break;
        }
        // pause for shootRate seconds
        yield return new WaitForSeconds(gunList[selectedGun].FireRate);
        isShooting = false;
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
        GameManager.Instance.PlayerDamagePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.PlayerDamagePanel.SetActive(false);
    }

    IEnumerator flashPoison()
    {
        GameManager.Instance.PlayerPoisonPanel.SetActive(true);

        // Get the CanvasGroup component (add one if it doesn't exist on the panel)
        CanvasGroup canvasGroup = GameManager.Instance.PlayerPoisonPanel.GetComponent<CanvasGroup>();

        // If no CanvasGroup, add one (you can also add this manually in the Inspector)
        if (canvasGroup == null)
        {
            canvasGroup = GameManager.Instance.PlayerPoisonPanel.AddComponent<CanvasGroup>();
        }

       
        float fadeDuration = 0.2f; // Duration of fade
        float fadeSpeed = 1f / fadeDuration;
       
        canvasGroup.alpha = 0.8f; // Ensure fully visible after fade-in

        // Wait for a while with the panel visible
        yield return new WaitForSeconds(1f);

        // Fade out the panel
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f; // Ensure fully transparent after fade-out

        // Disable the panel after fading out
        GameManager.Instance.PlayerPoisonPanel.SetActive(false);



        
    }
   
    public void addGun(gunStats gun)
    {
        /*gunList.Add(gun);
        GunSelect = gunList.Count - 1;
        shootDmg = gun.shootDmg;
        fireRange = gun.fireRange;
        fireRate = gun.fireRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;*/

        
        gunList.Add(gun);
        selectedGun = gunList.Count - 1;
        changeGun();
    }

    void Gunselect()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();

        } else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }

    void changeGun()
    {
        /*shootDmg = gunList[GunSelect].shootDmg;
        fireRange = gunList[GunSelect].fireRange;
        fireRate = gunList[GunSelect].fireRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[GunSelect].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[GunSelect].gunModel.GetComponent<MeshRenderer>().sharedMaterial;*/
        /*for (int childIndex = 0; childIndex < gunViewModel.transform.childCount; childIndex++)
        {
            Destroy(gunViewModel.transform.GetChild(childIndex).gameObject);
        }*/

        if (currentViewModel != null)
        {
            Destroy(currentViewModel);
        }

        currentViewModel = Instantiate(gunList[selectedGun].ViewModel);

        currentViewModel.transform.SetParent(gunViewModel.transform, false);
        viewModelAnimator = currentViewModel.GetComponent<Animator>();

    }

}
