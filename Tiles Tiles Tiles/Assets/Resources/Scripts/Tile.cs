using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isWalkable = true;
    public bool isCurrent = false;
    public bool isCover = false;
    public bool isLatter = false;
    public bool isSelectable = false;
    public bool isTarget = false;

    public List<Tile> listOfNearbyValidTiles;

    public int jumpHeight = 2;

    private Tile referenceTile;

    public bool isVisited = false;
    public int distance = 0;

    public Tile parent = default;

    //COmbat Stuff
    public bool isAttacable = false;
    protected bool isAttackMode = false;
    public bool isMoveMode = true;

    //Needed for A* AI STUFF
    public float f = 0;
    public float g = 0;
    public float h = 0;

    // Start is called before the first frame update
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ResetTileData()
    {
        //bool isWalkable = false;
        //isCover = false;
        isCurrent = false;//
        //isLatter = false;
        isSelectable = false;//
        isTarget = false;//

        listOfNearbyValidTiles.Clear();//

        

        referenceTile = default;//

        isVisited = false;//

        distance = 0;//

        parent = null;//


        //COMBAT STUFF

        isAttacable = false;


        //AI STUFF

        f = g = h = 0;
    }

    public void ScanTiles()
    {
        ResetTileData();

        //Debug.Log("cucucucufhdgd");

        GatherNearbyTiles(Vector3.forward);
        GatherNearbyTiles(Vector3.back);
        GatherNearbyTiles(Vector3.left);
        GatherNearbyTiles(Vector3.right);

        if (isLatter)
        {
            DetectLatterTop();
        }

        //if (isCover)
        //{
        //    SetCovertTiles();
        //}

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
                else
                {
                    //if(GridManager.instance.baseCharacter._isCombatMode)
                    if(!this.isMoveMode)
                    {
                        if (hit.transform.GetComponent<EnemyBaseClass>())
                        {                           
                            listOfNearbyValidTiles.Add(referenceTile);
                        }
                    }
                }

            }
        }
    }

    public void GatherNearbyTilesAttackMode(Vector3 direction)
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
                else
                {
                    if (hit.transform.GetComponent<EnemyBaseClass>())
                    {
                        //referenceTile.isMoveMode = false;
                        //if (!referenceTile.isMoveMode)
                        //{
                            //Debug.Log("xibiru");
                            listOfNearbyValidTiles.Add(referenceTile);
                        //}

                    }
                }

            }
        }
    }

    public Vector3 latterSpotPosition;

    public void DetectLatterTop()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + latterSpotPosition, Vector3.up);

        foreach (var col in colliders)
        {
            Tile tile = col.GetComponent<Tile>();
            if (tile)
            {
                listOfNearbyValidTiles.Add(tile);
            }
        }
    }

    public void SetCovertTiles()
    {
        foreach (var tile in listOfNearbyValidTiles)
        {
            tile.isCover = true;
        }
    }

    //AI Methods

    public void ScanTilesForAI(float jumpHeight, Tile target)
    {
        ResetTileData();

        //Debug.Log("cucucucufhdgd");

        GatherNearbyTilesForAI(Vector3.forward, jumpHeight, target);
        GatherNearbyTilesForAI(Vector3.back, jumpHeight, target);
        GatherNearbyTilesForAI(Vector3.left, jumpHeight, target);
        GatherNearbyTilesForAI(Vector3.right, jumpHeight, target);

        if (isLatter)
        {
            DetectLatterTop();
        }

        //if (isCover)
        //{
        //    SetCovertTiles();
        //}

    }

    public void GatherNearbyTilesForAI(Vector3 direction, float jumHeight, Tile target)
    {
        Vector3 halfExtends = new Vector3(0.25f, jumpHeight, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtends);

        foreach (var col in colliders)
        {
            referenceTile = col.GetComponent<Tile>();

            if (referenceTile != null && referenceTile.isWalkable)
            {
                RaycastHit hit;

                if (!Physics.Raycast(referenceTile.transform.position, Vector3.up, out hit, 1) || (referenceTile == target))
                {
                    listOfNearbyValidTiles.Add(referenceTile);
                }
                else
                {
                    //if(GridManager.instance.baseCharacter._isCombatMode)
                    if (!this.isMoveMode)
                    {
                        if (hit.transform.GetComponent<EnemyBaseClass>())
                        {
                            listOfNearbyValidTiles.Add(referenceTile);
                        }
                    }
                }

            }
        }
    }

}
