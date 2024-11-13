// PlayerState
// Desc: Struct of player parameters
// Authors: Gabriel Gillette
// Last Modified: Nov, 13 2024

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerState : ScriptableObject
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
        Position = new Vector3(0, 0, 0);
        Orientation = new Quaternion(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
        Scale = new Vector3(1, 1, 1);
        PrevCheckPointID = 0;
        CurrentHealth = 0;
    }

    /*---------------------------------------------- COPY CONSTRUCTOR */

    /// <summary>
    /// Copy Constructor for PlayerState
    /// </summary>
    public PlayerState(PlayerState other)
    {
        Position = new Vector3(other.Position.x, other.Position.y, other.Position.z);
        Orientation = new Quaternion(other.Orientation.x, other.Orientation.y, other.Orientation.z, other.Orientation.w);
        Scale = new Vector3(other.Scale.x, other.Scale.y, other.Scale.z);
        PrevCheckPointID = other.PrevCheckPointID;
        CurrentHealth = other.CurrentHealth;
    }

    /*---------------------------------------------------- PUBLIC METHODS */

    /// <summary>
    /// Set class properties from a PlayerController object.
    /// </summary>
    /// <param name="player"> PlayerController object.</param>
    /// <param name="respectTransform"> Whether to care about the Player's transform.</param>
    public void SetFromPlayer(GameObject player, bool respectTransform=false)
    {
        Transform playerT = player.transform;
        if(respectTransform)
        {
            this.Position = new Vector3(playerT.position.x, playerT.position.y, playerT.position.z);
            this.Orientation = new Quaternion(playerT.rotation.x, playerT.rotation.y, playerT.rotation.z, playerT.rotation.w);
            this.Scale = new Vector3(playerT.localScale.x, playerT.localScale.y, playerT.localScale.z);
        }

        playerController pc = player.GetComponent<playerController>();
        CurrentHealth = pc.Health;

    }


    /// <summary>
    /// Set class properties from a PlayerController object with full transform override.
    /// </summary>
    /// <param name="player"> PlayerController object. </param>
    /// <param name="position"> Global Position </param>
    /// <param name="orientation"> Player orientation. </param>
    /// <param name="scale"> Player scale. </param>
    public void SetFromPlayer(GameObject player, Vector3 position, Quaternion orientation, Vector3 scale)
    {
        SetFromPlayer(player, false);
        this.Position = new Vector3(position.x, position.y, position.z);
        this.Orientation = new Quaternion(orientation.x, orientation.y, orientation.z, orientation.w);
        this.Scale = new Vector3(scale.x, scale.y, scale.z);
    }


    /// <summary>
    /// Set class properties from a PlayerController object with full transform override and scale assumed to be one.
    /// </summary>
    /// <param name="player"> PlayerController object. </param>
    /// <param name="position"> Player position. </param>
    /// <param name="orientation"> Player orientation. </param>
    public void SetFromPlayer(GameObject player, Vector3 position, Quaternion orientation)
    {
        SetFromPlayer(player, position, orientation, new Vector3(1, 1, 1));
    }

    
    /// <summary>
    /// Copy state data back to player.
    /// </summary>
    /// <param name="player"> PlayerController object reference </param>
    /// <param name="respectTransform"> Whether to reflect the transform back or not. </param>
    public void ReflectToPlayer(ref GameObject player, bool respectTransform=false)
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;

        if (respectTransform)
        {
            //controller.transform.position = new Vector3(this.Position.x + 100, this.Position.y, this.Position.z);
            //player.transform.rotation = new Quaternion(this.Orientation.x, this.Orientation.y, this.Orientation.z, this.Orientation.w);
            //player.transform.localScale = new Vector3(this.Scale.x, this.Scale.y, this.Scale.z);
            player.transform.position = new Vector3(this.Position.x , this.Position.y, this.Position.z);
            player.transform.rotation = new Quaternion(this.Orientation.x, this.Orientation.y, this.Orientation.z, this.Orientation.w);
            player.transform.localScale = new Vector3(this.Scale.x, this.Scale.y, this.Scale.z);
        }
        controller.enabled = true;

        playerController pc = player.GetComponent<playerController>();
        pc.Health = this.CurrentHealth;
    }

    public string SerializeState()
    {
        return JsonUtility.ToJson(this);
    }

}