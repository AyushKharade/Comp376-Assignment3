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

    public int localScore;
    public int throwableItems;

    //references to objects
    public Transform facingDirection;
    Rigidbody rigidbodyRef;
    public Camera camRef;
    public GameObject localFigurine;
    public GameObject FigurinePrefab;



    // timer variables
    float throwTimer;





    //repspawn point
    public Transform respawnPoint;


    //Animator animator;
    public GameObject playerArms;
    Animator animator;

    // states
    [HideInInspector]public bool isDead;
    public bool onSurface;
    bool onBoat;
    bool Swimming;         // swim animation
    bool isThrowing;

    [HideInInspector]public bool LookingAtInteractable;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyRef = GetComponent<Rigidbody>();
        onSurface = true;
        animator = playerArms.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            //RaycastInteract();
            PlayerMovement();
            Abilities();
        }

        if (isDead)
        {
            //coutdown and respawn
        }

        //boat distance, test, the line below works
        //Debug.Log("Distance to boat: "+(int)(Vector3.Distance(transform.position,respawnPoint.position))+" m.");
    }


    // movement
    private void PlayerMovement()
    {

        //WASD swim:
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += facingDirection.forward * swimSpeed * fastSwimFactor* Time.deltaTime;
            animator.SetBool("isSwimming", true);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += facingDirection.forward * -swimSpeed * fastSwimFactor*Time.deltaTime;
            animator.SetBool("isSwimming", true);
        }

        
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += facingDirection.right * -swimSpeed * fastSwimFactor/2*Time.deltaTime;
            animator.SetBool("isSwimming", true);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += facingDirection.right * swimSpeed *fastSwimFactor/2* Time.deltaTime;
            animator.SetBool("isSwimming", true);
        }

        // letting go keys
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("isSwimming",false);
        }


        //animation state for surface
        if (onSurface)
            animator.SetBool("onSurface", true);
        else
            animator.SetBool("onSurface",false);









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
        if (Input.GetMouseButtonDown(0))
        {
            if (throwableItems > 0 && !onSurface)
            {
                throwableItems--;
                localFigurine.SetActive(true);
                isThrowing = true;
                animator.SetBool("throwing",true);
            }
            else
            {
                //make UI red and white
            }
        }


        // if throwing was true
        if (isThrowing)
        {
            if (throwTimer > 0.75f)
            {
                localFigurine.SetActive(false);
                isThrowing = false;
                throwTimer = 0;
                animator.SetBool("throwing", false);

                //instantiate
                Vector3 forceDirection = localFigurine.transform.position - transform.position;
                GameObject temp=Instantiate(FigurinePrefab,localFigurine.transform.position,Quaternion.identity);
                temp.GetComponent<Rigidbody>().AddForce(forceDirection*20f,ForceMode.Impulse);
            }
            else
            {
                throwTimer += Time.deltaTime;
            }
        }
    }

    // doesnt work properly
    private void RaycastInteract()
    {
        Ray ray = camRef.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (hit.collider.tag == "GoldBar")
            {
                LookingAtInteractable = true;
            }
            else if(hit.collider==null)
            {
                LookingAtInteractable = false;
            }
        }
        
        if (hit.collider != null)
        {
            Debug.Log("Looking at " + hit.collider.tag);
            Debug.DrawRay(transform.position, hit.transform.position, Color.green);
        }
        // draw ray
        
    }

}
