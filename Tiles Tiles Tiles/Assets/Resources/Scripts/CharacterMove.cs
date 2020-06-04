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



                    this.GetComponent<CharacterStats>().actionPoints--;
                    //if (this.GetComponent<CharacterStats>().actionPoints <= 0)
                    //{
                    //    TurnManager.instance.PlayerCharacterActionDepleted((CharacterStats)this);
                    //}
                }
            }
        }
    }

    
}
