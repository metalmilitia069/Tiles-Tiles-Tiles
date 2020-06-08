using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Tile Data")]
    public List<Tile> listOfSelectableTiles;
    public List<Tile> listOfAllTilesInLevel;

    public Tile tilePlaceholder;

    


    public CharacterBaseClass baseCharacter;

    [SerializeField]
    public Stack<Tile> stackTilePath = new Stack<Tile>();


    public delegate void OnScanTiles();
    public static event OnScanTiles EventScanTilesUpdate;

    public delegate void OnScanTilesForAI(float jumpHeight, Tile target);
    public static event OnScanTilesForAI EventScanTileUpdateForAI;


    //combat 
    public bool _isCombatMode = false;

    //AI STUFF
    public Tile actualTargetTile;

    #region Singleton

    public static GridManager instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Theres More than One GridManager in the Scene!!!");
            Destroy(this.gameObject);
        }

        instance = this;

        //DontDestroyOnLoad(this.gameObject);        
    }

    #endregion



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCurrentTile(GameObject characterStandingOnTile)
    {
        RaycastHit hit;
        Debug.DrawRay(characterStandingOnTile.transform.position, Vector3.down);
        if (Physics.Raycast(characterStandingOnTile.transform.position, Vector3.down, out hit, 1))
        {
            tilePlaceholder = hit.collider.transform.GetComponent<Tile>();

            tilePlaceholder.isCurrent = true;


            tilePlaceholder.isMoveMode = baseCharacter._isMoveMode; //To CHange Move or Attack Tile Neighbours


            if (tilePlaceholder.isCover) //&& tilePlaceholder.isCurrent)
            {
                baseCharacter.CoverMode(true);
            }
            else
            {
                baseCharacter.CoverMode(false);
            }
        }
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void CalculateAvailablePath(GameObject character)
    {
        baseCharacter = character.GetComponent<CharacterBaseClass>();
        


        EventScanTilesUpdate();
        GetCurrentTile(character);

        
        
        //BFS Algorithm
        var queueProcess = new Queue<Tile>();

        queueProcess.Enqueue(tilePlaceholder);
        tilePlaceholder.isVisited = true;

        while (queueProcess.Count > 0)
        {            
            Tile t = queueProcess.Dequeue();

            listOfSelectableTiles.Add(t);
            t.isSelectable = true;

            if (t.distance < baseCharacter._movePoints)
            {
                foreach (var tile in t.listOfNearbyValidTiles)
                {
                    if (!tile.isVisited)
                    {
                        tile.parent = t;
                        tile.isVisited = true;
                        tile.distance = 1 + t.distance;
                        queueProcess.Enqueue(tile);
                    }
                }
            }
        }
        baseCharacter.currentTile = tilePlaceholder;
        baseCharacter.isTilesFound = true;
    }

    public void CalculatePathToDesignatedTile(Tile tile)//
    {
        tile.isTarget = true;
        baseCharacter.isMoving = true;

        stackTilePath.Clear();

        Tile next = tile;

        while (next != null)
        {
            stackTilePath.Push(next);
            next = next.parent;
        }       
    }

    public void ClearSelectableTiles()
    {
        if (tilePlaceholder != null)
        {
            tilePlaceholder.isCurrent = false;
            tilePlaceholder = default;
        }

        foreach (var tile in listOfSelectableTiles)
        {
            tile.ResetTileData();
        }

        listOfSelectableTiles.Clear();
    }

    public void CalculateAttackPath(GameObject character)
    {
        baseCharacter = character.GetComponent<CharacterBaseClass>();

        EventScanTilesUpdate();
        GetCurrentTile(character);

        //BFS Algorithm
        var queueProcess = new Queue<Tile>();

        queueProcess.Enqueue(tilePlaceholder);
        tilePlaceholder.isVisited = true;

        while (queueProcess.Count > 0)
        {           
            Tile t = queueProcess.Dequeue();

            listOfSelectableTiles.Add(t);
            t.isAttacable = true;

            if (t.distance < baseCharacter._weaponRange)
            {
                foreach (var tile in t.listOfNearbyValidTiles)
                {
                    if (!tile.isVisited)
                    {
                        tile.parent = t;
                        tile.isVisited = true;
                        tile.distance = 1 + t.distance;
                        queueProcess.Enqueue(tile);
                    }
                }
            }
        }
        baseCharacter.currentTile = tilePlaceholder;
        baseCharacter.isAttackRangeFound = true;
    }


    //METHODS FOR THE AI 


    public void CalculateAvailablePathForAI(GameObject character)
    {
        baseCharacter = character.GetComponent<CharacterBaseClass>();



        //EventScanTilesUpdate();
        EventScanTileUpdateForAI(jumpHeight: 2, target: null);
        GetCurrentTile(character);



        //BFS Algorithm
        var queueProcess = new Queue<Tile>();

        queueProcess.Enqueue(tilePlaceholder);
        tilePlaceholder.isVisited = true;

        while (queueProcess.Count > 0)
        {
            Tile t = queueProcess.Dequeue();

            listOfSelectableTiles.Add(t);
            t.isSelectable = true;

            if (t.distance < baseCharacter._movePoints)
            {
                foreach (var tile in t.listOfNearbyValidTiles)
                {
                    if (!tile.isVisited)
                    {
                        tile.parent = t;
                        tile.isVisited = true;
                        tile.distance = 1 + t.distance;
                        queueProcess.Enqueue(tile);
                    }
                }
            }
        }
        baseCharacter.currentTile = tilePlaceholder;
        baseCharacter.isTilesFound = true;
    }

    public void FindPathAI(Tile target, GameObject playerCharacter, EnemyBaseClass enemyBase)//A*
    {
        //ComputeAdjacencyList(jumpHeight, target);
        EventScanTileUpdateForAI(jumpHeight: 2, target: null);
        GetCurrentTile(playerCharacter);
        //EnemyBaseClass enemyBase = playerCharacter.GetComponent<EnemyBaseClass>();

        List<Tile> openList = new List<Tile>();
        List<Tile> closeList = new List<Tile>();

        openList.Add(tilePlaceholder);
        tilePlaceholder.h = Vector3.Distance(tilePlaceholder.transform.position, target.transform.position);
        tilePlaceholder.f = tilePlaceholder.h;

        while (openList.Count > 0)
        {
            //A* algorithm

            Tile t = FindLowestF(openList);

            closeList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t, enemyBase);
                CalculatePathToDesignatedTile(actualTargetTile);
                return;
            }

            foreach (var tile in t.listOfNearbyValidTiles)
            {
                if (closeList.Contains(tile))
                {
                    //Do nothing, already processed
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);//g is the distance to beginning
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);//h is the estimated distance to the end
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }

        //todo: what do you if there is no path to the target tile???
        Debug.Log("Path not found");

    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (var tile in list)
        {
            if (tile.f < lowest.f)
            {
                lowest = tile;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t, EnemyBaseClass enemy)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= enemy._movePoints)
        {
            return t.parent;
        }

        Tile endTile = null;//default;
        for (int i = 0; i <= enemy._movePoints; i++)
        {
            endTile = tempPath.Pop();
        }
        Debug.Log("end tile :" + endTile);
        return endTile;

    }


}
