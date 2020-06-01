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
    private int _playerAttackRangeModifier;
    [SerializeField]
    private float _playerCriticalDamageModifier;

    //TODO: Elemental Attack
    [Header("PLAYER GATHERED ELEMENTAL ATTACK STATS")]
    [SerializeField]
    private int _elementalDmgFire = 0;
    [SerializeField]
    private int _elementalDmgElectricity = 0;
    [SerializeField]
    private int _elementalDmgCold = 0;
    [SerializeField]
    private int _elementalDmgPoison = 0;


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

    public void GatherEnemyDefenseStats()
    {

    }

    public void GatherPlayerAttackStats(CharacterStats characterRef)
    {
        _playerDamageModifier = characterRef.damageModifier;
    
        _playerCriticalChanceModifier = characterRef.criticalChanceModifier;
        
        _playerAttackRangeModifier = characterRef.attackRangeModifier;
        
        _playerCriticalDamageModifier = characterRef.criticalDamageModifier;
    }
}
