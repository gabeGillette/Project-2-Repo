// gunStats
// Desc: gun stats scriptable
// Authors: Jacquell Frazier, Gabriel Gillette
// Last Modified: Nov, 16 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public enum WEAPON_TYPE
    {
        HITSCAN,
        MELEE,   
        PROJECTILE,
        FLASHLIGHT
    }

    
    [SerializeField] WEAPON_TYPE type;

    [Header(" ------ Graphics ------ ")]

    [SerializeField] GameObject _viewModel;
    [SerializeField] ParticleSystem _hitEffect;

    [Header(" ------ Projectile Attributes ------ ")]

    [SerializeField] GameObject _projectile;

    [Header(" ------ Flashlight Attributes ------ ")]

    [SerializeField] bool _turnedOn;

    [Header(" ------ General Attributes ------ ")]

    [SerializeField] [Range(0, 500)] int _damage;
    [SerializeField] [Range(0, 1000)] float _range;
    [SerializeField] [Range(0, 120)] float _fireRate;
    [SerializeField] [Range(0, 32)] int _raysPerFire;
    [SerializeField] [Range(0, 5)] float _spreadAmt;
    [SerializeField] [Range(0, 1024)] int _ammoMax;
    [SerializeField] [Range(0, 1024)] int _ammoCur;

    [Header(" ------ Sound ------ ")]

    [SerializeField] AudioClip[] _shootSound;
    [SerializeField] AudioClip[] _hitSound;
    [SerializeField] [Range (0, 20)] float _shootVol;
    [SerializeField] [Range (0, 20)] float _hitVol;
}
