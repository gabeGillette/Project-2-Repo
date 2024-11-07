using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuConfirmQuit;
    [SerializeField] GameObject menuConfirmRestart;
    [SerializeField] Image playerHPBar;
    [SerializeField] TMP_Text playerHPText;
    [SerializeField] TMP_Text infoText;
    [SerializeField] float infoTime;

    GameObject _player;
    float timeScaleOrig;
    bool isPaused;

    public static GameManager instance;
    public GameObject playerDamageScreen;


    private int _checkPointFlags;
    private const int CHECKPOINT_MAX = 32;
    private int _checkPointCount;

    private PlayerState _playerState;
    public PlayerState CurrentPlayerState => _playerState;
    public GameObject Player => _player;

    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        _player = GameObject.FindWithTag("Player");

        _playerState = new PlayerState();
        _checkPointFlags = 0;
        _checkPointCount = 0;
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

    public void RespawnPlayer()
    {
        Debug.LogWarning(_playerState.Position);
        _player.transform.SetPositionAndRotation(_playerState.Position, _playerState.Rotation);
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
            _playerState.Position = _player.transform.position;
            _playerState.Rotation = _player.transform.rotation;
            _playerState.ActiveCheckpointID = index;
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

    public void CancelRestart()
    {
        menuConfirmRestart.SetActive(false);
        menuActive = null;
        stateUnpause(); // Resume the game
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

   
}
