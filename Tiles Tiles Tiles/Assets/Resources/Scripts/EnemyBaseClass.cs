﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseClass : MonoBehaviour
{
    public bool canBeAttacked = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (canBeAttacked)
        {
            Debug.Log("Mouse over Enemy!!");
        }
    }


}
