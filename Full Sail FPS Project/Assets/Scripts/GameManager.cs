// File: GameManager.CS
// Desc: Singleton class that keeps track of the current game state and acts as
//   a glue between game elements.
// Authors: Gabriel Gillette
// Last Updated: Nov 1, 2024

/*---------------------------------------------------------- SYSTEM INCLUDES */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------- CLASS DEFINITION */

public class GameManager : MonoBehaviour
{


/*-------------------------------------------------------- SERIALIZED FIELDS */

  [SerializeField] GameObject playerObject;

/*---------------------------------------------------- PRIVATE CLASS MEMBERS */

  static private GameManager _instance;

  /* These are what are known as bitflags */

  private ulong _checkPointFlags;
  private ulong _monsterSpawnerFlags;
  private ulong _gameEventFlags;


  // Start is called before the first frame update
  void Awake()
  {
    if (playerObject == null)
    {
      Debug.LogError("GameManager: playerObject field not set!");
    }

    _checkPointFlags = 0;
    _monsterSpawnerFlags = 0;
    _gameEventFlags = 0;
  }

  // Update is called once per frame
  void Update(){}

  public ref GameObject GetPlayer()
  {
    return ref playerObject;
  }

  public ref GameManager Instance()
  {
    return ref _instance;
  } // Instance

}
