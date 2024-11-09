// PlayerState
// Desc: Struct of player parameters
// Authors: Gabriel Gillette
// Last Modified: Nov, 8 2024

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerState 
{

    /*--------------------------------------------------- PRIVATE MEMBERS */

    // Tranfomers, linear algebra in disguise
    public Vector3 _position;
    public Quaternion _orientation;
    public Vector3 _scale;

    // Index of previous checkpoint
    public int _previousCheckPointID;

    // Current Health
    public int _curHealth;

    /*---------------------------------------------------- PUBLIC PROPERTIES */

    public Vector3 Position {  get { return _position; } set { _position = value; } }

    public void SetFromPlayer(playerController player, bool respectTransform=false)
    {
        if(respectTransform)
        {
            this._position = player.transform.position;
            this._orientation = player.transform.rotation;
            this._scale = player.transform.localScale;
        }

        this._curHealth = player.Health;

    }

    public void SetFromPlayer(playerController player, Vector3 position, Quaternion orientation, Vector3 scale)
    {
        SetFromPlayer(player, false);
        this._position = position;
        this._orientation = player.transform.rotation;
        this._scale = scale;
    }

    public void SetFromPlayer(playerController player, Vector3 position, Quaternion orientation)
    {
        SetFromPlayer(player, position, orientation, Vector3.one);
    }

    public void ReflectToPlayer(ref playerController player, bool respectTransform)
    {
        if (respectTransform)
        {
            player.transform.position = this._position;
            player.transform.rotation = this._orientation;
            player.transform.localScale = this._scale;
        }

        player.Health = this._curHealth;
    }
}