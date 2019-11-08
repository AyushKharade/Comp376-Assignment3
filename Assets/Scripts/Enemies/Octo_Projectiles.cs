using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octo_Projectiles : MonoBehaviour
{

    float deleteTime = 10f;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (timer > deleteTime)
        {
            Destroy(this.gameObject);
        }
        else
            timer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Player>().RegisterHit();
        }
        
    }
}
