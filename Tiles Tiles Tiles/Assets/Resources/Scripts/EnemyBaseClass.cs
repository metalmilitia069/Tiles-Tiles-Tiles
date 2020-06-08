using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseClass : MonoBehaviour
{
    public bool canBeAttacked = false;

    //Enemy Stats
    //Attack Stats
    [Header("ATTACK STATS")]
    [SerializeField]
    private int _damageModifier = 0;
    [SerializeField]
    private float _criticalChanceModifier = 0.0f;
    [SerializeField]
    private int _attackRangeModifier = 0;
    [SerializeField]
    private float _criticalDamageModifier = 0;

    //TODO: Elemental Attack
    [Header("ELEMENTAL ATTACK STATS")]
    [SerializeField]
    private int _elementalDmgFire = 0;
    [SerializeField]
    private int _elementalDmgElectricity = 0;
    [SerializeField]
    private int _elementalDmgCold = 0;
    [SerializeField]
    private int _elementalDmgPoison = 0;

    [Header("DEFENSE STATS")]
    //Defense Stats
    [SerializeField]
    public int armorNormal = 0;
    [SerializeField]
    public int armorBlindage = 0;
    [SerializeField]
    public float dodgeChance = 0.0f;
    [SerializeField]
    public int health = 0;

    //TODO: Elemental Defense
    [Header("ELEMENTAL DEFENSE STATS")]
    [SerializeField]
    public int elementalDefFire = 0;
    [SerializeField]
    public int elementalDefElectricity = 0;
    [SerializeField]
    public int elementalDefCold = 0;
    [SerializeField]
    public int elementalDefPoison = 0;



    //AI STUFF #########################################
    public GameObject target;

    public void ShowProbability()
    {
        Debug.Log(CombatCalculatorManager.instance.DisplayShotChance());
    }

    public void ApplyDamage(int Damage)
    {
        health -= Damage;
        if (health <= 0)
        {
            Debug.Log("ENEMY IS DEAD!!!!");
            Destroy(this.gameObject);
        }
    }








    [SerializeField]
    private List<Tile> selectableTiles = new List<Tile>();

    [SerializeField]
    public Tile currentTile;

    [SerializeField]
    public int _movePoints = 5;



    public bool isMoving = false;
    protected float halfHeight = 1;

    public bool isTilesFound = false;


    [SerializeField]
    private Stack<Tile> _stackTilePath = new Stack<Tile>();
    [SerializeField]
    private Vector3 _velocity = new Vector3();
    private Vector3 _movementDirection = new Vector3();

    public float moveSpeed = 2;

    private bool _isCoverMode = false;


    //Jump Variables
    public bool fallingDown = false;
    public bool jumpingUp = false;
    public bool movingEdge = false;
    public Vector3 jumpTarget;
    public float jumpVelocity = 4.5f;


    //Combat Mode
    [SerializeField]
    public int _weaponRange;
    public bool isAttackRangeFound = false;
    [SerializeField]
    public bool _isCombatMode = false;
    [SerializeField]
    public bool _isMoveMode = true;




    //from characterStats

    //Other Variables
    [Header("TURN VARIABLES")]
    public bool isTurnActive = false;
    public int actionPoints;

    // Start is called before the first frame update
    void Start()
    {
        AddEnemyToTeamList();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurnActive)
        {
            //if (CameraTargetManager.instance.isLocked)//
            //{
            //    //CameraTargetManager.instance.transform.parent = this.transform;//
            //    //CameraTargetManager.instance.transform.position = this.transform.position;//

            //    CameraTargetManager.instance.followTransform = transform;
            //}

            if (_isMoveMode)
            {
                //if (_listOfScannedEnemies.Count > 0)
                //{
                //    foreach (var enemy in _listOfScannedEnemies)
                //    {
                //        enemy.canBeAttacked = false;
                //    }
                //}

                //_listOfScannedEnemies.Clear();

                //if (!isMoving)
                //{
                //    ActivateMouseToMovement();
                //}
                //else
                //{
                //Move();
                //}

                if (isMoving)
                {
                    //FindNearestTarget();
                    //CalculatePathAI();
                    //GridManager.instance.CalculateAvailablePath(this.gameObject);//GridManager.instance.CalculateAvailablePathForAI(this.gameObject);//FindSelectableTiles();
                    Move();
                }

                if (!isTilesFound)
                {
                    if (this.GetComponent<EnemyBaseClass>().actionPoints <= 0)
                    {
                        TurnManager.instance.EnemyCharacterActionDepleted((EnemyBaseClass)this);
                        return;
                    }
                    //GridManager.instance.CalculateAvailablePath(this.gameObject);
                    FindNearestTarget();
                    CalculatePathAI();
                    GridManager.instance.CalculateAvailablePath(this.gameObject);
                }



            }

            //if (_isCombatMode)
            //{

            //    if (!isAttackRangeFound)
            //    {
            //        _weaponRange = weaponInstanceBelt[_currentWeaponIndex].GetComponent<WeaponBaseClass>().weaponRange + attackRangeModifier;
            //        GridManager.instance.CalculateAttackPath(this.gameObject);
            //        ScanForEnemies();
            //    }

            //    ActivateMouseToAttack();

            //}

            //if (Input.GetKeyDown(KeyCode.H))
            //{
            //    ChangeMode();
            //}

            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    if (_isCombatMode)
            //    {
            //        ChangeWeapon();
            //    }
            //}
        }
    }

    public void Move()
    {
        //if (this.GetComponent<CharacterStats>().actionPoints > 0)
        //{





        if (GridManager.instance.stackTilePath.Count > 0)
        {
            Tile t = GridManager.instance.stackTilePath.Peek();
            Vector3 destinationCoordinates = t.transform.position;

            destinationCoordinates.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, destinationCoordinates) >= 0.05f)
            {
                bool jump = (transform.position.y != destinationCoordinates.y);

                if (jump)
                {
                    Jump(destinationCoordinates);
                }
                else
                {
                    SetMovementDirection(destinationCoordinates);
                    SetRunningVelocity();
                }
                transform.forward = _movementDirection;
                transform.position += _velocity * Time.deltaTime;
            }
            else
            {
                transform.position = destinationCoordinates;
                GridManager.instance.stackTilePath.Pop();
            }
        }
        else
        {
            GridManager.instance.ClearSelectableTiles();
            isMoving = false;
            isTilesFound = false;

            //PUT EVENT TO HIDE TILES
        }
        //}





    }

    private void SetMovementDirection(Vector3 destinationCoordinates)
    {
        _movementDirection = destinationCoordinates - transform.position;
        _movementDirection.Normalize();
    }

    private void SetRunningVelocity()
    {
        _velocity = _movementDirection * moveSpeed;
    }

    public void CoverMode(bool option)
    {
        gameObject.GetComponentInChildren<Animator>().SetBool("IsInCoverState", option);
    }

    //private bool CoverAnimSwitch()
    //{
    //    _isCoverMode = !_isCoverMode;
    //    return _isCoverMode;
    //}

    public void Jump(Vector3 target)
    {
        if (fallingDown)
        {
            FallDownward(target);
        }
        else if (jumpingUp)
        {
            JumpUpward(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }

    public void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;

        SetMovementDirection(target);

        if (transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            jumpTarget = transform.position + ((target - transform.position) / 2.0f);
        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            _velocity = _movementDirection * (moveSpeed / 3.0f); //THis Is the Horizontal Speed Component of The Jumping Process
            //_velocity = Vector3.zero; // zero horizontal speed to climb latter
            float difference = targetY - transform.position.y;

            _velocity.y = jumpVelocity * (0.5f + (difference / 2));


        }
    }

    public void FallDownward(Vector3 target)
    {
        _velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            Vector3 p = transform.position;

            p.y = target.y;
            transform.position = p;

            _velocity = new Vector3();
        }
    }

    public void JumpUpward(Vector3 target)
    {
        _velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    public void MoveToEdge()
    {
        SetRunningVelocity();

        RaycastHit hit;

        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            movingEdge = false;
            fallingDown = true;

            _velocity /= 5.0f;
            _velocity.y = 1.5f;
        }
    }

    public void AddPlayerToTeamList()
    {
        //TurnManager.instance.playerTeam.Add((IPlayerTeam)this);
        //TurnManager.instance.playerTeamList.Add((CharacterStats)this);
    }


    //added to AI ###########################################################################################

    private void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = float.MaxValue;

        foreach (var obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position); //Look at this method as solution later >>> Vector3.SqrMagnitude

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        target = nearest;
    }
    private void CalculatePathAI()
    {
        Tile targetTile = GridManager.instance.GetTargetTile(target);
        //FindPathAI(targetTile);
        GridManager.instance.FindPathAI(targetTile, target, this);
    }

    //protected void FindPathAI(Tile target)//A*
    //{
    //    ComputeAdjacencyList(jumpHeight, target);
    //    GetCurrentTile();

    //    List<Tile> openList = new List<Tile>();
    //    List<Tile> closeList = new List<Tile>();

    //    openList.Add(currentTile);
    //    currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
    //    currentTile.f = currentTile.h;

    //    while (openList.Count > 0)
    //    {
    //        //A* algorithm

    //        Tile t = FindLowestF(openList);

    //        closeList.Add(t);

    //        if (t == target)
    //        {
    //            actualTargetTile = FindEndTile(t);
    //            MoveToTile(actualTargetTile);
    //            return;
    //        }

    //        foreach (var tile in t.adjacencyList)
    //        {
    //            if (closeList.Contains(tile))
    //            {
    //                //Do nothing, already processed
    //            }
    //            else if (openList.Contains(tile))
    //            {
    //                float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

    //                if (tempG < tile.g)
    //                {
    //                    tile.parent = t;

    //                    tile.g = tempG;
    //                    tile.f = tile.g + tile.h;
    //                }
    //            }
    //            else
    //            {
    //                tile.parent = t;

    //                tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);//g is the distance to beginning
    //                tile.h = Vector3.Distance(tile.transform.position, target.transform.position);//h is the estimated distance to the end
    //                tile.f = tile.g + tile.h;

    //                openList.Add(tile);
    //            }
    //        }
    //    }

    //    //todo: what do you if there is no path to the target tile???
    //    Debug.Log("Path not found");

    //}


    public void AddEnemyToTeamList()
    {
        //TurnManager.instance.playerTeam.Add((IPlayerTeam)this);
        TurnManager.instance.enemyTeamList.Add((EnemyBaseClass)this);
    }
}
