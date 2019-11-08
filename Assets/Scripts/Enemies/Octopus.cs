using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{

    GameObject target;
    public GameObject projectilePrefab;

    float cooldownTimer=0f;

    void Start()
    {
        
    }

    void Update()
    {


        if (target != null && !target.GetComponent<Player>().isDead)
        {
            //Debug.Log("Found you, VERMIN!");
            ChasePlayer();
        }
        else if (target.GetComponent<Player>().isDead)
            target = null;
    }


    //helped methods.

    void ChasePlayer()
    {
        if (target != null)
        {
            


            Vector3 towardsTarget_Dir= (target.transform.position - transform.position).normalized; 
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(towardsTarget_Dir.x,0,towardsTarget_Dir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,5f*Time.deltaTime);


            // chase until close enough
            float distance=Vector3.Distance(target.transform.position,transform.position);

            // follow
            Vector3 followDirection= (target.transform.position - transform.position).normalized;
            transform.Translate(followDirection*5f*Time.deltaTime);


            if (cooldownTimer > 10f && distance>10f)
            {
                cooldownTimer = 0f;
                //instantiate projectiles
                Vector3 forceDirection= (target.transform.position-transform.position).normalized;
                Vector3 spawnPoint = transform.position;

                //1
                GameObject projectileRef=Instantiate(projectilePrefab,new Vector3(spawnPoint.x+2f,spawnPoint.y,spawnPoint.z),Quaternion.identity);
                projectileRef.GetComponent<Rigidbody>().AddForce(forceDirection* Random.Range(80f, 110f), ForceMode.Impulse);


                //2
                GameObject projectileRef2 = Instantiate(projectilePrefab, new Vector3(spawnPoint.x, spawnPoint.y+2.5f, spawnPoint.z), Quaternion.identity);
                projectileRef2.GetComponent<Rigidbody>().AddForce(forceDirection * Random.Range(80f, 110f), ForceMode.Impulse);


                //3
                GameObject projectileRef3 = Instantiate(projectilePrefab, new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z+2f), Quaternion.identity);
                projectileRef3.GetComponent<Rigidbody>().AddForce(forceDirection * Random.Range(80f,110f), ForceMode.Impulse);

            }
            else
            {
                cooldownTimer += Time.deltaTime;
                Debug.Log("Commensing attack in: "+(int)(10f-cooldownTimer)+" seconds. Distance: "+distance);
            }
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.gameObject;
        }
    }
}
