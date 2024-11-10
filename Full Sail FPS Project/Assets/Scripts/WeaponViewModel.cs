using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponViewModel : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float fireRate = 0.1f;  // Time between shots for rapid firing
    public float damage = 10f;      // Damage per shot
    public Transform muzzlePoint;   // Where bullets originate from

    [Header("Effects")]
    public ParticleSystem muzzleFlash; // Muzzle flash effect
    public AudioClip fireSound;       // Gunfire sound

    private float nextFireTime = 0f;
    private AudioSource audioSource;

    [Header("Recoil Settings")]
    public float recoilAmount = 0.01f;  // Small backward movement
    public float recoilSpeed = 10f;     // Quick return to original position

    private Vector3 originalPosition;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        originalPosition = transform.localPosition; // Store initial position
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            FireWeapon();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void FireWeapon()
    {
        // Show muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Play firing sound
        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        // Implement recoil effect
        transform.localPosition -= new Vector3(0, 0, recoilAmount);
        StartCoroutine(ResetRecoil());

        // Implement firing logic (e.g., raycast to detect hits)
        RaycastHit hit;
        if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out hit, 100f))
        {
            Debug.Log("Hit " + hit.collider.name);
            // Apply damage if hit object has health component (optional)
        }
    }

    private IEnumerator ResetRecoil()
    {
        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * recoilSpeed);
            yield return null;
        }
        transform.localPosition = originalPosition;
    }
}
