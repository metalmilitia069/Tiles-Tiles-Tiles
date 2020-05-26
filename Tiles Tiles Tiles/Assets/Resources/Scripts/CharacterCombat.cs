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
    private bool _isCombatMode = false;
    [SerializeField]
    private bool _isMoveMode = true;
    [SerializeField]
    private WeaponClass weaponClass;
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
            _isCombatMode = false;

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
            _isMoveMode = false;
        }


        



    }


}
