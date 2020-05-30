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
    //public GameObject weaponPrefab;
    //public GameObject weaponBody;
    //public GameObject weaponGripPlace;
    //public GameObject weaponGripReal;
    public GameObject weaponFirePoint;
    public GameObject weaponFireDirection;
    public GameObject projectilePrefab;

    public WeaponClass weaponClass;

    public bool isCurrent = false;

    //Weapon Stats Calculations
    
    public int weaponRange;
    public int optimalRange;
    public int minDamage;
    public int maxDamage;
    public int expectedDamage;
    public int calculatedDamage;
    

    public float criticalChange;
    public float successShotProbabilty = 1;
    public float penalty;

    //Weapon Stats Bullet Behavior

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
                break;
            case WeaponClass.Gun:
                weaponRange = 5;
                break;
            case WeaponClass.Rifle:
                weaponRange = 7;
                break;
            case WeaponClass.MiniGun:
                weaponRange = 4;
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

    public void Attack(CharacterCombat character, EnemyBaseClass enemy)
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
                     
                }
            }
        }
        

    }
}
