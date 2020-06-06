using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraBehavior : MonoBehaviour
{
    //public float cameraArm;
    public Vector3 cameraArm;
    public GameObject cameraFocusPoint;

    float distance;

    public Transform target;
    public float smoothSpeed = 0.125f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 desiredPosition = target.position + cameraArm;
        transform.position = desiredPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this.transform.position = cameraFocusPoint.transform.position 
        //distance = Vector3.Distance(this.transform.position, target.transform.position);// = cameraArm;

        //transform.position = target.position + cameraArm;
        //Vector3 desiredPosition = target.position +  cameraArm;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        //transform.position = smoothedPosition;
       
        //transform.position = desiredPosition;
        
        //transform.LookAt(target);

        if (Input.GetKey(KeyCode.E))
        {
            this.transform.RotateAround(target.position, Vector3.up, 10.0f);
            cameraArm = this.transform.position - target.position;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.RotateAround(target.position, Vector3.up, -10.0f);
            cameraArm = this.transform.position - target.position;
        }

        //transform.position = target.position + cameraArm;


        Vector3 desiredPosition = target.position + cameraArm;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
