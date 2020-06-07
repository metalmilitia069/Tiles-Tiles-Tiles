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
        
    }
}
