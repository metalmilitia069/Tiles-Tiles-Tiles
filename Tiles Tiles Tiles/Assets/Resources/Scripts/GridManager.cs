using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Tile Data")]
    public List<Tile> listOfSelectableTiles;
    public List<Tile> listOfAllTilesInLevel;

    public Tile tilePlaceholder;


    CharacterBaseClass baseCharacter;

    [SerializeField]
    public Stack<Tile> stackTilePath = new Stack<Tile>();


    public delegate void OnScanTiles();
    public static event OnScanTiles EventScanTilesUpdate;

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
        }
    }

    public void CalculateAvailablePath(GameObject character)
    {
        EventScanTilesUpdate();
        GetCurrentTile(character);

        baseCharacter = character.GetComponent<CharacterBaseClass>();

        //BFS Algorithm
        var queueProcess = new Queue<Tile>();

        queueProcess.Enqueue(tilePlaceholder);
        tilePlaceholder.isVisited = true;

        while (queueProcess.Count > 0)
        {
            Debug.Log("mooooooooooooooooooooooooooo");
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

    public void CalculatePathToDesignatedTile(Tile tile)
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

        Debug.Log("CalculatePathToDesignatedTile");
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

}
