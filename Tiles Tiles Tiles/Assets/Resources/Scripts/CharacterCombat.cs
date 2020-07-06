using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterCombat : CharacterMove
{    
    [SerializeField]
    protected WeaponClass _weaponClass;// = WeaponClass.Gun;    
    protected int _currentWeaponIndex;


    [SerializeField]
    protected List<EnemyBaseClass> _listOfScannedEnemies;

    public GameObject weaponGripPlace;

    //public delegate void OnAttack(WeaponBaseClass weaponBaseClass);
    //public static event OnAttack EventAttackTarget;

    //TEST 
    public GameObject[] weaponPrefabBelt;
    public GameObject[] weaponInstanceBelt;
    public GameObject[] weaponHolsters;
    public int weaponBeltSize = 4;

    // Start is called before the first frame update
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {        
    }

    public void ChangeMode()
    {
        if(_isMoveMode)
        {
            _isCombatMode = true;
            _isMoveMode = false;
            isAttackRangeFound = false;
            foreach (var item in GridManager.instance.listOfAllTilesInLevel)
            {
                item.isMoveMode = false;
            }
            
        }
        else if (_isCombatMode)
        {
            _isCombatMode = false;
            _isMoveMode = true;            
            isTilesFound = false;
            foreach (var item in GridManager.instance.listOfAllTilesInLevel)
            {
                item.isMoveMode = true;
            }
        }

        GridManager.instance.ClearSelectableTiles();
    }

    public void ChangeWeapon()
    {   
        weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().isCurrent = false;
        weaponInstanceBelt[_currentWeaponIndex].transform.localPosition = weaponHolsters[_currentWeaponIndex].transform.localPosition;

        if (_currentWeaponIndex < weaponInstanceBelt.Length - 1)
        {
            _currentWeaponIndex++;
        }
        else
        {
            _currentWeaponIndex = 0;
        }

        weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().isCurrent = true;
        weaponInstanceBelt[_currentWeaponIndex].transform.localPosition = weaponGripPlace.transform.localPosition;

        _weaponClass = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponClass;        

        isAttackRangeFound = false;

        GridManager.instance.ClearSelectableTiles();
    }

    public void ScanForEnemies()
    {
        if (_listOfScannedEnemies.Count > 0)
        {
            foreach (var enemy in _listOfScannedEnemies)
            {
                enemy.canBeAttacked = false;
            }
        }
        _listOfScannedEnemies.Clear();

        foreach (var item in GridManager.instance.listOfSelectableTiles)
        {
            RaycastHit hit;
            if (Physics.Raycast(item.transform.position, Vector3.up, out hit, 1))
            {
                EnemyBaseClass enemyPlaceHolder = hit.collider.GetComponent<EnemyBaseClass>();
                if (enemyPlaceHolder)
                {
                    _listOfScannedEnemies.Add(enemyPlaceHolder);
                    enemyPlaceHolder.canBeAttacked = true;

                }
            }
        }
    }

    public void ActivateMouseToAttack()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                EnemyBaseClass enemyPlaceHolder = hit.collider.GetComponent<EnemyBaseClass>();
                if (enemyPlaceHolder)
                {
                    foreach (var enemy in _listOfScannedEnemies)
                    {
                        if (enemy == enemyPlaceHolder)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                Attack(enemy);
                            }
                            else
                            {
                                ShowProbability(enemy);
                            }
                        }
                    }
                }
            }
        //}
    }

    public void Attack(EnemyBaseClass enemy)
    {       
        //Debug.Log("The Enemy " + enemy.name + " is Being Attacked By " + this.gameObject.name + " Using " + _weaponClass);
        transform.LookAt(enemy.transform);        
        weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().GatherWeaponAttackStats((CharacterStats)this, enemy);
        CombatCalculatorManager.instance.GatherEnemyDefenseStats(enemy);
        CombatCalculatorManager.instance.GatherPlayerAttackStats((CharacterStats)this);
        CombatCalculatorManager.instance.PlayerFinalAttackCalculation(enemy);




        this.GetComponent<CharacterStats>().actionPoints--;
        if (this.GetComponent<CharacterStats>().actionPoints <= 0)
        {
            TurnManager.instance.PlayerCharacterActionDepleted((CharacterStats)this);
        }

    }

    public void ShowProbability(EnemyBaseClass enemy)
    {
        transform.LookAt(enemy.transform);
        weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().GatherWeaponAttackStats((CharacterStats)this, enemy);
        CombatCalculatorManager.instance.GatherEnemyDefenseStats(enemy);
        CombatCalculatorManager.instance.GatherPlayerAttackStats((CharacterStats)this);
        CombatCalculatorManager.instance.DisplayShotChance();//(weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>());
        enemy.ShowProbability();
    }
}
