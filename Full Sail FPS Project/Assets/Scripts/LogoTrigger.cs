using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoTrigger : MonoBehaviour
{
    public void NextState()
    {
        MainMenuManager.Instance.NextState();
    }
}
