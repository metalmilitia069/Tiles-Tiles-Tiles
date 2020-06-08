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

    public bool isTurnActive = false;

    // Start is called before the first frame update
    void Start()
    {

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

                if (!isMoving)
                {
                    //FindNearestTarget();
                    //CalculatePath();
                    //FindSelectableTiles();
                }

                if (!isTilesFound)
                {
                    if (this.GetComponent<CharacterStats>().actionPoints <= 0)
                    {
                        //TurnManager.instance.PlayerCharacterActionDepleted((CharacterStats)this);
                        return;
                    }
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


}
