using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Resume()
    {
        GameManager.instance.stateUnpause();
    }

    public void Restart()
    {
        GameManager.instance.restartlevel();
    }

    public void LastCheckPoint()
    {
        GameManager.instance.RespawnPlayer();
    }

    public void ConfirmQuitY()
    {
        GameManager.instance.ConfirmQuit();
    }

    public void ConfirmQuitN()
    {
        GameManager.instance.CancelQuit();
    }

    public void ConfirmRestartY()
    {
        GameManager.instance.ConfirmRestart();
    }

    public void ConfirmRestartN()
    {
        GameManager.instance.CancelRestart();
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
