using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public enum WeaponClass
{
    Melee,
    Gun,
    Rifle,
    MiniGun,
}
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


    [SerializeField]
    private List<EnemyBaseClass> _listOfScannedEnemies;

    public GameObject weaponGripPlace;

    public delegate void OnAttack(WeaponBaseClass weaponBaseClass);
    public static event OnAttack EventAttackTarget;

    //TEST 
    public GameObject weaponPrefab;
    public GameObject weaponInstance;

    // Start is called before the first frame update
    void Start()
    {
        weaponInstance = Instantiate(weaponPrefab, this.transform);
        //weaponPrefab.transform.localPosition = weaponGripPlace.transform.localPosition;
        weaponInstance.transform.localPosition = weaponGripPlace.transform.localPosition;

        //Instantiate(weaponPrefab);
        //weaponPrefab.transform.position = weaponGripPlace.transform.position;
        //weaponPrefab.transform.SetParent(this.transform);
        //weaponPrefab.transform.parent = this.transform;
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
            switch (_weaponClass)
            {
                case WeaponClass.Melee:
                    _weaponRange = 1;
                    break;
                case WeaponClass.Gun:
                    _weaponRange = 5;
                    break;
                case WeaponClass.Rifle:
                    _weaponRange = 7;
                    break;
                case WeaponClass.MiniGun:
                    _weaponRange = 4;
                    break;
                default:
                    Debug.Log("No Weapon Selected");
                    break;

            }

            if (!isAttackRangeFound)
            {
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
        int[] weaponArray = { (int)WeaponClass.Melee, (int)WeaponClass.Gun, (int)WeaponClass.Rifle, (int)WeaponClass.MiniGun };
        

        if (index < weaponArray.Length-1)
        {
            index++;
        }
        else
        {
            index = 0;
        }

        _weaponClass = (WeaponClass)index;
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
        weaponInstance.GetComponent<WeaponBaseClass>().Attack(this, enemy);
    }



}
