using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : MonoBehaviour
{

    [SerializeField] Light _spotLight;
    bool _active;

    public void SwitchLight()
    {
        _active = !_active;
        _spotLight.enabled = _active;
    }

}
