using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    // Start is called before the first frame update
    Player Pscript;
    GameObject target;           // target could be player or a figurine


    Vector3 chaseDirection;
    void Start()
    {
        Pscript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            ChaseTarget();
        }
    }


    private void ChaseTarget()
    {
            //chase target
            chaseDirection = target.transform.position - transform.position;
            transform.LookAt(target.transform.position,Vector3.up);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            //damage player
            Pscript.RegisterHit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null)
        {
            if (other.tag == "Player")
            {
                // assign target
                Debug.Log("Acquired Target: " + other.tag);
                target = other.gameObject;

                
            }
        }

        if (target.tag == "Player" && other.tag == "Figurine")
        {
            //change target, leave player
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == target.tag)
        {
            target = null;
        }
    }
}
