using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Resume()
    {
        //GameManager.instance.stateUnpause();
        Debug.Log("resume");
    }

    public void Restart()
    {
        Debug.Log("restart");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameManager.instance.stateUnpause();
    }

    public void LastCheckPoint()
    {
        Debug.Log("lcp");
    }

    public void ConfirmQuitY()
    {
        Debug.Log("lcp");
    }

    public void ConfirmQuitN()
    {
        Debug.Log("lcp");
    }

    public void ConfirmRestartY()
    {
        Debug.Log("lcp");
    }

    public void ConfirmRestartN()
    {
        Debug.Log("lcp");
    }

    public void Quit()
    {
        Debug.Log("get me the heck outa here!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();

#endif
    }
}
