using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCalculatorManager : MonoBehaviour
{
    [Header("WEAPON GATHERED STATS")]
    [SerializeField]
    private int _weaponCalculatedBaseDamage;
    [SerializeField]
    private float _weaponSuccessShotProbability;
    [SerializeField]
    private float _weaponCriticalChance;
    [SerializeField]
    private float _weaponCriticalDamage;
    
    [Header("ATTACK STATS")]

    [Header("PLAYER GATHERED ATTACK STATS")]
    //Attack Stats
    [SerializeField]
    private int _playerDamageModifier;
    [SerializeField]
    private float _playerCriticalChanceModifier;    
    [SerializeField]
    private float _playerCriticalDamageModifier;

    //TODO: Elemental Attack
    [Header("PLAYER GATHERED ELEMENTAL ATTACK STATS")]
    [SerializeField]
    private int _playerElementalDmgFire;
    [SerializeField]
    private int _playerElementalDmgElectricity;
    [SerializeField]
    private int _playerElementalDmgCold;
    [SerializeField]
    private int _playerElementalDmgPoison;

    [Header("DEFENSE STATS")]

    [Header("ENEMY GATHERED DEFENSE STATS")]
    //Defense Stats
    [SerializeField]
    private int _enemyArmorNormal;
    [SerializeField]
    private int _enemyArmorBlindage;
    [SerializeField]
    private float _enemyDodgeChance;

    //TODO: Elemental Defense
    [Header("ELEMENTAL DEFENSE STATS")]
    [SerializeField]
    private int _enemyElementalDefFire;
    [SerializeField]
    private int _enemyElementalDefElectricity;
    [SerializeField]
    private int _enemyElementalDefCold;
    [SerializeField]
    private int _enemyElementalDefPoison;

    [Header("MANAGER CALCULATED VARIABLES")]
    [SerializeField]
    private int _finalDamage;
    [SerializeField]
    private float _finalCriticalProbability;
    [SerializeField]
    private WeaponBaseClass _cachedWeapon;

    #region Singleton

    public static CombatCalculatorManager instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Theres More than One CombatCalculatorManager in the Scene!!!");
            Destroy(this.gameObject);
        }

        instance = this;

        //DontDestroyOnLoad(this.gameObject);        
    }

    #endregion




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GatherWeaponAttackStats(WeaponBaseClass weaponRef)
    {
        _cachedWeapon = weaponRef;

        _weaponCalculatedBaseDamage = weaponRef.calculatedBaseDamage;//
        _weaponSuccessShotProbability = weaponRef.successShotProbability;//
        _weaponCriticalChance = weaponRef.weaponCriticalChance;//
        _weaponCriticalDamage = weaponRef.weaponCriticalDamage;//
    }

    public void GatherEnemyDefenseStats(EnemyBaseClass enemyRef)
    {
        _enemyArmorNormal = enemyRef.armorNormal;//
        _enemyArmorBlindage = enemyRef.armorBlindage;//
        _enemyDodgeChance = enemyRef.dodgeChance;//
    }

    public void GatherPlayerAttackStats(CharacterStats characterRef)
    {
        _playerDamageModifier = characterRef.damageModifier;//   
        _playerCriticalChanceModifier = characterRef.criticalChanceModifier;//        
        _playerCriticalDamageModifier = characterRef.criticalDamageModifier;//
    }

    public void PlayerFinalAttackCalculation(EnemyBaseClass enemy)
    {
        float finalAttackProbability = _weaponSuccessShotProbability - _enemyDodgeChance;
        float diceRoll = Random.Range(0.0f, 1.0f);
        bool success = (diceRoll <= finalAttackProbability);
        if (success)
        {
            int finalDamage = _weaponCalculatedBaseDamage + _playerDamageModifier - _enemyArmorNormal - _enemyArmorBlindage;
            _finalDamage = finalDamage;
            float finalCriticalProbability = _weaponCriticalChance + _playerCriticalChanceModifier;
            _finalCriticalProbability = finalCriticalProbability;
            float diceRoll02 = Random.Range(0.0f, 1.0f);
            bool success02 = (diceRoll02 <= finalCriticalProbability);

            if (success02)
            {                 
                finalDamage = (finalDamage * ((int)(_weaponCriticalDamage + _playerCriticalDamageModifier)));
                _finalDamage = finalDamage;
                Debug.Log("Critical Shot Success!!!");
            }
        }
        else
        {
            Debug.Log("Shot MISSED!!!");
            _finalDamage = 0;
        }

        Debug.Log("Calculated Critical Chance = " + _finalCriticalProbability);
        Debug.Log("FINAL DAMAGE ON ENEMY = " + _finalDamage);

        ResetCalculaterVariables();
    }

    public string DisplayShotChance()
    {
        float finalAttackProbability = _weaponSuccessShotProbability - _enemyDodgeChance;
        int weaponDamageMedian = (int)((_cachedWeapon.maxDamage + _cachedWeapon.minDamage) / 2);
        int finalDamage = weaponDamageMedian + _playerDamageModifier - _enemyArmorNormal - _enemyArmorBlindage;
        string probabilityText = ("Shot Success Chance = " + finalAttackProbability * 100 + "%  || Damage Preview = " + finalDamage);
        return probabilityText;
    }

    public void ResetCalculaterVariables()
    {
        _finalDamage = default;
        _finalCriticalProbability = default;
    }


}
