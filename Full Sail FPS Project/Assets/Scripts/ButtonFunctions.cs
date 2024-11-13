using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void ButContinueGame()
    {
        GameManager.Instance.OpenMenu(GameManager.MENU.NONE);
        GameManager.Instance.stateUnpause();
    }

    public void ButRestartLevel()
    {
        GameManager.Instance.OpenMenu(GameManager.MENU.CONFIRM_RESTART);
    }

    public void ButLastCheckPoint()
    {
        GameManager.Instance.OpenMenu(GameManager.MENU.NONE);
        GameManager.Instance.stateUnpause();
        GameManager.Instance.RestoreCheckpointState();
    }

    public void ConfirmQuitY()
    {
        GameManager.Instance.QuitGame();
    }

    public void ConfirmQuitN()
    {
        GameManager.Instance.OpenMenu(GameManager.MENU.PREV);
    }

    public void ConfirmRestartY()
    {
        GameManager.Instance.OpenMenu(GameManager.MENU.NONE);
        GameManager.Instance.stateUnpause();
       // GameManager.Instance.RespawnPlayer(true);
        GameManager.Instance.restartlevel();
    }

    public void ConfirmRestartN()
    {
        GameManager.Instance.OpenMenu(GameManager.MENU.PREV);
    }

    public void Quit()
    {
        GameManager.Instance.OpenMenu(GameManager.MENU.CONFIRM_QUIT);
    }
}
