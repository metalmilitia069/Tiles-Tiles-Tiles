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

    public delegate void OnAttack(WeaponBaseClass weaponBaseClass);
    public static event OnAttack EventAttackTarget;

    //TEST 
    public GameObject[] weaponPrefabBelt;
    public GameObject[] weaponInstanceBelt;
    public GameObject[] weaponHolsters;
    public int weaponBeltSize = 4;

    // Start is called before the first frame update
    void Start()
    {
        //weaponInstanceBelt = new GameObject[weaponPrefabBelt.Length];
        

        //int weaponIndex = 0;

        //foreach (var weapon in weaponPrefabBelt)
        //{
        //    weaponInstanceBelt[weaponIndex] = Instantiate(weapon, this.transform);
        //    if (weaponInstanceBelt[weaponIndex].GetComponent<WeaponBaseClass>().isCurrent)
        //    {
        //        weaponInstanceBelt[weaponIndex].transform.localPosition = weaponGripPlace.transform.localPosition;
        //        _currentWeaponIndex = weaponIndex;
        //        _weaponClass = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponClass;
        //        _weaponRange = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponRange;
        //    }
        //    else
        //    {                
        //        weaponInstanceBelt[weaponIndex].transform.localPosition = weaponHolsters[weaponIndex].transform.localPosition;
        //    }
        //        weaponIndex++;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (_isMoveMode)
        //{
        //    //_isCombatMode = false;
        //    if (_listOfScannedEnemies.Count > 0)
        //    {
        //        foreach (var enemy in _listOfScannedEnemies)
        //        {
        //            enemy.canBeAttacked = false;
        //        }
        //    }

        //    _listOfScannedEnemies.Clear();

        //    if (!isMoving)
        //    {
        //        ActivateMouseToMovement();
        //    }
        //    else
        //    {
        //        Move();
        //    }

        //    if (!isTilesFound)
        //    {
        //        GridManager.instance.CalculateAvailablePath(this.gameObject);
        //    }           
        //}
        
        //if (_isCombatMode)
        //{
            
        //    if (!isAttackRangeFound)
        //    {
        //        _weaponRange = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponRange;
        //        GridManager.instance.CalculateAttackPath(this.gameObject);
        //        ScanForEnemies();
        //    }

        //    ActivateMouseToAttack();

        //}
        
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        //    ChangeMode();            
        //}

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    if (_isCombatMode)
        //    {
        //        ChangeWeapon();
        //    }
        //}
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
        if(Input.GetMouseButtonDown(0))
        {
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
                            Attack(enemy);
                        }
                    }
                }
            }
        }
    }

    public void Attack(EnemyBaseClass enemy)
    {       
        Debug.Log("The Enemy " + enemy.name + " is Being Attacked By " + this.gameObject.name + " Using " + _weaponClass);
        
        transform.LookAt(enemy.transform);        
        weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().Attack((CharacterStats)this, enemy);
    }
}
