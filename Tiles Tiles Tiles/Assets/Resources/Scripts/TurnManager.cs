using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    #region Singleton

    
    public static TurnManager instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Theres More than One TurnManager in the Scene!!!");
            Destroy(this.gameObject);
        }

        instance = this;

        //DontDestroyOnLoad(this.gameObject);        
    }



    #endregion

    [SerializeField]
    //public List<IPlayerTeam> playerTeam;// = new List<IPlayerTeam>();
    public List<CharacterStats> playerTeamList;
    public List<CharacterStats> playerTurnList;


    public bool isTurnStarted = true;
    public bool isPlayerTurn = true; //TODO: change to false after testing
    public bool isEnemyTurn = false;

    public bool isListStarted = false;

    // Start is called before the first frame update
    void Start()
    {        
        if (playerTeamList.Count > 0)
        {
            CharacterStats chara = (CharacterStats)playerTeamList[0];
            chara.isTurnActive = true;

            CameraTargetManager.instance.isLocked = true;
        }
    }

    // Update is called once per frame
    void Update()
    {        
        if (isTurnStarted)
        {
            playerTurnList = new List<CharacterStats>(playerTeamList);

            isTurnStarted = false;
        }

        if (isPlayerTurn)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchCharacter();
                CameraTargetManager.instance.isLocked = true;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                isPlayerTurn = false;
                Debug.Log("Players Turn is Over!!!");
                foreach (var character in playerTurnList)
                {
                    character.isTurnActive = false;
                    GridManager.instance.ClearSelectableTiles();
                }
                playerTurnList.Clear();
                isEnemyTurn = true;
            }
        }
    }

    public void PlayerCharacterActionDepleted(CharacterStats characterStats)
    {
        if (playerTurnList.Count == 1)
        {
            isPlayerTurn = false;
            Debug.Log("Players Turn is Over!!!");
            characterStats.isTurnActive = false;
            playerTurnList.Remove(characterStats);
            isEnemyTurn = true;


            CameraTargetManager.instance.isLocked = false;
            return;
        }

        SwitchCharacter();
        playerTurnList.Remove(characterStats);
    }

    public void SwitchCharacter()
    {
        if (playerTurnList.Count > 0)
        {

            foreach (var player in playerTurnList)
            {
                //if (player is CharacterStats)
                //{


                CharacterStats p = (CharacterStats)player;
                if (p.isTurnActive)
                {
                    int index = playerTurnList.IndexOf(p);
                    p.isTurnActive = false;
                    p._isMoveMode = true;
                    p.isTilesFound = false;
                    index++;
                    if (index >= playerTurnList.Count)
                    {
                        index = 0;
                    }
                    p = (CharacterStats)playerTurnList[index];
                    CameraTargetManager.instance.followTransform = p.transform;
                    p.isTurnActive = true;
                    break;
                }
                //}
            }
        }        
    }

    public void EndTurn()
    {

    }
}
