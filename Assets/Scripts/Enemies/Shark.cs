using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    // Start is called before the first frame update
    Player Pscript;
    Director Dscript;

    GameObject target;           // target could be player or a figurine

    Vector3 originalRotation = new Vector3(0, -90, 0);
    public float swimSpeed=9f;
    float chaseSpeed=12f;

    float chaseTimer;


    Vector3 chaseDirection;
    void Start()
    {
        Pscript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Dscript = GameObject.FindGameObjectWithTag("Director").GetComponent<Director>();
        RandomizeStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {

            if (transform.eulerAngles != originalRotation)
            {
                transform.eulerAngles = originalRotation;
            }
            Movement();
        }
        else
            ChaseTarget();
        SelfDelete();
    }


    private void Movement()
    {
        //transform.Translate(new Vector3(-swimSpeed*Time.deltaTime,0,0));         // old shark prefab
        transform.Translate(new Vector3(0,0, swimSpeed * Time.deltaTime));         // shark parent prefab
    }

    private void RandomizeStats()
    {
        // size
        float sizeFactor = Random.Range(0.8f,1.2f);
        transform.localScale = new Vector3(sizeFactor,sizeFactor,sizeFactor);

        //speed

        float speedFactor = Random.Range(0f,10f);
        swimSpeed += speedFactor;
    }


    private void SelfDelete()
    {
        if (transform.position.x < -200)
        {
            Dscript.sharkCount -= 1;
            Destroy(this.gameObject);
        }
    }

    private void ChaseTarget()
    {
            //chase target
         chaseDirection = target.transform.position - transform.position;


        Vector3 towardsTarget_Dir = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(towardsTarget_Dir , Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
        // follow
        Vector3 followDirection = (target.transform.position - transform.position).normalized;
        //transform.Translate(followDirection * 15f * Time.deltaTime);
        transform.Translate(transform.forward * 25f * Time.deltaTime);


        if (target.tag == "Player")
        {
            if (target.GetComponent<Player>().isDead)
                target = null;

            if (target.GetComponent<Player>().onSurface)
            {
                target.GetComponent<Player>().SharksChasing -= 1;
                target = null;
                
            }
        }
        else  // its a figurine
        {
            // follow figurine for a few seconds then leave.
            chaseTimer += Time.deltaTime;
            if (chaseTimer > 7.5f)
            {
                target = null;
                chaseTimer = 0;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //only player wasnt already being chased by two sharks
            if (other.GetComponent<Player>().SharksChasing < 1)
            {
                AudioManager.instance.Play("Detected");
                target = other.gameObject;
                other.GetComponent<Player>().SharksChasing += 1;
            }
        }


        if (target != null & other.tag == "Figurine")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SharksChasing -= 1;
            // lock onto other target
            target = other.gameObject;
            Debug.Log("Oo shiny");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            //damage player
            Pscript.RegisterHit();
        }
    }
    
}
