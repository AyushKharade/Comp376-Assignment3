using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldBar : MonoBehaviour
{
    //parameters
    int value=10;
    float weightFactor=0.05f;




    // references
    GameObject playerRef;
    GameObject UI_E_Ref;


    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        UI_E_Ref = GameObject.FindGameObjectWithTag("E_Press");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            //assign valye to local score and weight to weight factor.
            playerRef.GetComponent<Player>().localScore += value;
            if (playerRef.GetComponent<Player>().carryWeightFactor <= 0.45f)
            {
                Debug.Log("Cannot carry more gold bars.");
            }
            else
            {
                playerRef.GetComponent<Player>().carryWeightFactor -= weightFactor;
                UI_E_Ref.GetComponent<Image>().enabled = false;
                Destroy(this.gameObject);
            }
        }
    }









    // Trigger methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            UI_E_Ref.GetComponent<Image>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MainCamera")
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            UI_E_Ref.GetComponent<Image>().enabled = false;
        }
    }


}
