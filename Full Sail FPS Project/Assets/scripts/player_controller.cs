using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;


    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;




    Vector3 moveDir;
    Vector3 playerVel;


    bool isSprinting;
    bool isShooting;


    int jumpCount;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        movement();
        sprint();


    }

    void movement()
    {
        // Check if the character is on the ground
        if (controller.isGrounded)
        {
            // Reset vertical velocity when grounded
            playerVel.y = 0;

            // Handle jumping and horizontal movement
            jump();
        }

        // Get input for movement
        moveDir = (transform.forward * Input.GetAxis("Vertical")) +
                   (transform.right * Input.GetAxis("Horizontal"));

        // Apply horizontal movement
        controller.Move(moveDir * speed * Time.deltaTime);

        // Apply gravity
        playerVel.y -= gravity * Time.deltaTime;

    }




    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;

        }

    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreMask))
        {
            Debug.Log(hit.collider.name);
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }

        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
