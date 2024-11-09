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

    /*---------------------------------------------------- PUBLIC PROPERTIES */

    // Tranfomers, linear algebra in disguise
    public Vector3 Position { get; set; }
    public Quaternion Orientation { get; set; }
    public Vector3 Scale { get; set; }

    // Index of previous checkpoint
    public int PrevCheckPointID { get; set; }

    // Current Health
    public int CurrentHealth { get; set; }

    /*---------------------------------------------------- CONSTRUCTOR */

    public PlayerState() 
    {
        Position = Vector3.zero;
        Orientation = Quaternion.identity;
        Scale = Vector3.one;
        PrevCheckPointID = 0;
        CurrentHealth = 0;
    }


    /*---------------------------------------------------- PUBLIC METHODS */

    // these be setters
    // couple overloads for convienence

    public void SetFromPlayer(playerController player, bool respectTransform=false)
    {
        if(respectTransform)
        {
            this.Position = player.transform.position;
            this.Orientation = player.transform.rotation;
            this.Scale = player.transform.localScale;
        }

        this.CurrentHealth = player.Health;

    }

    public void SetFromPlayer(playerController player, Vector3 position, Quaternion orientation, Vector3 scale)
    {
        SetFromPlayer(player, false);
        this.Position = position;
        this.Orientation = player.transform.rotation;
        this.Scale = scale;
    }

    public void SetFromPlayer(playerController player, Vector3 position, Quaternion orientation)
    {
        SetFromPlayer(player, position, orientation, Vector3.one);
    }

    // Copy state data back to player

    public void ReflectToPlayer(ref playerController player, bool respectTransform)
    {
        if (respectTransform)
        {
            player.transform.position = this.Position;
            player.transform.rotation = this.Orientation;
            player.transform.localScale = this.Scale;
        }

        player.Health = this.CurrentHealth;
    }
}