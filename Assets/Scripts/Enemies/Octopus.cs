using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{

    float OctopousSpeed=2.5f;
    float OctopousSwimSpeed=0.2f;
    int faceDir = 1;

    public GameObject target;
    public GameObject projectilePrefab;

    float cooldownTimer=0f;

    float existTime=55f;
    public float existTimer;
    bool timeToLeave;

    void Start()
    {
        OctopousSwimSpeed += Random.Range(0f, 0.4f);
    }

    void Update()
    {


        if (target != null)
        {
            if (target.GetComponent<Player>().isDead)
            {
                target = null;
            }
            ChasePlayer();
        }

        // if no target mind business.
        else if(!timeToLeave && target==null)
        {
            if (transform.position.z > 150)
            {
                transform.Rotate(0, 180, 0);
                faceDir = -1;
                OctopousSwimSpeed = Random.Range(0.2f, 0.6f);
            }
            else if (transform.position.z < -150)
            {
                transform.Rotate(0, -180, 0);
                faceDir = 1;
                OctopousSwimSpeed = Random.Range(0.2f, 0.6f);
            }

            // translation
            transform.Translate(transform.forward * OctopousSwimSpeed*faceDir);

            //fix orientation
            if (transform.localRotation.x != 0 || transform.localRotation.z != 0)
                transform.localRotation = Quaternion.Euler(0,transform.localRotation.y,0);



        }

        // exist
        existTimer += Time.deltaTime;
        if (existTimer > existTime)
        {
            //leave
            target = null;
            timeToLeave = true;

            // rotate
            //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(transform.position.x,transform.position.y,150));
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);

            // keep moving
            transform.Translate(transform.forward*faceDir*OctopousSwimSpeed);
            if (transform.position.z > 180 || transform.position.z < -180)
            {
                GameObject.FindGameObjectWithTag("Director").GetComponent<Director>().octopusCount -= 1;
                Destroy(this.gameObject);
            }
        }
    }


    //helpee methods.

    void ChasePlayer()
    {
        if (target != null)
        {
            


            Vector3 towardsTarget_Dir= (target.transform.position - transform.position).normalized; 
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(towardsTarget_Dir.x,towardsTarget_Dir.y,towardsTarget_Dir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,5f*Time.deltaTime);


            // chase until close enough
            float distance=Vector3.Distance(target.transform.position,transform.position);

            // follow
            Vector3 followDirection= (target.transform.position - transform.position).normalized;
            transform.Translate(followDirection*5f*Time.deltaTime);

            //if(distance>10f)
            //    transform.Translate(transform.forward*OctopousSpeed);


            if (cooldownTimer > 10f && distance>10f)
            {
                cooldownTimer = 0f;
                //instantiate projectiles
                Vector3 forceDirection= (target.transform.position-transform.position).normalized;
                Vector3 spawnPoint = transform.position;

                //1
                GameObject projectileRef=Instantiate(projectilePrefab,new Vector3(spawnPoint.x+3f,spawnPoint.y,spawnPoint.z),Quaternion.identity);
                projectileRef.GetComponent<Rigidbody>().AddForce(forceDirection* Random.Range(80f, 110f), ForceMode.Impulse);


                //2
                GameObject projectileRef2 = Instantiate(projectilePrefab, new Vector3(spawnPoint.x, spawnPoint.y+3f, spawnPoint.z), Quaternion.identity);
                projectileRef2.GetComponent<Rigidbody>().AddForce(forceDirection * Random.Range(80f, 110f), ForceMode.Impulse);


                //3
                GameObject projectileRef3 = Instantiate(projectilePrefab, new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z+3f), Quaternion.identity);
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
        if (other.tag == "Player" && !timeToLeave)
        {
            target = other.gameObject;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Player>().RegisterHit();
        }
    }
}
