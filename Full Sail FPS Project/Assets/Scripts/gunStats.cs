// gunStats
// Desc: gun stats scriptable
// Last Modified: Nov, 16 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class gunStats : ScriptableObject
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
//[CreateAssetMenu]

//public class gunStats : ScriptableObject
//{
//    public enum WEAPON_TYPE
//    {
//        HITSCAN,
//        MELEE,
//        PROJECTILE,
//        FLASHLIGHT
//    }


//    [SerializeField] WEAPON_TYPE _type;

//    [Header(" ------ Graphics ------ ")]

//    public GameObject gunModel;


//    [SerializeField] GameObject _viewModel;
//    [SerializeField] ParticleSystem _hitEffect;

//    [Header(" ------ Projectile Attributes ------ ")]

//    [SerializeField] GameObject _projectile;

//    [Header(" ------ Flashlight Attributes ------ ")]

//    [SerializeField] bool _turnedOn;

//    [Header(" ------ General Attributes ------ ")]

//    [SerializeField][Range(0, 500)] public int _damage;
//    [SerializeField][Range(0, 1000)] public float _range;
//    [SerializeField][Range(0, 120)] public float _fireRate;
//    [SerializeField][Range(0, 32)] public int _raysPerFire;
//    [SerializeField][Range(0, 5)] public float _spreadAmt;
//    [SerializeField][Range(0, 1024)] public int _ammoMax;
//    [SerializeField][Range(0, 1024)] public int _ammoCur;

//    [Header(" ------ Sound ------ ")]

//    [SerializeField] AudioClip[] _shootSounds;
//    [SerializeField] AudioClip[] _hitSounds;
//    [SerializeField][Range(0, 20)] float _shootVol;
//    [SerializeField][Range(0, 20)] float _hitVol;

//    public void RefillAmmo()
//    {
//        _ammoCur = _ammoMax;
//    }

//    public WEAPON_TYPE WeaponType { get { return _type; } set { value = _type; } }
//    public GameObject ViewModel { get { return _viewModel; } set { value = _viewModel; } }
//    public ParticleSystem HitEffect { get { return _hitEffect; } set { value = _hitEffect; } }
//    public GameObject Projectile { get { return _projectile; } set { value = _projectile; } }
//    public bool IsOn { get { return _turnedOn; } set { value = _turnedOn; } }
//    public int Damage { get { return _damage; } set { value = _damage; } }
//    public float Range { get { return _range; } set { value = _range; } }
//    public float FireRate { get { return _fireRate; } set { value = _fireRate; } }
//    public int RaysPerFire { get { return _raysPerFire; } set { value = _raysPerFire; } }
//    public float SpreadAmt { get { return _spreadAmt; } set { value = _spreadAmt; } }
//    public int AmmoMax { get { return _ammoMax; } set { value = _ammoMax; } }
//    public int AmmoCur { get { return _ammoCur; } set { value = _ammoCur; } }
//    public AudioClip[] ShootSounds { get { return _shootSounds; } set { value = _shootSounds; } }
//    public AudioClip[] HitSounds { get { return _hitSounds; } set { value = _hitSounds; } }
//    public float ShootVol { get { return _shootVol; } set { value = _shootVol; } }
//    public float HitVol { get { return _hitVol; } set { value = _hitVol; } }
//}
