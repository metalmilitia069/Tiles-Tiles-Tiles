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
    public List<CharacterStats> playerTeam;
    
    [SerializeField]
    public Queue<int> intqueue;

    public bool isPlayerTurn = true; //TODO: change to false after testing

    public bool isListStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(playerTeam.Count);
        //if (playerTeam.Count > 0)
        //{
        //    CharacterStats chara = (CharacterStats)playerTeam[0];
        //    chara.isTurnActive = true;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTeam != null && !isListStarted)
        {
            CharacterStats chara = (CharacterStats)playerTeam[0];
            chara.isTurnActive = true;
            isListStarted = true;
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isPlayerTurn)
            {
                foreach (var player in playerTeam)
                {
                    if (player is CharacterStats)
                    {
                        CharacterStats p = (CharacterStats)player;
                        if (p.isTurnActive)
                        {
                            int index = playerTeam.IndexOf(p);
                            p.isTurnActive = false;
                            p._isMoveMode = true;
                            p.isTilesFound = false;
                            index++;
                            if (index >= playerTeam.Count)
                            {
                                index = 0;
                            }
                            p = (CharacterStats)playerTeam[index];
                            p.isTurnActive = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
