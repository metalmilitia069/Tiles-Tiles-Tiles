using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraBehavior : MonoBehaviour
{
    //public float cameraArm;
    public Vector3 cameraArm;
    public GameObject cameraFocusPoint;


    public Transform target;
    public float smoothSpeed = 0.125f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this.transform.position = cameraFocusPoint.transform.position 
        //Vector3.Distance(this.transform.position, cameraFocusPoint.transform.position) = cameraArm;

        //transform.position = target.position + cameraArm;
        Vector3 desiredPosition = target.position + cameraArm;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        //transform.LookAt(target);
    }
}
