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

  private int _checkPointFlags;
  private int _monsterSpawnerFlags;
  private int _gameEventFlags;


  private const int CHECKPOINT_MAX = 32;
  private const int MONSTERSPAWNER_MAX = 32*32;
  private const int GAMEEVENT_MAX = 32*32;

  private int _checkPointCount;
  private int _monsterSpawnerCount;
  private int _gameEventCount;


  private PlayerState _playerState;


  // Start is called before the first frame update
  void Awake()
  {
    _instance = this;

    if (playerObject == null)
    {
      Debug.LogError("GameManager: playerObject field not set!");
    }



    PlayerState _playerState = new PlayerState();

  _checkPointFlags = 0;
    _monsterSpawnerFlags = 0;
    _gameEventFlags = 0;

    _checkPointCount = 0;
    _monsterSpawnerCount = 0;
    _gameEventCount = 0;

    //_playerState.transform.position = playerObject.transform.position;
    //_playerState.transform.rotation = playerObject.transform.rotation;
    //_playerState.transform.localScale = playerObject.transform.localScale;

  }

  // Update is called once per frame
  void Update(){}

  public ref GameObject GetPlayer()
  {
    return ref playerObject;
  }

  static public ref GameManager Instance()
  {
    return ref _instance;
  } // Instance

  public void RespawnPlayer()
  {
    playerObject.transform.position = new Vector3(0, 0, 0);
    //playerObject.transform.rotation = _playerState.transform.rotation;
    //playerObject.transform.localScale = _playerState.transform.localScale;
  }

  public int RegisterCheckpoint()
  {
    if (_checkPointCount < CHECKPOINT_MAX)
    {
      _checkPointCount++;
      return _checkPointCount;
    }
    else
    {
      Debug.LogError("Too many checkpoints!");
    }
    return 0;
  }

  public void ActivateCheckPoint(int index)
  {
    int flag = 1 << index;

    if ((flag & _checkPointFlags) == 0)
    {

      _checkPointFlags |= flag;

      _playerState.transform.position = playerObject.transform.position;
      _playerState.transform.rotation = playerObject.transform.rotation;
      _playerState.transform.localScale = playerObject.transform.localScale;
    }
  }

}
