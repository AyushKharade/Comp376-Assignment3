using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //parameters
    public float swimSpeed;
    float fastSwimFactor = 1;
    public float carryWeightFactor=1f;
    public float jumpForce = 20f;                           // only applicable when on boat or on surface

    int throwableItems;

    //references to objects
    public Transform facingDirection;
    Rigidbody rigidbodyRef;


    //repspawn point
    Vector3 respawnPoint;
    

    //Animator animator;

    // states
    [HideInInspector]public bool isDead;
    public bool onSurface;
    bool onBoat;
    bool swimming;         // swim animation

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = transform.position;
        rigidbodyRef = GetComponent<Rigidbody>();
        onSurface = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            PlayerMovement();
            Abilities();
        }

        if (isDead)
        {
            //coutdown and respawn
        }
    }


    // movement
    private void PlayerMovement()
    {

        //WASD swim:
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += facingDirection.forward * swimSpeed * fastSwimFactor* Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += facingDirection.forward * -swimSpeed * fastSwimFactor*Time.deltaTime;
        }

        // A & D, strafe, dot product
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += facingDirection.right * -swimSpeed * fastSwimFactor/2*Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += facingDirection.right * swimSpeed *fastSwimFactor/2* Time.deltaTime;
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && onSurface)
        {
            rigidbodyRef.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        else if (Input.GetKey(KeyCode.Space) && !onSurface)
        {
            transform.Translate(Vector3.up * swimSpeed * Time.deltaTime);
        }

        // go down fast
        if (Input.GetKey(KeyCode.LeftControl) && !onSurface)
        {
            transform.Translate(Vector3.down * swimSpeed * Time.deltaTime);
        }




        // fast swimming
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            fastSwimFactor = 1.5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            fastSwimFactor = 1f;
        }
    }

    //abilities
    private void Abilities()
    {
        // figurines
    }

}
