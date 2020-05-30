using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public enum WeaponClass
//{
//    Melee,
//    Gun,
//    Rifle,
//    MiniGun,
//}
public class CharacterCombat : CharacterMove
{
    //[SerializeField]
    //public bool _isCombatMode = false;
    //[SerializeField]
    //public bool _isMoveMode = true;
    [SerializeField]
    private WeaponClass _weaponClass;// = WeaponClass.Gun;
    //[SerializeField]
    //private int _weaponRange;
    private int _currentWeaponIndex;


    [SerializeField]
    private List<EnemyBaseClass> _listOfScannedEnemies;

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
        weaponInstanceBelt = new GameObject[weaponPrefabBelt.Length];
        

        int weaponIndex = 0;

        foreach (var weapon in weaponPrefabBelt)
        {
            weaponInstanceBelt[weaponIndex] = Instantiate(weapon, this.transform);
            if (weaponInstanceBelt[weaponIndex].GetComponent<WeaponBaseClass>().isCurrent)
            {
                weaponInstanceBelt[weaponIndex].transform.localPosition = weaponGripPlace.transform.localPosition;
                _currentWeaponIndex = weaponIndex;
                _weaponRange = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponRange;
            }
            else
            {
                Debug.Log("CUUU");
                weaponInstanceBelt[weaponIndex].transform.localPosition = weaponHolsters[weaponIndex].transform.localPosition;
            }
                weaponIndex++;
        }

        


    }

    

    // Update is called once per frame
    void Update()
    {
        if (_isMoveMode)
        {
            //_isCombatMode = false;

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
                GridManager.instance.CalculateAvailablePath(this.gameObject);
            }           
        }
        
        if (_isCombatMode)
        {
            //switch (_weaponClass)
            //{
            //    case WeaponClass.Melee:
            //        _weaponRange = 1;
            //        break;
            //    case WeaponClass.Gun:
            //        _weaponRange = 5;
            //        break;
            //    case WeaponClass.Rifle:
            //        _weaponRange = 7;
            //        break;
            //    case WeaponClass.MiniGun:
            //        _weaponRange = 4;
            //        break;
            //    default:
            //        Debug.Log("No Weapon Selected");
            //        break;

            //}

            

            if (!isAttackRangeFound)
            {
                _weaponRange = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponRange;
                GridManager.instance.CalculateAttackPath(this.gameObject);
                ScanForEnemies();
            }

            ActivateMouseToAttack();

        }
        
        if(Input.GetKeyDown(KeyCode.H))
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

    int index = 0;

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
        //_weaponRange = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponRange;

        isAttackRangeFound = false;

        GridManager.instance.ClearSelectableTiles();
    }

    public void ScanForEnemies()
    {
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
        //EventAttackTarget();
        Debug.Log("The Enemy " + enemy.name + " is Being Attacked By " + this.gameObject.name + " Using " + _weaponClass);
        //Debug.DrawRay(weaponInstance.GetComponent<WeaponBaseClass>().weaponFirePoint.transform.position, enemy.transform.position, Color.red, 1);
        transform.LookAt(enemy.transform);
        //weaponInstance.GetComponent<WeaponBaseClass>().Attack(this, enemy);
        weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().Attack(this, enemy);
    }



}
