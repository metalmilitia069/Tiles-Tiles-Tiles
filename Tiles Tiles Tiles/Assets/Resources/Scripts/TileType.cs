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

        //if (_isForwardDirection)
        //{
        //    _direction = Vector3.forward;
        //}
        //else if (_isBackwardDirection)
        //{
        //    _direction = Vector3.back;
        //}
        //else if (_isBackwardDirection)
        //{
        //    _direction = Vector3.right;
        //}
        //else if (_isBackwardDirection)
        //{
        //    _direction = Vector3.left;
        //}

        //_latterHeightVector = new Vector3(0, _latterHeight, 0);
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
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    //private void ChangeTileType()
    //{
    //    RaycastHit hit;
    //    Physics.Raycast(transform.position, Vector3.down, out hit, 1);

    //    TileModifier tileModifier = hit.collider.GetComponent<TileModifier>();
    //    if (tileModifier)
    //    {

    //    }

    //}

    //public void DetectLatterTop()
    //{
    //    Collider[] colliders = Physics.OverlapBox(transform.position + _direction, _latterHeightVector);

    //    foreach (var col in colliders)
    //    {
    //        Tile tile = col.GetComponent<Tile>();
    //        if (tile)
    //        {
    //            listOfNearbyValidTiles.Add(tile);
    //        }
    //    }
    //}
}
