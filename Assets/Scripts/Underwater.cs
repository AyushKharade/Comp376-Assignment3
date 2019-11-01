using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour
{
    public GameObject playerRef;
    public GameObject camRef;

    // change light for underwater effect (place holder)
    public Light directionalLightRef;
    Vector4 originalColor;

    void Start()
    {
        originalColor = directionalLightRef.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            //playerRef.GetComponent<Rigidbody>().useGravity=false;
            playerRef.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Physics.gravity = new Vector3(0,-1.5f,0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            directionalLightRef.color = new Vector4(40 / 255f, 50 / 255f, 142 / 255f, 1);
            playerRef.GetComponent<Player>().onSurface = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            directionalLightRef.color = originalColor;
            //playerRef.GetComponent<Rigidbody>().useGravity = true;
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }
}
