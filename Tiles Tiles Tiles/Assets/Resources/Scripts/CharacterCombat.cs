using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WeaponClass
{
    Melee,
    Gun,
    Rifle,
    MiniGun,
}
public class CharacterCombat : CharacterMove
{
    [SerializeField]
    public bool _isCombatMode = false;
    [SerializeField]
    public bool _isMoveMode = true;
    [SerializeField]
    private WeaponClass _weaponClass;// = WeaponClass.Gun;
    [SerializeField]
    private int _weaponRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoveMode)
        {
            //_isCombatMode = false;

            if (!isMoving)
            {
                ActivateMouse();
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
                    _weaponRange = 2;
                    break;
                case WeaponClass.Gun:
                    _weaponRange = 5;
                    break;
                case WeaponClass.Rifle:
                    _weaponRange = 5;
                    break;
                case WeaponClass.MiniGun:
                    _weaponRange = 5;
                    break;
                default:
                    Debug.Log("No Weapon Selected");
                    break;

            }

            
        }


        
        if(Input.GetKeyDown(KeyCode.H))
        {
            ChangeMode();            
        }




    }

    public void ChangeMode()
    {
        if(_isMoveMode)
        {
            _isCombatMode = true;
            _isMoveMode = false;
            
        }
        else if (_isCombatMode)
        {
            _isCombatMode = false;
            _isMoveMode = true;            
            isTilesFound = false;
        }

        GridManager.instance.ClearSelectableTiles();
    }

    //public void 


}
