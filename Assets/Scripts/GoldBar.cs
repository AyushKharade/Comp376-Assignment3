﻿using System.Collections;
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


    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        UI_E_Ref = GameObject.FindGameObjectWithTag("E_Press");

        //director
        DScript = GameObject.FindGameObjectWithTag("Director").GetComponent<Director>();

        pickedUp = false;
    }

    void Update()
    {
    }

    // agitateEnemies();
    

}
