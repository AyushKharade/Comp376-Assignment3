using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    //Level
    public int level = 1;
    float levelTimer;
    bool level2;         // shark speed boost + 25
    bool level3;        // octopus 
    bool level4;        // shark speed boost +35 (total is now +60) max sharks is 12



    // parameters
    int maxGold=10;
    int maxSharks=8;
    int maxOctopus=1;

    public int goldCount=0;
    public int sharkCount=0;

    // spawn gold in any random offset from +75 to -75 on x and Z

    // prefab references
    public GameObject gold1Prefab;
    public GameObject gold2Prefab;
    public GameObject gold3Prefab;

    GameObject[] GoldArray = new GameObject[3];


    // enemies
    //shark
    public GameObject sharkPrefab;
    float sharkSpeedBoost = 0f;
    // spawn offsets : (-85,+85 on z) && (19 to -44) on y



    // octopus


    // UI ELEMENTS
    public Text LevelUI;


    void Start()
    {
        GoldArray[0] = gold1Prefab;
        GoldArray[1] = gold2Prefab;
        GoldArray[2] = gold3Prefab;

        sharkSpeedBoost = 0f;
        maxSharks = 8;
        maxOctopus = 1;
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



        // if sharks
        if (sharkCount < maxSharks && Random.Range(1, 200) < 2)
        {
            Vector3 spawnPos = new Vector3(130, Random.Range(-44,19),Random.Range(-85,85));
            GameObject temp= Instantiate(sharkPrefab, spawnPos, Quaternion.identity);
            temp.GetComponent<Shark>().swimSpeed += sharkSpeedBoost;
            temp.transform.Rotate(0, -90, 0);
            sharkCount++;
        }


        // octopus spawn

        if (level3)
        { }
    }

    void Update()
    {

        levelTimer += Time.deltaTime;

        if(levelTimer > 30f && !level2)
              IncreaseLevel2();

        if (levelTimer > 75f && !level3)
            IncreaseLevel3();

        if (levelTimer > 100f && !level4)
            IncreaseLevel4();
    }


    
    private void IncreaseLevel2()
    {
        level2 = true;
        level++;
        // increase speed of sharks
        sharkSpeedBoost += 25f;
        LevelUI.text = "Level: " + level;

    }


    private void IncreaseLevel3()
    {
        // spawn octopuses
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().oxyenDepletionRate += 1.75f;
        level3 = true;
        level++;
        LevelUI.text = "Level: " + level;

    }


    private void IncreaseLevel4()
    {
        level4 = true;
        level++;
        LevelUI.text = "Level: "+level;

        sharkSpeedBoost += 35f;
        maxSharks += 4;
    }
}
