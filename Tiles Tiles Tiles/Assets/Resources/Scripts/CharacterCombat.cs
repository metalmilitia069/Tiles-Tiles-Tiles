using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : CharacterMove
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            ActivateMouse();
        }
        else
        {
            Move();
        }

        if (!isTilesFound)
        {
            GridManager.instance.CalculateAvailablePath(this.gameObject);
        }
    }
}
