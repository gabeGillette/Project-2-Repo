//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;

//public class SpawnContainer : MonoBehaviour
//{
//  //[SerializeField] GameObject trigger;
//  [SerializeField] GameObject monster;
//  [SerializeField] float delay;

//  private bool _spawned;

//  private void Start()
//  {
//    //if(trigger != null) trigger.GetComponent<SpawnTrigger>().registerContainer(gameObject);
//  }

//  public void Spawn()
//  {
//    if(!_spawned) StartCoroutine(_delayedSpawn());
//  }

//  private IEnumerator _delayedSpawn()
//  {
//    _spawned = true;
//    yield return new WaitForSeconds(delay);
//    Instantiate(monster);
//  }
  
//}
