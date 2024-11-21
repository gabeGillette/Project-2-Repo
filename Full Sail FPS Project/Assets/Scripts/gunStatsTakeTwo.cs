using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStatsTakeTwo : MonoBehaviour
{
    public GameObject gunModel;
    [Range(1, 10)] public int shootDamage;
    [Range(15, 1000)] public int shootDistance;
    [Range(0.1f, 1)] public float shootRate;
    [Range(5, 30)] public int ammoCur, ammoMax;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootVol;

}
