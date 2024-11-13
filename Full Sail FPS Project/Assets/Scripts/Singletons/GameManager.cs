// GameManager
// Desc: Singleton Class that handles top-level gameplay priorities.
// Authors: Gabriel Gillette, Kenton Weaver, Adam McKee
// Last Modified: Nov, 9 2024

using System;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// Menu Enum
    /// </summary>
    public enum MENU
    {
        PREV = -1,
        NONE = 0,
        PAUSE = 1,
        LOSE = 2,
        WIN = 3,
        CONFIRM_QUIT = 4,
        CONFIRM_RESTART = 5
    }

    /*------------------------------------------------------ PRIVATE MEMBERS */

    /// <summary>
    /// Singleton instance.
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Previous menu.
    /// </summary>
    private MENU _prevMenu;

    /// <summary>
    /// Current open menu.
    /// </summary>
    private MENU _activeMenu;

    /// <summary>
    /// Current open menu GameObject ref.
    /// </summary>
    private GameObject _menuActive;

    // TODO Delete Me
    /// <summary>
    /// Bitflags for active checkpoints.
    /// </summary>
    //private int _checkPointFlags;

    // TODO Delete Me
    /// <summary>
    /// The max number of checkpoints we can have per stage.
    /// </summary>
    // Ints are assumed to be 32 bit on most platforms.
    //private const int _CHECKPOINT_MAX = 32;

    /// <summary>
    /// How many checkpoints exist on the map.
    /// </summary>
    private int _checkPointCount;

    /// <summary>
    /// Player reference.
    /// </summary>
    private GameObject _playerGameOb;

    /// <summary>
    /// Player reference.
    /// </summary>
    private playerController _player;

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
    /// Player state cached from star of scene.
    /// </summary>
    private PlayerState _startPlayerState;

    /// <summary>
    /// Cached poisonscreen canvas group.
    /// </summary>
    private CanvasGroup _poisonScreenCanvasGroup;

    /*--------------------------------------------------- SERIALIZED MEMBERS */

    /// <summary>
    /// The player damage panel;
    /// </summary>
    [SerializeField] GameObject _damagePanel;

    /// <summary>
    /// The palyer poison panel.
    /// </summary>
    [SerializeField] GameObject _poisionPanel;

    /// <summary>
    /// Pause Menu;
    /// </summary>
    [SerializeField] GameObject _menuPause;

    /// <summary>
    /// Win Menu
    /// </summary>
    [SerializeField] GameObject _menuWin;

    /// <summary>
    /// Lose Menu.
    /// </summary>
    [SerializeField] GameObject _menuLose;

    /// <summary>
    /// Confirm Quit menu.
    /// </summary>
    [SerializeField] GameObject _menuConfirmQuit;

    /// <summary>
    /// Confirm restart menu.
    /// </summary>
    [SerializeField] GameObject _menuConfirmRestart;

    /// <summary>
    /// Graphical health bar.
    /// </summary>
    [SerializeField] Image _healthBar;

    /// <summary>
    /// Numeric health display.
    /// </summary>
    [SerializeField] TMP_Text _numericHealthDisplay;

    /*---------------------------------------------------- PUBLIC PROPERTIES */

    /// <summary>
    /// Access the previously cached PlayerState.
    /// </summary>
    public PlayerState CachedPlayerState => _playerState;

    /// <summary>
    /// Access reference to player.
    /// </summary>
    public GameObject Player => _playerGameOb;

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

    /// <summary>
    /// Init the GameManager before anything else.
    /// </summary>
    void Awake()
    {
        _instance = this;
        _timeScaleOrig = Time.timeScale;
        _playerGameOb = GameObject.FindWithTag("Player");
        _player = _playerGameOb.GetComponent<playerController>();

        _playerState = new PlayerState();
        _playerState.SetFromPlayer(_playerGameOb, true);
        _startPlayerState = new PlayerState(_playerState);
        //_checkPointFlags = 0;
        _checkPointCount = 0;

        _poisonScreenCanvasGroup = PlayerPoisonPanel.GetComponent<CanvasGroup>();

    }


    /// <summary>
    /// GameManager update loop.
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (_activeMenu == MENU.NONE)
            {
                statePause();
                OpenMenu(MENU.PAUSE);
            }
            else if (_activeMenu > MENU.PAUSE)
            {
                OpenMenu(MENU.PREV);
            }
            else if (_activeMenu == MENU.PAUSE)
            {
                OpenMenu(MENU.NONE);
                stateUnpause();
            }

        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Period))
        {
            RestorePlayerState();
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            ActivateCheckPoint(Player.transform.position, Player.transform.rotation);
        }
#endif

    }


    /*------------------------------------------------------ PRIVATE METHODS */



    /*------------------------------------------------------- PUBLIC METHODS */

    /// <summary>
    /// Respawn player at last CheckPoint
    /// </summary>
    [Obsolete] public void RespawnPlayer()
    {        
        RespawnPlayer(true);
    }

    [Obsolete] public void RespawnPlayer(bool LastCheckPoint)
    {
        Debug.Log("Player Respawned");
        _playerState.ReflectToPlayer(ref _playerGameOb, true);
    }

    public void RestorePlayerState()
    {
        _playerState.ReflectToPlayer(ref _playerGameOb, true);
    }

    public void youLose()
    {
        statePause();
        _menuActive = _menuLose;
        _menuActive.SetActive(true);
    }

    /* TODO: Delete Me!
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
    }*/

    [Obsolete] public void ActivateCheckPoint(int index)
    {
        /*int flag = 1 << index;

        if ((flag & _checkPointFlags) == 0)
        {
            _checkPointFlags |= flag;

            _playerState.SetFromPlayer(_player, true);
        }*/
        //ActivateCheckPoint();
    }

    public void ActivateCheckPoint(Vector3 position, Quaternion orientaion)
    {
        _playerState.SetFromPlayer(_playerGameOb, position, orientaion);
    }

    public void OpenMenu(MENU menu)
    {
        if (menu < MENU.NONE)
        {
            OpenMenu(_prevMenu);
        }
        else 
        {
            if(_menuActive != null)
            {
                _menuActive.SetActive(false);
            }

            _prevMenu = _activeMenu;
            _activeMenu = menu;
            switch (menu)
            {
                case MENU.PAUSE:
                    _menuActive = _menuPause;
                    _menuActive.SetActive(true);
                    break;
                case MENU.LOSE:
                    _menuActive = _menuLose;
                    _menuActive.SetActive(true);
                    break;
                case MENU.WIN:
                    _menuActive = _menuWin;
                    _menuActive.SetActive(true);
                    break;
                case MENU.CONFIRM_QUIT:
                    _menuActive = _menuConfirmQuit;
                    _menuActive.SetActive(true);
                    break;
                case MENU.CONFIRM_RESTART:
                    _menuActive = _menuConfirmRestart;
                    _menuActive.SetActive(true);
                    break;

                case MENU.NONE:
                default:
                    _menuActive.SetActive(false);
                    break;
            }
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
        OpenMenu(MENU.NONE);
    }

    public void updatePlayerHealth(int total, int max)
    {
        float normalizedAmt = (float)total / max;
        _healthBar.fillAmount = Mathf.Clamp(normalizedAmt, 0, 1);
        _numericHealthDisplay.text = (normalizedAmt * 100).ToString("F0");
    }

    public void displayInfo(string msg)
    {
        StartCoroutine(_displayInfo(msg, 5));
    }

    private IEnumerator _displayInfo(string msg, float time)
    {
        /*infoText.gameObject.SetActive(true);
        infoText.text = msg;*/
        yield return new WaitForSeconds(time);
        /*infoText.gameObject.SetActive(false);*/
    }

    public void restartlevel()
    {
        Time.timeScale = _timeScaleOrig; // Reset time scale
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name); // Reload the current level
    }

    // TODO: Delete me!
    /*public void ConfirmRestart()
    {
        Time.timeScale = _timeScaleOrig; // Reset time scale
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name); // Reload the current level
        _menuActive = null;
    }*/



    public void WinGame()
    {
        OpenMenu(MENU.WIN);
        statePause(); // Resume the game
    }

    public void FadeOutPoisonScreen(float duration)
    {
       StartCoroutine(FadeCanvasGroup(_poisonScreenCanvasGroup, _poisonScreenCanvasGroup.alpha, 0f, duration));
    }

    public void QuitGame()
    {
        Debug.Log("Exiting the game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
