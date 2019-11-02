using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figurine_Script : MonoBehaviour
{


    float timer;
    float deleteTime=45f;


    void Start()
    {
        
    }

    void Update()
    {
        if (timer > deleteTime)
        {
            Destroy(this.gameObject);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }



    //on trigger shark, distract
}
