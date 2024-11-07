using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunscript : MonoBehaviour
{
    // Reference to the Animator for the gun
    [SerializeField] Animator gunAnimator;

    // Reference to the playerController to access shooting state and fireRate
    [SerializeField] playerController player;

    void Update()
    {
        // Check if player is shooting and cooldown allows it
        if (player != null && player.isShooting)
        {
            // Trigger shooting animation
            TriggerShootAnimation();
        }
    }

    void TriggerShootAnimation()
    {
        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("ShootTrigger");
        }
    }
}
