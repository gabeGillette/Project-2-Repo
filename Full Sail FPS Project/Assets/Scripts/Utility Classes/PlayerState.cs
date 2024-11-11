// PlayerState
// Desc: Struct of player parameters
// Authors: Gabriel Gillette
// Last Modified: Nov, 9 2024

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerState 
{

    /*---------------------------------------------------- PUBLIC PROPERTIES */

    // Tranfomers, linear algebra in disguise

    /// <summary>
    /// Player position.
    /// </summary>
    public Vector3 Position { get; set; }

    /// <summary>
    /// Player orientation.
    /// </summary>
    public Quaternion Orientation { get; set; }

    /// <summary>
    /// Player scale.
    /// </summary>
    public Vector3 Scale { get; set; }

    /// <summary>
    /// Index of previous checkpoint
    /// </summary>
    public int PrevCheckPointID { get; set; }

    /// <summary>
    /// Current Player Health.
    /// </summary>
    public int CurrentHealth { get; set; }

    /*---------------------------------------------------- CONSTRUCTOR */

    /// <summary>
    /// Constructor for PlayerState
    /// </summary>
    public PlayerState() 
    {
        Position = Vector3.zero;
        Orientation = Quaternion.identity;
        Scale = Vector3.one;
        PrevCheckPointID = 0;
        CurrentHealth = 0;
    }

    /*---------------------------------------------- COPY CONSTRUCTOR */

    /// <summary>
    /// Copy Constructor for PlayerState
    /// </summary>
    public PlayerState(PlayerState other)
    {
        Position = other.Position;
        Orientation = other.Orientation;
        Scale = other.Scale;
        PrevCheckPointID = other.PrevCheckPointID;
        CurrentHealth = other.CurrentHealth;
    }

    /*---------------------------------------------------- PUBLIC METHODS */

    /// <summary>
    /// Set class properties from a PlayerController object.
    /// </summary>
    /// <param name="player"> PlayerController object.</param>
    /// <param name="respectTransform"> Whether to care about the Player's transform.</param>
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


    /// <summary>
    /// Set class properties from a PlayerController object with full transform override.
    /// </summary>
    /// <param name="player"> PlayerController object. </param>
    /// <param name="position"> Global Position </param>
    /// <param name="orientation"> Player orientation. </param>
    /// <param name="scale"> Player scale. </param>
    public void SetFromPlayer(playerController player, Vector3 position, Quaternion orientation, Vector3 scale)
    {
        SetFromPlayer(player, false);
        this.Position = position;
        this.Orientation = player.transform.rotation;
        this.Scale = scale;
    }


    /// <summary>
    /// Set class properties from a PlayerController object with full transform override and scale assumed to be one.
    /// </summary>
    /// <param name="player"> PlayerController object. </param>
    /// <param name="position"> Player position. </param>
    /// <param name="orientation"> Player orientation. </param>
    public void SetFromPlayer(playerController player, Vector3 position, Quaternion orientation)
    {
        SetFromPlayer(player, position, orientation, Vector3.one);
    }

    
    /// <summary>
    /// Copy state data back to player.
    /// </summary>
    /// <param name="player"> PlayerController object reference </param>
    /// <param name="respectTransform"> Whether to reflect the transform back or not. </param>
    public void ReflectToPlayer(ref playerController player, bool respectTransform=false)
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