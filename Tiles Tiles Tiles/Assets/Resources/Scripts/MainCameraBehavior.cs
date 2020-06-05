using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraBehavior : MonoBehaviour
{
    public float cameraArm;
    public Vector3 direction;
    public GameObject cameraFocusPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = cameraFocusPoint.transform.position 
        //Vector3.Distance(this.transform.position, cameraFocusPoint.transform.position) = cameraArm;
    }
}
