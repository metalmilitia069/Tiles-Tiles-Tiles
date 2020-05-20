using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModifier : MonoBehaviour
{
    [SerializeField]
    private bool isLatter;
    [SerializeField]
    private bool isCover;
    // Start is called before the first frame update
    void Start()
    {
        ChangeTileType();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeTileType()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.up, out hit, 1);

        Tile tile = hit.collider.GetComponent<Tile>();

        if (tile)
        {
            Debug.Log("shkjlhkjldshkjldskjhlsda  hdhjshkjlhsad");
            if (isLatter)
            {
                tile.isLatter = true;
            }
            if (isCover)
            {
                tile.isCover = true;
            }
        }

    }


}
