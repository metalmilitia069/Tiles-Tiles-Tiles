﻿using System.Collections;
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



    //combat 
    public bool _isCombatMode = false;

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
            t.isAttakable = true;

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

}
