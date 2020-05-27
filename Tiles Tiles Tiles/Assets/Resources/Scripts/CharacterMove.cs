using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : CharacterBaseClass
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //if (!isMoving)
        //{
        //    ActivateMouse();            
        //}
        //else
        //{
        //    Move();
        //}

        //if (!isTilesFound)
        //{
        //    GridManager.instance.CalculateAvailablePath(this.gameObject);            
        //}        
    }

    protected void ActivateMouseToMovement()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();

                if (tile != null && tile.isSelectable)
                {
                    GridManager.instance.CalculatePathToDesignatedTile(tile);
                }
            }
        }
    }

    
}
