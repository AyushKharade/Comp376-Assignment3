using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_MouseLook : MonoBehaviour
{

    float mouse_sensitivity = 100f;
    public GameObject playerPosRef;

    public Transform facingDirection;


    float xRotation;

    void Start()
    {
        
    }

    void Update()
    {
        // accept mouse input
        float mouseX = Input.GetAxis("Mouse X")*mouse_sensitivity*Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y")*mouse_sensitivity*Time.deltaTime;

        Transform playerT = playerPosRef.GetComponent<Transform>();

        

        xRotation -= mouseY;

        xRotation=Mathf.Clamp(xRotation,-85f,80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //if(!playerPosRef.GetComponent<Player>().onSurface)
            //facingDirection.transform.rotat
        //else
            facingDirection.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);





        // player horizontal rotation
        playerT.Rotate(Vector3.up * mouseX);


    }
}
