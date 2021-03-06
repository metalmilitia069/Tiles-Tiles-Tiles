﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : CharacterCombat/*, IPlayerTeam*/
{
    //Character Stats
    //Attack Stats
    [Header("ATTACK STATS")]
    [SerializeField]
    public int damageModifier = 0;
    [SerializeField]
    public float criticalChanceModifier = 0.0f;
    [SerializeField]
    public int attackRangeModifier = 0;
    [SerializeField]
    public float criticalDamageModifier = 0;

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
    [SerializeField]
    public int health = 0;

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


    //Other Variables
    [Header("TURN VARIABLES")]
    public bool isTurnActive = false;
    
    public int actionPoints;



    // Start is called before the first frame update
    void Start()
    {
        AddPlayerToTeamList();

        weaponInstanceBelt = new GameObject[weaponPrefabBelt.Length];



        int weaponIndex = 0;

        foreach (var weapon in weaponPrefabBelt)
        {
            weaponInstanceBelt[weaponIndex] = Instantiate(weapon, this.transform);
            if (weaponInstanceBelt[weaponIndex].GetComponent<WeaponBaseClass>().isCurrent)
            {
                weaponInstanceBelt[weaponIndex].transform.localPosition = weaponGripPlace.transform.localPosition;
                _currentWeaponIndex = weaponIndex;
                _weaponClass = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponClass;
                _weaponRange = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponRange + attackRangeModifier;
            }
            else
            {
                weaponInstanceBelt[weaponIndex].transform.localPosition = weaponHolsters[weaponIndex].transform.localPosition;
            }
            weaponIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurnActive)
        {
            if (CameraTargetManager.instance.isLocked)//
            {
                //CameraTargetManager.instance.transform.parent = this.transform;//
                //CameraTargetManager.instance.transform.position = this.transform.position;//

                CameraTargetManager.instance.followTransform = transform;
            }

            if (_isMoveMode)
            {
                if (_listOfScannedEnemies.Count > 0)
                {
                    foreach (var enemy in _listOfScannedEnemies)
                    {
                        enemy.canBeAttacked = false;
                    }
                }

                _listOfScannedEnemies.Clear();

                if (!isMoving)
                {
                    ActivateMouseToMovement();                    
                }
                else
                {
                    Move();                    
                }

                if (!isTilesFound)
                {
                    if (this.GetComponent<CharacterStats>().actionPoints <= 0)
                    {
                        TurnManager.instance.PlayerCharacterActionDepleted((CharacterStats)this);
                        return;
                    }
                    GridManager.instance.CalculateAvailablePath(this.gameObject);
                }

                
                
            }

            if (_isCombatMode)
            {

                if (!isAttackRangeFound)
                {
                    _weaponRange = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponRange + attackRangeModifier;
                    GridManager.instance.CalculateAttackPath(this.gameObject);
                    ScanForEnemies();
                }

                ActivateMouseToAttack();

            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                ChangeMode();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_isCombatMode)
                {
                    ChangeWeapon();
                }
            }
        }
    }

    //public void AddPlayerToTeamList()
    //{
    //    TurnManager.instance.playerTeam.Add((IPlayerTeam)this);
    //}

    private void OnMouseDown()
    {
        CameraTargetManager.instance.followTransform = transform;
    }
}
