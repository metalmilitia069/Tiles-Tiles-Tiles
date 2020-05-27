using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseClass : MonoBehaviour
{
    public GameObject weaponPrefab;
    public GameObject weaponBody;
    public GameObject weaponGripPlace;
    public GameObject weaponFirePoint;
    public GameObject weaponGripReal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("WeaponPrefab Coord is: " + weaponPrefab.transform.position);
            Debug.Log("WeaponBody Coord is: " + weaponBody.transform.position);
            Debug.Log("WeaponGripPlace Coord is: " + weaponGripPlace.transform.position);
            Debug.Log("WeaponGripReal Coord is: " + weaponGripReal.transform.position);
            Debug.Log("WeaponFirePoint Coord is: " + weaponFirePoint.transform.position);
        }
    }
}
