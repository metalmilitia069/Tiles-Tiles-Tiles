﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModifier : MonoBehaviour
{
    [SerializeField]
    private bool isLatter;
    [SerializeField]
    public bool isCover;
    [SerializeField]
    public bool isHalfCover = true;
    [SerializeField]
    public bool isFullCover = false;
    [SerializeField]
    public float halfCoverPenalty = 0.20f;
    [SerializeField]
    public float fullCoverPenalty = 0.90f;
    

    [SerializeField]
    private float _latterHeight = 5;
    //[SerializeField]
    //private Vector3 _latterHeightVector;

    [SerializeField]
    private bool _isForwardDirection = false;
    [SerializeField]
    private bool _isBackwardDirection = false;
    [SerializeField]
    private bool _isRightDirection = false;
    [SerializeField]
    private bool _isLeftDirection = false;
    [SerializeField]
    private Vector3 _position;

    [SerializeField]
    private bool _isRayUp = false;
    private Vector3 _rayDirection;



    private bool isTypeChanged = false;
    // Start is called before the first frame update
    void Start()
    {
        if (_isForwardDirection)
        {
            _position = Vector3.forward;
        }
        else if (_isBackwardDirection)
        {
            _position = Vector3.back;
        }
        else if (_isRightDirection)
        {
            _position = Vector3.right;
        }
        else if (_isLeftDirection)
        {
            _position = Vector3.left;
        }

        _position = _position + new Vector3(0, _latterHeight, 0);

        if (_isRayUp)
        {
            _rayDirection = Vector3.up;
        }
        else
        {
            _rayDirection = Vector3.down;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTypeChanged)
        {
            ChangeTileType();
        }

        if (isHalfCover)
        {
            isFullCover = false;
        }
        else
        {
            isFullCover = true;
        }
    }

    private void ChangeTileType()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, _rayDirection, out hit, 1))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile)
            {
                isTypeChanged = true;
                //Debug.Log("shkjlhkjldshkjldskjhlsda  hdhjshkjlhsad");
                if (isLatter)
                {
                    tile.isLatter = true;
                    tile.latterSpotPosition = _position;
                }
                if (isCover)
                {
                    tile.SetCovertTiles();
                }
            }
        }



    }


}
