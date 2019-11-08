using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    // Start is called before the first frame update
    Player Pscript;
    Director Dscript;

    GameObject target;           // target could be player or a figurine


    public float swimSpeed=9f;
    float chaseSpeed=12f;

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
        Movement();
        SelfDelete();
    }


    private void Movement()
    {
        transform.Translate(new Vector3(-swimSpeed*Time.deltaTime,0,0));
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
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(towardsTarget_Dir.x, 0, towardsTarget_Dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);

        //
        // follow
        Vector3 followDirection = (target.transform.position - transform.position).normalized;
        transform.Translate(followDirection * 5f * Time.deltaTime);
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
