using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //parameters
    public float swimSpeed;
    float fastSwimFactor = 1;
    public float carryWeightFactor=1f;
    public float jumpForce = 20f;                           // only applicable when on boat or on surface

    public int localScore;
    public int throwableItems;
    public int itemsCarrying;

    //references to objects
    public Transform facingDirection;
    Rigidbody rigidbodyRef;
    public Camera camRef;
    public GameObject localFigurine;
    public GameObject FigurinePrefab;
    public GameObject UI_E;
    Director Dscript;



    // timer variables
    float throwTimer;
    float interactTimer;




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
    [HideInInspector] public bool interacting;

    [HideInInspector]public bool LookingAtInteractable;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyRef = GetComponent<Rigidbody>();
        onSurface = true;
        animator = playerArms.GetComponent<Animator>();

        Dscript = GameObject.FindGameObjectWithTag("Director").GetComponent<Director>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            //RaycastInteract();
            PlayerMovement();
            Abilities();

            //reset velocity if there any for now
            if (GetComponent<Rigidbody>().velocity != Vector3.zero && !onSurface)
                GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (isDead)
        {
            //coutdown and respawn
        }

        //boat distance, test, the line below works
        //Debug.Log("Distance to boat: "+(int)(Vector3.Distance(transform.position,respawnPoint.position))+" m.");



        // interact animation.
        if (interacting)
        {
            if (interactTimer > 0.8f)
            {
                interacting = false;
                interactTimer = 0f;
                animator.SetBool("interacting",false);
            }
            else
            {
                interactTimer += Time.deltaTime;
                animator.SetBool("interacting", true);
            }
        }
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





    // collison to pick up objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GoldBar")
        {
            UI_E.GetComponent<Image>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GoldBar")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (carryWeightFactor <= 0.45f)
                {
                    Debug.Log("Cannot carry more gold bars.");
                }
                else
                {
                    if (!other.gameObject.GetComponent<GoldBar>().pickedUp)       // other wise it registers same objecy twice
                    {
                        other.gameObject.GetComponent<GoldBar>().pickedUp = true;
                        localScore += other.gameObject.GetComponent<GoldBar>().value;
                        Debug.Log("Picked up value: " + other.gameObject.GetComponent<GoldBar>().value + ", weighing: " + other.gameObject.GetComponent<GoldBar>().weightFactor);
                        carryWeightFactor -= other.gameObject.GetComponent<GoldBar>().weightFactor;
                        interacting = true;
                        UI_E.GetComponent<Image>().enabled = false;
                        itemsCarrying++;
                        Dscript.goldCount -= 1;
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GoldBar")
        {
            UI_E.GetComponent<Image>().enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
