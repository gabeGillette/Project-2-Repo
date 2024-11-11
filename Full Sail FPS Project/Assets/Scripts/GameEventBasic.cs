using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventBasic : MonoBehaviour, IGameEvent
{

    private List<GameObject> _children = new();

    public void FireEvent()
    {
        ActivateAllChildren();
    }

    // Start is called before the first frame update
    void Start()
    {
        CacheAllChildren();
        DeactivateAllChildren();
    }

    private void CacheAllChildren()
    {
        for (int childIndex = 0; childIndex < transform.childCount; ++childIndex)
        {
            _children.Add(transform.GetChild(childIndex).gameObject);
        }
    }

    private void DeactivateAllChildren()
    {
        foreach (GameObject child in _children)
        {
            child.SetActive(false);
        }
    }

    private void ActivateAllChildren()
    {
        foreach (GameObject child in _children)
        {
            child.SetActive(true);
        }
    }

}
