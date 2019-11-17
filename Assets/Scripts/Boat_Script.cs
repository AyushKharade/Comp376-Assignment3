using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boat_Script : MonoBehaviour
{

    //references

    // UI score
    public GameObject UI_Score;


    //variables
    int totalScore;

    GameObject playerRef;
    Player Pscript;
    GameObject UI_E_Ref;



    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        Pscript = playerRef.GetComponent<Player>();
        UI_E_Ref = GameObject.FindGameObjectWithTag("E_Press");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && (Pscript.localScore!=0 || Pscript.throwableItems!=6 || Pscript.oxygen != 100))
        {
            UI_E_Ref.GetComponent<Image>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && (Pscript.localScore != 0 || Pscript.throwableItems != 6 || Pscript.oxygen < 100 || Pscript.flashCharges < 100))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UI_E_Ref.GetComponent<Image>().enabled = false;
                totalScore += Pscript.localScore;
                Pscript.localScore = 0;
                Pscript.carryWeightFactor = 1f;
                Pscript.itemsCarrying = 0;
                Pscript.throwableItems = 6;
                Pscript.oxygen = 100f;
                Pscript.flashCharges = 100f;
                // update UI
                UI_Score.GetComponent<Text>().text = "Score: " + totalScore;

                //animation:
                Pscript.interacting = true;

                playerRef.GetComponent<Player>().SharksChasing = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            UI_E_Ref.GetComponent<Image>().enabled = false;
        }
    }
}
