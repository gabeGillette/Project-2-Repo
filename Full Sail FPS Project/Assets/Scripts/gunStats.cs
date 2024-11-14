using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public GameObject gunModel;
    public int shootDmg;
    public int fireRange;
    public float fireRate;
    public int ammoCur, ammoMax;


    public ParticleSystem hiteffect;

    public AudioClip[] shootSound;
    public float ShootVol;
}
