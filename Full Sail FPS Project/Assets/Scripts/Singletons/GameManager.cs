// File: GameManager.CS
// Desc: Singleton class that keeps track of the current game state and acts as
//   a glue between game elements.
// Authors: Gabriel Gillette
// Last Updated: Nov 6, 2024

/*---------------------------------------------------------- SYSTEM INCLUDES */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------- CLASS DEFINITION */

public class GameManager : MonoBehaviour
{


    /*-------------------------------------------------------- SERIALIZED FIELDS */


    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuConfirmQuit;
    [SerializeField] GameObject menuConfirmRestart;

    /*---------------------------------------------------- PRIVATE CLASS MEMBERS */

    GameObject _player;
    float timeScaleOrig;
    bool isPaused;

    static public GameManager instance;

    /* These are what are known as bitflags */

    private int _checkPointFlags;



    private const int CHECKPOINT_MAX = 32;


    private int _checkPointCount;



    private PlayerState _playerState;


    public PlayerState CurrentPlayerState { get { return _playerState; } }


    public GameObject Player { get { return _player; } }

    // Start is called before the first frame update
    void Awake()
    {

        instance = this;
        timeScaleOrig = Time.timeScale;
        _player = GameObject.FindWithTag("Player");


        _playerState = new PlayerState();

        _checkPointFlags = 0;


        _checkPointCount = 0;



    }

    // Update is called once per frame
    void Update(){
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }



    public void RespawnPlayer()
    {
        Debug.LogWarning(_playerState.Position);
        _player.transform.SetPositionAndRotation(_playerState.Position, _playerState.Rotation);
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
            _playerState.Position = _player.transform.position;
            _playerState.Rotation = _player.transform.rotation;
            _playerState.ActiveCheckpointID = index;


        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = timeScaleOrig;
        menuActive.SetActive(false);
        menuActive = null;
    }

}
  