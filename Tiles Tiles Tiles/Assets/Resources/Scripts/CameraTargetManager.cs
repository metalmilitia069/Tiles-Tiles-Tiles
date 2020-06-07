using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetManager : MonoBehaviour
{
    #region Singleton

    public static CameraTargetManager instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is Another CameraTargetManager in the Scene!!!");
            Destroy(this.gameObject);
        }

        instance = this;
    }

    #endregion


    public Transform followTransform;
    public Transform cameraTransform;

    public bool isLocked = true;
    public float speed = 5.0f;
    public float movementTime;
    public float rotationAmount;

    public Vector3 zoomAmount;
    public Vector3 newZoom;
    public Vector3 newPosition;
    public Quaternion newRotation;



    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    // Start is called before the first frame update
    void Start()
    {
        //newPosition = transform.position;
        newRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        //else
        //{
            if (Input.GetKey(KeyCode.W))
            {
                //isLocked = false;
                //this.transform.parent = null;
                UnlockCamera();
                this.transform.position += (this.transform.forward * speed * Time.deltaTime);//(Vector3.back * speed * Time.deltaTime);

                //newPosition += (this.transform.forward * speed * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                UnlockCamera();
                this.transform.position += (-this.transform.right * speed * Time.deltaTime);//(Vector3.right * speed * Time.deltaTime);
                //newPosition += (-this.transform.right * speed * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                UnlockCamera();
                this.transform.position += (this.transform.right * speed * Time.deltaTime);//(Vector3.left * speed * Time.deltaTime);
                //newPosition += (this.transform.right * speed * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                UnlockCamera();
                this.transform.position += (-this.transform.forward * speed * Time.deltaTime);//(Vector3.forward * speed * Time.deltaTime);
                //newPosition += (-this.transform.forward * speed * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
            }

            //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);

            if (Input.GetKey(KeyCode.E))
            {
                //this.transform.Rotate(Vector3.up, 10);
                newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                //this.transform.Rotate(Vector3.up, -10);
                newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }

            if (Input.GetKey(KeyCode.R))
            {
                newZoom += zoomAmount;
            }

            if (Input.GetKey(KeyCode.T))
            {
                newZoom -= zoomAmount;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * speed);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * speed);

            //if (Input.GetKey(KeyCode.E))
            //{
            //    this.transform.rotation
            //}

            HandleMouseInput();

        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }


    }

    public void UnlockCamera()
    {
        isLocked = false;
        //this.transform.parent = null;
        followTransform = null;
    }

    public void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                this.transform.position = transform.position + (dragStartPosition - dragCurrentPosition);
            }
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }


}
