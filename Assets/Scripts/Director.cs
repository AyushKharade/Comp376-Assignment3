using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{

    // parameters
    int maxGold=10;
    int maxSharks=3;
    int maxOctopus=1;

    public int goldCount=0;

    // spawn gold in any random offset from +75 to -75 on x and Z

    // prefab references
    public GameObject gold1Prefab;
    public GameObject gold2Prefab;
    public GameObject gold3Prefab;

    GameObject[] GoldArray = new GameObject[3];


    void Start()
    {
        GoldArray[0] = gold1Prefab;
        GoldArray[1] = gold2Prefab;
        GoldArray[2] = gold3Prefab;
        
    }

    private void FixedUpdate()
    {
        if (goldCount < maxGold && Random.Range(1,100)<2)
        {
            //spawn position
            Vector3 spawnPos = new Vector3(transform.position.x+ Random.Range(-75f, 75f), transform.position.y,transform.position.z+Random.Range(-75f,75f));


            //randomize which gold, chest is rare, goldbar is common
            int index;
            int rand = Random.Range(0, 10);
            if (rand < 5)
                index = 0;
            else if (rand < 8)
                index = 1;
            else
                index = 2;

            //spawn
            //Debug.Log("Spawned index "+index);
            //if(index<2)
              //  Instantiate(GoldArray[index],spawnPos, Quaternion.identity);
            //else
                Instantiate(GoldArray[index],spawnPos, Quaternion.Euler(-90,0,0));

            
            goldCount++;
        }
    }

    void Update()
    {
        
    }
}
