﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseClass : MonoBehaviour
{
    public bool canBeAttacked = false;

    //Character Stats
    //Attack Stats
    [Header("ATTACK STATS")]
    [SerializeField]
    private int _damageModifier = 0;
    [SerializeField]
    private float _criticalChanceModifier = 0.0f;
    [SerializeField]
    private int _attackRangeModifier = 0;
    [SerializeField]
    private float _criticalDamageModifier = 0;

    //TODO: Elemental Attack
    [Header("ELEMENTAL ATTACK STATS")]
    [SerializeField]
    private int _elementalDmgFire = 0;
    [SerializeField]
    private int _elementalDmgElectricity = 0;
    [SerializeField]
    private int _elementalDmgCold = 0;
    [SerializeField]
    private int _elementalDmgPoison = 0;

    [Header("DEFENSE STATS")]
    //Defense Stats
    [SerializeField]
    private int _armorNormal = 0;
    [SerializeField]
    private int _armorBlindage = 0;
    [SerializeField]
    private float _dodgeChance = 0.0f;

    //TODO: Elemental Defense
    [Header("ELEMENTAL DEFENSE STATS")]
    [SerializeField]
    private int _elementalDefFire = 0;
    [SerializeField]
    private int _elementalDefElectricity = 0;
    [SerializeField]
    private int _elementalDefCold = 0;
    [SerializeField]
    private int _elementalDefPoison = 0;

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
