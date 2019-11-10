using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldBar : MonoBehaviour
{
    //parameters
    public int value;
    public float weightFactor;

    public bool pickedUp;
    bool expired;
    float expireTime;
    public float expireTimer;

    // references
    GameObject playerRef;
    GameObject UI_E_Ref;
    //GameObject DirectorRef;
    Director DScript;


    //bonus
    bool willAgitate;
    bool agitated;


    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        UI_E_Ref = GameObject.FindGameObjectWithTag("E_Press");

        //director
        DScript = GameObject.FindGameObjectWithTag("Director").GetComponent<Director>();

        pickedUp = false;
        expireTime= Random.Range(35f, 50f);


        if (Random.Range(1, 10) < 6)
            willAgitate = true;                         // not all gold bars will agitate
    }

    void Update()
    {
        
        expireTimer += Time.deltaTime;
        if (expireTimer > expireTime)
        {
            GameObject.FindGameObjectWithTag("Director").GetComponent<Director>().goldCount -= 1;
            Destroy(this.gameObject);
        }


        if (willAgitate && expireTimer > (expireTime - 10f) && !agitated)
        {
            Debug.Log("NEARBY ENEMIES AGITATED.");
            agitated = true;

            // overlap sphere, if player wasnt being chased already, chase
            Collider[] arr = Physics.OverlapSphere(transform.position, 50f);
            GameObject shark=null;
            GameObject playerRef= GameObject.FindGameObjectWithTag("Player");

            foreach (Collider col in arr)
            {
                if (col.tag == "Shark" && playerRef.GetComponent<Player>().SharksChasing == 0)
                {
                    shark = col.gameObject;
                    shark.GetComponent<Shark>().target = playerRef;
                    playerRef.GetComponent<Player>().SharksChasing += 1;
                    break;
                }
                    
            }
            if (shark == null)
                Debug.Log("No sharks were nearby the gold, so no one was agitated");
        }

    }

    // agitateEnemies();
    

}
