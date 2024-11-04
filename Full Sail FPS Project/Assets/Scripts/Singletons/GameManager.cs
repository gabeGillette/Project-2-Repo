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

  static public GameManager instance;

  /* These are what are known as bitflags */

  private int _checkPointFlags;
  //private int _monsterSpawnTriggerFlags;
  //private int _gameEventFlags;


  private const int CHECKPOINT_MAX = 32;
  //private const int MONSTERSPAWNTRIGGER_MAX = 32*32;
  //private const int GAMEEVENT_MAX = 32*32;

  private int _checkPointCount;
  //private int _monsterSpawnTriggerCount;
  //private int _gameEventCount;


  private PlayerState _playerState;
  //private Vector3 _pos;
  //private Quaternion _rot;

  public PlayerState CurrentPlayerState { get { return _playerState; } }


  // Start is called before the first frame update
  void Awake()
  {

    instance = this;

    if (playerObject == null)
    {
      Debug.LogError("GameManager: playerObject field not set!");
    }

    _playerState = new PlayerState();

    _checkPointFlags = 0;
    //_monsterSpawnTriggerFlags = 0;
    //_gameEventFlags = 0;

    _checkPointCount = 0;
    //_monsterSpawnTriggerCount = 0;
    //_gameEventCount = 0;


  }

  // Update is called once per frame
  void Update(){}

  public ref GameObject GetPlayer()
  {
    return ref playerObject;
  }


  public void RespawnPlayer()
  {
    Debug.LogWarning(_playerState.Position);
    playerObject.transform.SetPositionAndRotation(_playerState.Position, _playerState.Rotation);
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
      _playerState.Position = playerObject.transform.position;
      _playerState.Rotation = playerObject.transform.rotation;
      _playerState.ActiveCheckpointID = index;

      //_monsterSpawnTriggerFlags = 0;
      //_gameEventFlags = 0;
    }
  }

  /*
  public int RegisterMonsterSpawnerTrigger(int checkPointID)
  {
    if (_monsterSpawnTriggerCount * _checkPointCount < MONSTERSPAWNTRIGGER_MAX)
    {
      _monsterSpawnTriggerCount++;
      return (_checkPointCount == 0) ? _monsterSpawnTriggerCount : (_monsterSpawnTriggerCount / _checkPointCount);
    }
    else
    {
      Debug.LogError("Too many spawners!");
    }
    return 0;
  }

  public void ActivateSpawner(int checkPoint, int spawnIndex)
  {

    if(_playerState.ActiveCheckpointID != checkPoint) return;
    

    int flag = 1 << spawnIndex;

    if ((flag & _monsterSpawnTriggerFlags) == 0)
    {
      _monsterSpawnTriggerFlags |= flag;
      
    }
  }
  */
}
  