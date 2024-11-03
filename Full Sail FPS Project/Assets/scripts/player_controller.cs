using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
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
    //how fast we jump
    [SerializeField] int jumpHeight;
    //gravity to pull us down
    [SerializeField] int gravity;
    //current amount of jumps
    int jumpCount;
    //direction we're moving
    Vector3 moveDir;
    //our velocity
    Vector3 playerVel;
    
    void Start()
    {
        jumpCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //constantly check how we're moving
        movement();
        sprint();


    }

    void movement()
    {
        //if player is grounded
        if (controller.isGrounded)
        {
            //set players velocity to 0 because they're on the ground
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


    }



    //how we handle jump
    void jump()
    {
        //check if player is jumping
        if (Input.GetButtonDown("Jump")&& jumpCount<max_Amount_Of_Jumps)
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
}
