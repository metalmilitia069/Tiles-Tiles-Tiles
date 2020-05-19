using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isWalkable = false;
    public bool isCurrent = false;
    public bool isCover = false;
    public bool isLatter = false;
    public bool isSelectable = false;
    public bool isTarget = false;

    public List<Tile> listOfNearbyValidTiles;

    public int jumpHeight = 100;

    private Tile referenceTile;

    public bool isVisited = false;
    public int distance = 0;

    public Tile parent = default;

    // Start is called before the first frame update
    void Start()
    {
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
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }

    }

    public void ResetTileData()
    {
        //bool isWalkable = false;
        bool isCurrent = false;
        bool isCover = false;
        bool isLatter = false;
        bool isSelectable = false;
        bool isTarget = false;

        

        int jumpHeight = 100;

         referenceTile = default;

        bool isVisited = false;

        int distance = 0;

        Tile parent = default;
    }

    public void ScanTiles()
    {
        ResetTileData();

        GatherNearbyTiles(Vector3.forward);
        GatherNearbyTiles(Vector3.back);
        GatherNearbyTiles(Vector3.left);
        GatherNearbyTiles(Vector3.right);


    }

    public void GatherNearbyTiles(Vector3 direction)
    {
        Vector3 halfExtends = new Vector3(0.25f, jumpHeight, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtends);

        foreach (var col in colliders)
        {
            referenceTile = col.GetComponent<Tile>();

            if (referenceTile != null && referenceTile.isWalkable)
            {
                RaycastHit hit;

                if (!Physics.Raycast(referenceTile.transform.position, Vector3.up, out hit, 1))
                {
                    listOfNearbyValidTiles.Add(referenceTile);
                }
            }
        }
    }
}
