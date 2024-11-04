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
    //_spawners = new List<GameObject>();
    _checkPointIndex = GameManager.instance.RegisterCheckpoint();
  }


  void OnTriggerEnter(Collider other)
  {
    if (_triggered) return;
    _triggered = true;

    if (!other.isTrigger)
    {
      if (other.GetComponent<test_player_controller>() != null)
      {
        GameManager.instance.ActivateCheckPoint(_checkPointIndex);
        foreach(GameObject spawner in _spawners)
        {
          spawner.SetActive(true);
        }
      }
    }
  }

  public void ResisterSpawner(GameObject spawner)
  {
    spawner.gameObject.SetActive(false);
    _spawners.Add(spawner);
  }

}
