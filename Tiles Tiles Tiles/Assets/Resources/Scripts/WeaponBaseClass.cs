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
    public int weaponRange;
    public int optimalRange;
    public int minDamage;
    public int maxDamage;
    public int calculatedBaseDamage;//
    public int expectedDamage;//
    public int calculatedDamage;//
    public float distanceFromTarget;
    

    public float criticalChance;//
    public float successShotProbability = 1;//
    public float damagePenalty;
    public float shotProbabilityPenalty;
    public float weaponCriticalDamage;//

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

    public void Attack(CharacterStats character, EnemyBaseClass enemy)//(CharacterCombat character, EnemyBaseClass enemy)
    {
        transform.LookAt(enemy.transform);
        Ray ray = new Ray(weaponFirePoint.transform.position, transform.forward * 100);//enemy.transform.position);//Input.mousePosition);
        Debug.DrawRay(weaponFirePoint.transform.position, transform.forward * 100, Color.red, 2);//enemy.transform.position, Color.red, 1);//Input.mousePosition, Color.red, 1);
        
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, Mathf.Infinity);

        foreach (var hit in hits)
        {
            TileModifier cover = hit.collider.GetComponent<TileModifier>();
            if (cover && cover.isCover)
            {
                if (cover.isHalfCover)
                {
                    Debug.Log("Hit HALF Cover");
                    successShotProbability -= cover.halfCoverPenalty;
                }
                else if (cover.isFullCover)
                {
                    Debug.Log("Hit FULL Cover");
                    successShotProbability -= cover.fullCoverPenalty;
                }
            }
            EnemyBaseClass enemyclass = hit.collider.GetComponent<EnemyBaseClass>();
            if (enemyclass)
            {
                Debug.Log("Hit Enemy!!!");
                distanceFromTarget = Vector3.Distance(character.transform.position, enemy.transform.position);
                Debug.Log("Distance From The Target: " + distanceFromTarget);
                CalculateBaseDamage();
            }
        }
    }

    public void CalculateBaseDamage()
    {
        calculatedBaseDamage = Random.Range(minDamage, maxDamage + 1);
        Debug.Log("calculated Base Damage = " + calculatedBaseDamage);
    }
}
