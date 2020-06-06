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



    public bool isLocked = true;
    public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //isLocked = false;
            //this.transform.parent = null;
            UnlockCamera();
            this.transform.position += (Vector3.back * speed * Time.deltaTime);            
        }
        if (Input.GetKey(KeyCode.A))
        {
            UnlockCamera();
            this.transform.position += (Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            UnlockCamera();
            this.transform.position += (Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            UnlockCamera();
            this.transform.position += (Vector3.forward * speed * Time.deltaTime);
        }

        //if (Input.GetKey(KeyCode.E))
        //{
        //    this.transform.rotation
        //}
    }

    public void UnlockCamera()
    {
        isLocked = false;
        this.transform.parent = null;        
    }


}
