using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontrol : MonoBehaviour
{
    //camera sensitivity
    [SerializeField] int sens;
    //min and max of lock
    [SerializeField] int lockvertmin, lockvertmax;
    //plane comtrols
    [SerializeField] bool inverty;
    //x's rotation
    float rotX;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;

        if (inverty)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;

        }

        //clamp camera 
        rotX = Mathf.Clamp(rotX, lockvertmin, lockvertmax);
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);

    }
}