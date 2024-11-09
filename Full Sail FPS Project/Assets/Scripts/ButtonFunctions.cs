using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.stateUnpause();
    }

    public void Restart()
    {
        GameManager.Instance.restartlevel();
    }

    public void LastCheckPoint()
    {
        GameManager.Instance.RespawnPlayer();
    }

    public void ConfirmQuitY()
    {
        GameManager.Instance.ConfirmQuit();
    }

    public void ConfirmQuitN()
    {
        GameManager.Instance.CancelQuit();
    }

    public void ConfirmRestartY()
    {
        GameManager.Instance.ConfirmRestart();
    }

    public void ConfirmRestartN()
    {
        GameManager.Instance.CancelRestart();
    }

    public void Quit()
    {
        Debug.Log("Exiting the game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
