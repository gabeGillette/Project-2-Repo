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

    /// <summary>
    /// Singleton instance.
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Current open menu.
    /// </summary>
    private GameObject _menuActive;

    /// <summary>
    /// Bitflags for active checkpoints.
    /// </summary>
    private int _checkPointFlags;

    /// <summary>
    /// The max number of checkpoints we can have per stage.
    /// </summary>
    // Ints are a assumed to be 32 bit on most platforms.
    private const int _CHECKPOINT_MAX = 32;

    /// <summary>
    /// How many checkpoints exist on the map.
    /// </summary>
    private int _checkPointCount;

    /// <summary>
    /// Player reference.
    /// </summary>
    private GameObject _player;

    /// <summary>
    /// Cached timescale. 
    /// </summary>
    private float _timeScaleOrig; // Cache me outside. </humor>

    /// <summary>
    /// Pause flag.
    /// </summary>
    private bool _isPaused;

    /// <summary>
    /// Player state cache.
    /// </summary>
    private PlayerState _playerState;

    /// <summary>
    /// Cached poisonscreen canvas group.
    /// </summary>
    private CanvasGroup _poisonScreenCanvasGroup;



    /*--------------------------------------------------- SERIALIZED MEMBERS */

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] GameObject _damagePanel;
    [SerializeField] GameObject _poisionPanel;

    [SerializeField] GameObject _menuPause;
    [SerializeField] GameObject _menuWin;
    [SerializeField] GameObject _menuLose;
    [SerializeField] GameObject _menuConfirmQuit;
    [SerializeField] GameObject _menuConfirmRestart;
    [SerializeField] Image _healthBar;
    [SerializeField] TMP_Text _digitalHealthDisplay;
    //[SerializeField] TMP_Text infoText;
    //[SerializeField] float infoTime;

    /*---------------------------------------------------- PUBLIC PROPERTIES */

    public PlayerState CachedPlayerState => _playerState;

    /// <summary>
    /// 
    /// </summary>
    public GameObject Player => _player;

    /// <summary>
    /// Access Damage Panel;
    /// </summary>
    public GameObject PlayerDamagePanel => _damagePanel;

    /// <summary>
    /// Access Poison Panel.
    /// </summary>
    public GameObject PlayerPoisonPanel => _poisionPanel;

    /// <summary>
    /// Access GameManager.
    /// </summary>
    public static GameManager Instance => _instance;

    /*--------------------------------------------------------- UNITY EVENTS */

    void Awake()
    {
        _instance = this;
        _timeScaleOrig = Time.timeScale;
        _player = GameObject.FindWithTag("Player");

        _playerState = new PlayerState();
        _checkPointFlags = 0;
        _checkPointCount = 0;

        _poisonScreenCanvasGroup = PlayerPoisonPanel.GetComponent<CanvasGroup>();

    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (_menuActive == null)
            {
                statePause();
                _menuActive = _menuPause;
                _menuActive.SetActive(true);
            }
            else if (_menuActive == _menuPause)
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
        _menuActive = _menuLose;
        _menuActive.SetActive(true);
    }

    public int RegisterCheckpoint()
    {
        if (_checkPointCount < _CHECKPOINT_MAX)
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
        _isPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void stateUnpause()
    {
        _isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = _timeScaleOrig;
        if (_menuActive != null) _menuActive.SetActive(false);
        _menuActive = null;
    }

    public void updatePlayerHealth(int total, int max)
    {
        float normalizedAmt = (float)total / max;
        _healthBar.fillAmount = Mathf.Clamp(normalizedAmt, 0, 1);
        _digitalHealthDisplay.text = (normalizedAmt * 100).ToString("F0");
    }

    public void displayInfo(string msg)
    {
        StartCoroutine(_displayInfo(msg, 5));
    }

    IEnumerator _displayInfo(string msg, float time)
    {
        /*infoText.gameObject.SetActive(true);
        infoText.text = msg;*/
        yield return new WaitForSeconds(time);
        /*infoText.gameObject.SetActive(false);*/
    }

    public void restartlevel()
    {
        
        if (_menuLose != null)
        {
            _menuLose.SetActive(false);
        }
        statePause();
        _menuActive = _menuConfirmRestart;
        _menuActive.SetActive(true);
    }

    public void ConfirmRestart()
    {
        Time.timeScale = _timeScaleOrig; // Reset time scale
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name); // Reload the current level
        _menuActive = null;
    }


    // ------------ MENU STUFF

    public void CancelRestart()
    {
        _menuConfirmRestart.SetActive(false);
        _menuActive = null;
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }

    public void CancelQuit()
    {
        _menuConfirmQuit.SetActive(false);
        _menuActive = null;
        stateUnpause(); // Resume the game
    }

    public void WinGame()
    {
        _menuWin.SetActive(true);
        _menuActive = _menuWin;
        statePause(); // Resume the game
    }

    public void FadeOutPoisonScreen(float duration)
    {
       StartCoroutine(FadeCanvasGroup(_poisonScreenCanvasGroup, _poisonScreenCanvasGroup.alpha, 0f, duration));
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
