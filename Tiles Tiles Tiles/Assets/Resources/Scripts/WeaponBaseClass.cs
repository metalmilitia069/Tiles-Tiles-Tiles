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
public class WeaponBaseClass : MonoBehaviour
{    
    public GameObject weaponFirePoint;
    public GameObject weaponFireDirection;
    public GameObject projectilePrefab;

    public WeaponClass weaponClass;

    public bool isCurrent = false;

    //Weapon Stats Calculations
    [Header("WEAPON STATS")]
    public int weaponRange;//ok
    public int optimalRange;//ok
    public int minDamage;//ok
    public int maxDamage;//ok
    public int calculatedBaseDamage;//->    
    public float distanceFromTarget;//ok
    

    public float weaponCriticalChance;//->
    public float successShotProbability = 1;//->
    public float damagePenalty = 1.0f;//ok    
    public float weaponCriticalDamage;//->

    //[Header("COVER DAMAGE REDUCTION")]
    private bool isFullCover = false;
    private bool isHalfCover = false;

    //Weapon Stats Bullet Behavior
    [Header("WEAPON BEHAVIOR")]
    public bool hasSpread = false;
    public float fireRate;





    // Start is called before the first frame update
    void Start()
    {
        //weaponClass
        switch (weaponClass)
        {
            case WeaponClass.Melee:
                weaponRange = 1;
                optimalRange = 1;
                break;
            case WeaponClass.Gun:
                weaponRange = 5;
                optimalRange = 3;
                break;
            case WeaponClass.Rifle:
                weaponRange = 7;
                optimalRange = 5;
                break;
            case WeaponClass.MiniGun:
                weaponRange = 4;
                optimalRange = 3;
                break;
            default:
                Debug.Log("No Weapon Selected");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GatherWeaponAttackStats(CharacterStats character, EnemyBaseClass enemy)//(CharacterCombat character, EnemyBaseClass enemy)
    {
        transform.LookAt(enemy.transform);
        Ray ray = new Ray(weaponFirePoint.transform.position, transform.forward * 100);//enemy.transform.position);//Input.mousePosition);
        Debug.DrawRay(weaponFirePoint.transform.position, transform.forward * 100, Color.red, 2);//enemy.transform.position, Color.red, 1);//Input.mousePosition, Color.red, 1);
        
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, Vector3.Distance(this.transform.position, enemy.transform.position));//Mathf.Infinity);//enemy.transform.position.magnitude);
        
        


        ////Reset Odds
        //successShotProbability = 1;
        //damagePenalty = 1.0f;

        foreach (var hit in hits)
        {
            TileModifier cover = hit.collider.GetComponent<TileModifier>();
            if (cover && cover.isCover)
            {
                if (cover.isHalfCover )
                {
                    //Debug.Log("Hit HALF Cover");
                    successShotProbability -= cover.halfCoverPenalty;
                    isHalfCover = true;                    
                }
                else if (cover.isFullCover )//&& !isCoverComputed)
                {
                    //Debug.Log("Hit FULL Cover");
                    successShotProbability -= cover.fullCoverPenalty;
                    isFullCover = true;                    
                }
            }
            EnemyBaseClass enemyclass = hit.collider.GetComponent<EnemyBaseClass>();
            if (enemyclass)
            {
                //Debug.Log("Hit Enemy!!!");
                distanceFromTarget = Vector3.Distance(character.transform.position, enemy.transform.position);
                //Debug.Log("Distance From The Target: " + distanceFromTarget);
                if (optimalRange +1 >= distanceFromTarget && optimalRange -1 <= distanceFromTarget)//
                {
                    damagePenalty -= 0f;
                    successShotProbability -= 0f;
                }
                else
                {
                    damagePenalty -= 0.2f;
                    successShotProbability -= .1f;
                }
                CalculateBaseDamage();
            }
        }
    }

    public void CalculateBaseDamage()
    {
        //Debug.Log("damage penalty = " + damagePenalty);
        calculatedBaseDamage = Random.Range(minDamage, maxDamage + 1);
        //Debug.Log("calculated Base Damage = " + calculatedBaseDamage);

        calculatedBaseDamage = (int)(calculatedBaseDamage * damagePenalty);
        //Debug.Log("calculated Base Damage * Range Penalty = " + calculatedBaseDamage);

        if (isHalfCover)
        {
            calculatedBaseDamage = (int)(calculatedBaseDamage * 0.80f);
            isHalfCover = false;
            //Debug.Log("isHalfCover");

        }
        else if (isFullCover)
        {
            calculatedBaseDamage = (int)(calculatedBaseDamage * 0.50f);
            isFullCover = false;
            //Debug.Log("isFullCover");
        }
        //Debug.Log("calculated Base Damage * Penalty * CoverAbsortion = " + calculatedBaseDamage);

        
        CombatCalculatorManager.instance.GatherWeaponAttackStats(this);



        //Debug.Log("Shot Success Chance = " + successShotProbability * 100 + "%");

        //Debug.Log("Shot Critical Chance = " + weaponCriticalChance * 100 + "%");

        //Debug.Log("Shot Critical Damage Rate = " + weaponCriticalDamage * 100 + "%");


        //Reset Odds
        successShotProbability = 1;
        damagePenalty = 1.0f;
    }

    
}
