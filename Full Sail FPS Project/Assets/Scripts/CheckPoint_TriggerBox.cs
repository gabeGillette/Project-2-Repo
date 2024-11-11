using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint_TriggerBox : MonoBehaviour, IGameEvent
{
    bool _triggered = false;

    public void FireEvent()
    {
        if (_triggered) return;
        GameManager.Instance.ActivateCheckPoint(transform.position, transform.rotation);
        _triggered = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            FireEvent();
        }
    }
}
