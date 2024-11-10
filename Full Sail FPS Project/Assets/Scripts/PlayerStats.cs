using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public float health = 100f;
    public float speed = 5f;
    public float damage = 10f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void IncreaseHealth(float amount)
    {
        health += amount;
        Debug.Log("Health increased by: " + amount);
    }

    public IEnumerator ApplySpeedBoost(float duration)
    {
        float originalSpeed = speed;
        speed *= 2;
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        Debug.Log("Speed boost ended.");
    }

    public IEnumerator ApplyDamageBoost(float duration)
    {
        float originalDamage = damage;
        damage *= 2;
        yield return new WaitForSeconds(duration);
        damage = originalDamage;
        Debug.Log("Damage boost ended.");
    }
}
