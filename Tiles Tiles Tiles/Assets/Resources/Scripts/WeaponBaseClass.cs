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
public class WeaponBaseClass : MonoBehaviour
{
    //public GameObject weaponPrefab;
    //public GameObject weaponBody;
    //public GameObject weaponGripPlace;
    //public GameObject weaponGripReal;
    public GameObject weaponFirePoint;
    public GameObject projectilePrefab;

    public WeaponClass weaponClass;

    //Weapon Stats Calculations
    
    public int range;
    public int optimalRange;
    public int minDamage;
    public int maxDamage;
    public int expectedDamage;
    public int calculatedDamage;
    

    public float criticalChange;
    public float successShotProbabilty;
    public float penalty;

    //Weapon Stats Bullet Behavior

    public bool hasSpread = false;
    public float fireRate;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
