using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBaseClass : MonoBehaviour
{
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
    private Vector3 _velocity = new Vector3();
    private Vector3 _movementDirection = new Vector3();

    public float moveSpeed = 2;

    private bool _isCoverMode = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        if(GridManager.instance.stackTilePath.Count > 0)
        {
            Tile t = GridManager.instance.stackTilePath.Peek();
            Vector3 destinationCoordinates = t.transform.position;

            destinationCoordinates.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, destinationCoordinates) > 0.05f)
            {
                bool jump = (transform.position.y != destinationCoordinates.y);
                jump = false; //DELETE TO IMPLEMENT JUMP LATER
                if (jump)
                {
                    //TODO
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
        //gameObject.GetComponent<Animator>().SetBool("IsInCoverState", CoverAnimFLipFlop());
        //Debug.Log(_isCoverMode);
    }

    //private bool CoverAnimSwitch()
    //{
    //    _isCoverMode = !_isCoverMode;
    //    return _isCoverMode;
    //}

}
