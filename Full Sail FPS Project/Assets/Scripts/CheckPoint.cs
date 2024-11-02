using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

  private int _checkPointIndex;

    // Start is called before the first frame update
    void Start()
    {
      _checkPointIndex = GameManager.instance.RegisterCheckpoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  void OnTriggerEnter(Collider other)
  {
    if (!other.isTrigger)
    {
      if (other.GetComponent<test_player_controller>() != null)
      {
        GameManager.instance.ActivateCheckPoint(_checkPointIndex);
      }
    }
  }
}
