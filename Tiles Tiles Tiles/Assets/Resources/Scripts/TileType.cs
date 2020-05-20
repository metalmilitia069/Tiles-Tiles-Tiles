using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileType : Tile
{
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
}
