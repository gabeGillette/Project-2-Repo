using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

  private int _checkPointIndex;
  private List<GameObject> _spawners = new List<GameObject>();
  private bool _triggered;

  public int CheckPointID
  {
    get {  return _checkPointIndex; }
  }

  // Start is called before the first frame update
  void Start()
  {
        // find all spawn triggers
        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            if(transform.GetChild(childIndex).CompareTag("Spawn_Trigger"))
            {
                _spawners.Add(transform.GetChild(childIndex).gameObject);
            }
        }

    //_spawners = new List<GameObject>();
    _checkPointIndex = GameManager.instance.RegisterCheckpoint();
  }


  void OnTriggerEnter(Collider other)
  {
    if (_triggered) return;
    _triggered = true;

    if (!other.isTrigger)
    {
      if (other.CompareTag("Player"))
      {
        GameManager.instance.displayInfo("CheckPoint Reached!");
        GameManager.instance.ActivateCheckPoint(_checkPointIndex);
        foreach(GameObject spawner in _spawners)
        {
          spawner.SetActive(true);
        }
      }
    }
  }

  public void RegisterSpawner(GameObject spawner)
  {
    spawner.gameObject.SetActive(false);
    _spawners.Add(spawner);
  }

}
