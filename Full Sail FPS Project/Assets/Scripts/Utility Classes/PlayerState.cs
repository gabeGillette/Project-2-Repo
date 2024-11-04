using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState 
{

  private Vector3 _pos;
  private Quaternion _rot;
  private int _activeCheckpointID;


  public PlayerState()
  {
    _pos = Vector3.zero;
    _rot = Quaternion.identity;
  }

  public Vector3 Position
  {
    get { return _pos; }
    set { _pos = value; }
  }

  public Quaternion Rotation
    { 
    get { return _rot; }
    set { _rot = value; }
  }

  public int ActiveCheckpointID
  {
    get { return _activeCheckpointID; }
    set { _activeCheckpointID = value; }
  }

}