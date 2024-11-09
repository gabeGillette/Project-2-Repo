// GameManager
// Desc: Singleton Class that handles top-level gameplay priorities.
// Authors: Gabriel Gillette, Kenton Weaver, Adam McKee
// Last Modified: Nov, 9 2024

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /*------------------------------------------------------ PRIVATE MEMBERS */

    private GameObject menuActive;
    private int _checkPointFlags;
    private const int CHECKPOINT_MAX = 32;
    private int _checkPointCount;
    private bool _playerIsRespawning;
    GameObject _player;
    float timeScaleOrig;
    bool isPaused;
    public static GameManager instance;
    public GameObject playerDamageScreen;
    public GameObject playerPoisonScreen;
    public GameObject player;
    private PlayerState _playerState;

    private CanvasGroup poisonScreenCanvasGroup;

    /*--------------------------------------------------- SERIALIZED MEMBERS */

    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuConfirmQuit;
    [SerializeField] GameObject menuConfirmRestart;
    [SerializeField] Image playerHPBar;
    [SerializeField] TMP_Text playerHPText;
    [SerializeField] TMP_Text infoText;
    [SerializeField] float infoTime;

    /*---------------------------------------------------- PUBLIC PROPERTIES */

    public PlayerState CurrentPlayerState => _playerState;
    public GameObject Player => _player;

    /*--------------------------------------------------------- UNITY EVENTS */

    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        _player = GameObject.FindWithTag("Player");

        _playerState = new PlayerState();
        _checkPointFlags = 0;
        _checkPointCount = 0;

        poisonScreenCanvasGroup = playerPoisonScreen.GetComponent<CanvasGroup>();

    }

    void Update()
    {
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

    /*------------------------------------------------------ PRIVATE METHODS */



    /*------------------------------------------------------- PUBLIC METHODS */

    [Obsolete] public void RespawnPlayer()
    {
        // TODO: DELETE ME!
        /*if (!_playerIsRespawning)
        {
            _playerIsRespawning = true;

            Debug.LogWarning(_playerState.Position);
            _player.transform.SetPositionAndRotation(_playerState.Position, _playerState.Rotation);
            _player.GetComponent<playerController>().Health = 5;
            _playerIsRespawning = false;
        }*/
        RespawnPlayer(true);
    }

    public void RespawnPlayer(bool LastCheckPoint)
    {
        Debug.Log("Player Respawned");
        //_player.transform.SetPositionAndRotation(_playerState.Position, _playerState.Rotation);
        _player.GetComponent<playerController>().Health = 5;
        
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
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
            //_playerState.Position = _player.transform.position;
            //_playerState.Rotation = _player.transform.rotation;
            //_playerState.ActiveCheckpointID = index;
        }
    }

    public void statePause()
    {
        isPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void stateUnpause()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = timeScaleOrig;
        if (menuActive != null) menuActive.SetActive(false);
        menuActive = null;
    }

    public void updatePlayerHealth(int total, int max)
    {
        float normalizedAmt = (float)total / max;
        playerHPBar.fillAmount = Mathf.Clamp(normalizedAmt, 0, 1);
        playerHPText.text = (normalizedAmt * 100).ToString("F0");
    }

    public void displayInfo(string msg)
    {
        StartCoroutine(_displayInfo(msg, infoTime));
    }

    IEnumerator _displayInfo(string msg, float time)
    {
        infoText.gameObject.SetActive(true);
        infoText.text = msg;
        yield return new WaitForSeconds(time);
        infoText.gameObject.SetActive(false);
    }

    public void restartlevel()
    {
        
        if (menuLose != null)
        {
            menuLose.SetActive(false);
        }
        statePause();
        menuActive = menuConfirmRestart;
        menuActive.SetActive(true);
    }

    public void ConfirmRestart()
    {
        Time.timeScale = timeScaleOrig; // Reset time scale
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name); // Reload the current level
        menuActive = null;
    }


    // ------------ MENU STUFF

    public void CancelRestart()
    {
        menuConfirmRestart.SetActive(false);
        menuActive = null;
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }

    public void CancelQuit()
    {
        menuConfirmQuit.SetActive(false);
        menuActive = null;
        stateUnpause(); // Resume the game
    }

    public void WinGame()
    {
        menuWin.SetActive(true);
        menuActive = menuWin;
        statePause(); // Resume the game
    }

    public void FadeOutPoisonScreen(float duration)
    {
       StartCoroutine(FadeCanvasGroup(poisonScreenCanvasGroup, poisonScreenCanvasGroup.alpha, 0f, duration));
    }
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // Interpolate the alpha value
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Make sure we set the final alpha
        canvasGroup.alpha = endAlpha;
    }
}
