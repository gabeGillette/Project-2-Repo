// GameManager
// Desc: Singleton Class that handles top-level gameplay priorities.
// Authors: Gabriel Gillette, Kenton Weaver, Adam McKee
// Last Modified: Nov, 9 2024

using System;
using System.Collections;
using System.IO;

//using System.Reflection;
//using System.Runtime.Remoting.Metadata.W3cXsd2001;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.EventSystems;
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

    /// <summary>
    /// Reference to checkpoint indicator UI graphic
    /// </summary>
    [SerializeField] GameObject _checkPointIndicator;

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
            RestoreCheckpointState();
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            ActivateCheckPoint(Player.transform.position, Player.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            saveGame("test");
        }
#endif

    }


    /*------------------------------------------------------ PRIVATE METHODS */



    /*------------------------------------------------------- PUBLIC METHODS */

    [Obsolete] public void RespawnPlayer()
    {
        return;
    }


    /// <summary>
    /// Restores game state back to last checkpoint.
    /// </summary>
    public void RestoreCheckpointState()
    {
        _playerState.ReflectToPlayer(ref _playerGameOb, true);
    }


    /// <summary>
    /// Display "lose" menu
    /// </summary>
    public void youLose()
    {
        statePause();
        _menuActive = _menuLose;
        _menuActive.SetActive(true);
    }



    /// <summary>
    /// Sets checkpoint
    /// </summary>
    /// <param name="position">Player respawn position.</param>
    /// <param name="orientaion">Player respawn rotation.</param>
    public void ActivateCheckPoint(Vector3 position, Quaternion orientaion)
    {
        _playerState.SetFromPlayer(_playerGameOb, position, orientaion);
        StartCoroutine(displayCheckpointMessage(2.0f));
    }


    /// <summary>
    /// Opens a menu specified by enum.
    /// </summary>
    /// <param name="menu">Menu to open.</param>
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


    /// <summary>
    /// Pauses the game state.
    /// </summary>
    public void statePause()
    {
        _isPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }


    /// <summary>
    /// Unpauses the game state.
    /// </summary>
    public void stateUnpause()
    {
        _isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = _timeScaleOrig;
        OpenMenu(MENU.NONE);
    }


    /// <summary>
    /// Update the player health UI.
    /// </summary>
    /// <param name="total">Current health.</param>
    /// <param name="max">Max health.</param>
    public void updateHealthDisplay(int total, int max)
    {
        float normalizedAmt = (float)total / max;
        _healthBar.fillAmount = Mathf.Clamp(normalizedAmt, 0, 1);
        _numericHealthDisplay.text = (normalizedAmt * 100).ToString("F0");
    }


    /// <summary>
    /// Completely reload the level.
    /// </summary>
    public void restartlevel()
    {
        Time.timeScale = _timeScaleOrig; // Reset time scale
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name); // Reload the current level
    }


    /// <summary>
    /// Starts "win" sequence.
    /// </summary>
    public void WinGame()
    {
        OpenMenu(MENU.WIN);
        statePause(); // Resume the game
    }


    /// <summary>
    /// Displays poision animation.
    /// </summary>
    /// <param name="duration">Seconds to display.</param>
    public void FadeOutPoisonScreen(float duration)
    {
       StartCoroutine(FadeCanvasGroup(_poisonScreenCanvasGroup, _poisonScreenCanvasGroup.alpha, 0f, duration));
    }


    /// <summary>
    /// Quit out of the application completely like a damn quitter.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Exiting the game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    /// <summary>
    /// Fade a canvas group.
    /// </summary>
    /// <param name="canvasGroup">Canvas group to fade.</param>
    /// <param name="startAlpha">Init alpha.</param>
    /// <param name="endAlpha">Target alpha.</param>
    /// <param name="duration">Time.</param>
    /// <returns></returns>
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


    /// <summary>
    /// Display the checkpoint indicator.
    /// </summary>
    /// <param name="time">Seconds to display.</param>
    /// <returns></returns>
    private IEnumerator displayCheckpointMessage(float time)
    {
        _checkPointIndicator.SetActive(true);
        yield return new WaitForSeconds(time);
        _checkPointIndicator.SetActive(false);
    }



    public int saveGame(string filename)
    {

        string filePath = Path.Combine(Application.persistentDataPath, "filename.sav");

        Directory.CreateDirectory(filePath);

        PlayerState pst = PlayerState.CreateInstance<PlayerState>();
        //pst = new PlayerState(_playerState);
        pst.Position = Vector3.zero;
        pst.Scale = Vector3.one;
        pst.Orientation = Quaternion.identity;
        

        string playerSav = JsonUtility.ToJson(pst);

        File.WriteAllText(Path.Combine(filePath, "player.json"), playerSav);

        return 1;
    }
}
