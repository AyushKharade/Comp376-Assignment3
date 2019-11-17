using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //parameters

    public int health=2;
    int lives = 3;
    public float swimSpeed;
    float fastSwimFactor = 1;
    public float carryWeightFactor=1f;
    float jumpForce = 10f;                           // only applicable when on boat or on surface


    public int SharksChasing = 0;


    [HideInInspector]public int localScore;
    public int throwableItems;
    [HideInInspector]public int itemsCarrying;

    //references to objects
    public Transform facingDirection;
    Rigidbody rigidbodyRef;
    public Camera camRef;
    public GameObject localFigurine;
    public GameObject FigurinePrefab;
    public GameObject UI_E;
    Director Dscript;


    //oxygen cylinder
    //[Range(0f,100f)]
    public float oxygen = 100f;
    [HideInInspector]public float oxyenDepletionRate = 1.5f;

    public float flashCharges = 100f;
    public GameObject FlashLight;


    // timer variables
    float throwTimer;
    float interactTimer;

    //respawn timer
    float respawnTimer;
    float respawnTime = 4f;


    // UI Elements
    public GameObject OxygenUI;
    public Text FigCount;
    public Text CarryCount;
    public Image health1;
    public Image health2;
    public Text LivesUI;
    public GameObject FlashUI;

    public GameObject secondScreen;


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
    //______________________________________________________________________________________________________






    void Start()
    {
        rigidbodyRef = GetComponent<Rigidbody>();
        onSurface = true;
        animator = playerArms.GetComponent<Animator>();

        Dscript = GameObject.FindGameObjectWithTag("Director").GetComponent<Director>();
        oxyenDepletionRate = 1.5f;


        //audio manager
        AudioManager.instance.Play("Underwater");
        AudioManager.instance.sounds[1].volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            PlayerMovement();
            Abilities();
            ManageOxygen();
            ManageFlashCharges();

            //reset velocity if there any for now
            //if (GetComponent<Rigidbody>().velocity != Vector3.zero && !onSurface)
            //    GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (isDead)
        {
            Debug.Log("Dead, respawning in: "+(respawnTime-respawnTimer));
            //coutdown and respawn
            if (respawnTimer > respawnTime && lives > 0)
            {
                Respawn();
                respawnTimer = 0f;
            }
            else
            {
                respawnTimer += Time.deltaTime;
            }
        }





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


        UpdateUI();
        
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



        // enable second camera
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (secondScreen.activeSelf)
                secondScreen.SetActive(false);
            else
                secondScreen.SetActive(true);
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            if (FlashLight.activeSelf)
                FlashLight.SetActive(false);
            else
                FlashLight.SetActive(true);
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
                //Vector3 forceDirection = facingDirection.transform.position - transform.position;
                GameObject temp=Instantiate(FigurinePrefab,localFigurine.transform.position,localFigurine.transform.rotation);
                //temp.GetComponent<Rigidbody>().AddForce(forceDirection*20f,ForceMode.Impulse);
                temp.GetComponent<Rigidbody>().AddForce(facingDirection.forward*20f,ForceMode.Impulse);
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


    private void ManageOxygen()
    {
        //oxygen
        if (!onSurface)
        {
            if (oxygen <= 0)
            {
                //kill player
                RegisterHit();
            }
            else
            {
                oxygen -= oxyenDepletionRate * Time.deltaTime;
                if (oxygen < 0)
                    oxygen = 0;
            }
        }
        else {
            if (oxygen < 100)
            {
                oxygen += 10 * Time.deltaTime;
            }
        }

        //Update UI
        float UI_oxy_value = oxygen / 100f;
        OxygenUI.transform.localScale = new Vector3(UI_oxy_value,1,1);
        if (oxygen < 25f)
        {
            OxygenUI.GetComponentInChildren<Image>().color = new Vector4(166/255f,51/255f,31/255f, 1);
        }
        else
        {
            OxygenUI.GetComponentInChildren<Image>().color = new Vector4(37 / 255f, 51 / 255f, 106 / 255f, 1);
        }
    }


    private void ManageFlashCharges()
    {
        if (FlashLight.activeSelf)
        {
            flashCharges -= 5 * Time.deltaTime;
            if (flashCharges <= 0)
                FlashLight.SetActive(false);
        }
        else
        {
            if (flashCharges < 100)
                flashCharges += 7.5f * Time.deltaTime;
        }

        // update UI
        float flashUI_Value = flashCharges / 100f;
        FlashUI.transform.localScale = new Vector3(flashUI_Value,1,1);
    }

    public void RegisterHit()
    {
        health--;
        if (health <= 0)
        {
            lives --;
            LivesUI.text = "Lives: " +lives;
            animator.SetBool("isDead",true);
            isDead = true;
            //GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().drag = 0;
        }

        // UI
        if (health == 1)
        {
            health2.enabled = false;
        }
        else if (isDead)
            health1.enabled = false;
    }

    public void Respawn()
    {
        health = 2;
        isDead = false;
        localScore = 0;
        throwableItems = 6;
        carryWeightFactor = 1f;
        itemsCarrying = 0;
        oxygen = 100f;
        flashCharges = 100f;
        transform.position = respawnPoint.position;
        GetComponent<Rigidbody>().useGravity = true;
        UI_E.GetComponent<Image>().enabled = false;
        animator.SetBool("isDead", false);

        SharksChasing = 0;


        // ui
        health1.enabled = true;
        health2.enabled = true;

    }


    private void UpdateUI()
    {
        CarryCount.text = ""+itemsCarrying;
        FigCount.text = "" + throwableItems;
    }










    // collison to pick up objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GoldBar" && !isDead)
        {
            UI_E.GetComponent<Image>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GoldBar" && !isDead)
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
