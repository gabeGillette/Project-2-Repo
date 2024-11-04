using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{

  [SerializeField] GameObject checkPoint;

  //int _checkPointIndex;
  //int _monsterSpawnerIndex;

  List<GameObject> _spContainers = new List<GameObject>();

  // Start is called before the first frame update
  void Start()
  { 
    if (checkPoint != null)
    {
      //_checkPointIndex = checkPoint.GetComponent<CheckPoint>().CheckPointIndex;
      //_monsterSpawnerIndex = GameManager.instance.RegisterMonsterSpawnerTrigger(_checkPointIndex);
      //gameObject.SetActive(false);
      checkPoint.GetComponent<CheckPoint>().ResisterSpawner(gameObject);
    }
    else
    {
      //_monsterSpawnerIndex = GameManager.instance.RegisterMonsterSpawnerTrigger(0);
    }
  }

  // Update is called once per frame
  void Update()
  {
   
  }

  public void registerContainer(GameObject container)
  {
    _spContainers.Add(container);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<test_player_controller>() != null)
    {
      if (checkPoint != null && GameManager.instance.CurrentPlayerState.ActiveCheckpointID == checkPoint.GetComponent<CheckPoint>().CheckPointID)
      {
        foreach (var container in _spContainers)
        {
          container.GetComponent<SpawnContainer>().Spawn();
        }
      }
      else
      {
        foreach (var container in _spContainers)
        {
          container.GetComponent<SpawnContainer>().Spawn();
        }
      }
    }
  }
}
