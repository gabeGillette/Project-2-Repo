using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class test_player_controller : MonoBehaviour
{
  private uint speed = 10;
  [SerializeField] CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
           characterController.Move(characterController.transform.forward * speed * Time.deltaTime);


        }

    if (Input.GetKey(KeyCode.DownArrow))
    {
      characterController.Move(characterController.transform.forward * (-1) * speed * Time.deltaTime);


    }

    if (Input.GetKey(KeyCode.LeftArrow))
    {

      characterController.transform.Rotate(new Vector3(0, 20 * Time.deltaTime * speed, 0));

    }

    if (Input.GetKey(KeyCode.RightArrow))
    {

      characterController.transform.Rotate(new Vector3(0, -20 * Time.deltaTime * speed, 0));

    }

    if (Input.GetKeyDown(KeyCode.R))
    {

      GameManager.Instance().RespawnPlayer();

    }

  }
}
