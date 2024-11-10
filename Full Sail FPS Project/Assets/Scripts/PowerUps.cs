using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : InteractableBase
{
    public enum PowerUpType { HealthBoost, SpeedBoost, DamageBoost }
    public PowerUpType powerUpType;
    public float powerUpValue = 20f;
    public float duration = 5f;

    protected override void Activate()
    {
        switch (powerUpType)
        {
            case PowerUpType.HealthBoost:
                PlayerStats.instance.IncreaseHealth(powerUpValue);
                break;
            case PowerUpType.SpeedBoost:
                PlayerStats.instance.StartCoroutine(PlayerStats.instance.ApplySpeedBoost(duration));
                break;
            case PowerUpType.DamageBoost:
                PlayerStats.instance.StartCoroutine(PlayerStats.instance.ApplyDamageBoost(duration));
                break;
        }
        Debug.Log($"{interactableName} activated: {powerUpType}");
    }
}
