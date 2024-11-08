//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SpawnTrigger : MonoBehaviour
//{

//  GameObject checkPoint;

//  //int _checkPointIndex;
//  //int _monsterSpawnerIndex;

//  List<GameObject> _spContainers = new List<GameObject>();

//  // Start is called before the first frame update
//  void Start()
//  {
//        /*if (checkPoint != null)
//        {
//          checkPoint.GetComponent<CheckPoint>().ResisterSpawner(gameObject);
//        }
//        else
//        {
//          //_monsterSpawnerIndex = GameManager.instance.RegisterMonsterSpawnerTrigger(0);
//        }*/

//        // find all spawn triggers
//        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
//        {
//            if (transform.GetChild(childIndex).CompareTag("Spawn_Container"))
//            {
//                _spContainers.Add(transform.GetChild(childIndex).gameObject);
//            }
//        }
//    }

//  // Update is called once per frame
//  void Update()
//  {
   
//  }

//  public void registerContainer(GameObject container)
//  {
//    _spContainers.Add(container);
//  }

//  private void OnTriggerEnter(Collider other)
//  {
//    if (other.CompareTag("Player"))
//    {
//      if (checkPoint != null && GameManager.instance.CurrentPlayerState.ActiveCheckpointID == checkPoint.GetComponent<CheckPoint>().CheckPointID)
//      {
//        foreach (var container in _spContainers)
//        {
//          container.GetComponent<SpawnContainer>().Spawn();
//        }
//      }
//      else
//      {
//        foreach (var container in _spContainers)
//        {
//          container.GetComponent<SpawnContainer>().Spawn();
//        }
//      }
//    }
//  }
//}
