using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileType : Tile
{
    [SerializeField]
    private float _latterHeight = 5;
    [SerializeField]
    private Vector3 _latterHeightVector;

    [SerializeField]
    private bool _isForwardDirection = false;
    [SerializeField]
    private bool _isBackwardDirection = false;
    [SerializeField]
    private bool _isRightDirection = false;
    [SerializeField]
    private bool _isLeftDirection = false;
    [SerializeField]
    private Vector3 _direction;
    //protected bool isLatter = false;
    //protected bool isCover = false;
    // Start is called before the first frame update
    void Start()
    {
        GridManager.EventScanTilesUpdate += ScanTiles;
        ScanTiles();        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.up);

        if (isCurrent)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (isTarget)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (isSelectable)
        {
            GetComponent<Renderer>().material.color = Color.blue;

            if (isLatter)
            {
                GetComponent<Renderer>().material.color = Color.yellow;                
            }
            else if (isCover)
            {
                GetComponent<Renderer>().material.color = Color.green;
            }

            if (isAttackble)
            {
                GetComponent<Renderer>().material.color = Color.cyan;
            }
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }    
}
