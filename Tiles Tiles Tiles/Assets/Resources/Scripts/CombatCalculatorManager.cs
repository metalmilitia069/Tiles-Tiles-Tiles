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
        _weaponCalculatedBaseDamage = weaponRef.calculatedBaseDamage;
        _weaponSuccessShotProbability = weaponRef.successShotProbability;
        _weaponCriticalChance = weaponRef.weaponCriticalChance;
        _weaponCriticalDamage = weaponRef.weaponCriticalDamage;
    }

    public void GatherEnemyDefenseStats(EnemyBaseClass enemyRef)
    {
        _enemyArmorNormal = enemyRef.armorNormal;
        _enemyArmorBlindage = enemyRef.armorBlindage;
        _enemyDodgeChance = enemyRef.dodgeChance;
    }

    public void GatherPlayerAttackStats(CharacterStats characterRef)
    {
        _playerDamageModifier = characterRef.damageModifier;    
        _playerCriticalChanceModifier = characterRef.criticalChanceModifier;        
        _playerCriticalDamageModifier = characterRef.criticalDamageModifier;
    }

    public void FinalAttackCalculation()
    {

    }
}
