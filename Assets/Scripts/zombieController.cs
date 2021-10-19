using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class zombieController : MonoBehaviour
{
    //setting up state machine
    private enum State
    {
        Idle,
        ChaseTarget,
        Attack, 
        BeFriend
    }

    private State state;

    private State StateEnum
    {
        get { return state; }
        set
        {
            if (state != value)
            {
                Debug.Log($"going from {state} to {value}");
                state = value;
            }
        }
    }

    //gameobjects 
    public Transform knight;

    public Tilemap walkableTilemap;

    public knightUIController KUIController;

    private zombieUIController zombUIController;
    
    private Vector3 lastKnightPos;

    private Coroutine coroutinevar;

    private Coroutine MovementCoroutine
    {
        get { return coroutinevar;}
        set
        {
            if (coroutinevar != null)
            {
                StopCoroutine(coroutinevar);
            }

            coroutinevar = value;
        }
    }

    //setting up astar objects
    private Vector3Int[,] walkableArea;
    private Astar astar;
    private BoundsInt bounds;
    private Vector3Int direction;
    private List<Spot> roadPath = new List<Spot>();
    
    //methods to find grid position of knight + zombie 
    private Vector2Int GridPositionOfKnight
    {
        get
        {
            return (Vector2Int) walkableTilemap.WorldToCell(knight.position);
        }
    }

    private Vector2Int GridPositionOfZombie
    {
        get
        {
            return (Vector2Int) walkableTilemap.WorldToCell(transform.position);
        }
    }

    private Vector2Int GridPositionOfRandom
    {
        get
        {
            var gridSize = walkableTilemap.size;
            var randomPos = new Vector3Int(Random.Range(-gridSize.x, gridSize.x), Random.Range(-gridSize.y, gridSize.y),
                Random.Range(gridSize.z, gridSize.z));
            return (Vector2Int) walkableTilemap.WorldToCell(randomPos);
        }
    }
    
    void Start()
    {
        zombUIController = GetComponentInChildren<zombieUIController>();
        //tilemap a* set-up
        lastKnightPos = knight.position;
        walkableTilemap.CompressBounds();
        bounds = walkableTilemap.cellBounds;

        CreateGrid();
        astar = new Astar(walkableArea, bounds.size.x, bounds.size.y);
    }

    private void CreateGrid()
    {
        walkableArea = new Vector3Int[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                if (walkableTilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    walkableArea[i, j] = new Vector3Int(x, y, 0);
                }
                else
                {
                    walkableArea[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }
    
    void Update()
    {
        switch (state)
        {
            default:
            case State.Idle:
                Idling();
                FindTarget();
                break;
            case State.ChaseTarget:
                ChaseKnight();
                break;
            case State.Attack:
                AttackKnight();
                break;
            case State.BeFriend:
                BeFriend();
                break;
        }
        //check if knight has moved and start coroutine
        /*if (knight.position != lastKnightPos)
        {
            if (roadPath != null && roadPath.Count > 0)
                roadPath.Clear();
            roadPath = astar.CreatePath(walkableArea, GridPositionOfZombie, GridPositionOfKnight);
            if (roadPath == null)
            {
                Debug.Log("roadPath is empty");
                return;
            }

            StartCoroutine(keepMoving(roadPath));
        }*/
        //lastKnightPos = knight.position;
    }

    private void FixedUpdate()
    {
        lastKnightPos = knight.position;
    }

    private void Idling()
    {
        if (knight.position != lastKnightPos)
        {
            roadPath = astar.CreatePath(walkableArea, GridPositionOfZombie, GridPositionOfRandom);
            if (roadPath == null)
            {
                //Debug.Log("roadPath is empty for IDLING ZOMBIE");
                return;
            }
            
            MovementCoroutine = StartCoroutine(keepMoving(roadPath));
        }
    }

    private void FindTarget()
    {
        float targetRange = 5f;
        if (Vector3.Distance(transform.position, knight.position) < targetRange)
        {
            StateEnum = State.ChaseTarget;
        }
    }
    private void ChaseKnight()
    {
        //if (roadPath != null && roadPath.Count > 0)
            //roadPath.Clear();
        if (knight.position != lastKnightPos)
        {
            roadPath = astar.CreatePath(walkableArea, GridPositionOfZombie, GridPositionOfKnight);
            if (roadPath == null)
            {
                Debug.Log("roadPath is empty for CHASEKNIGHT ZOMBIE");
                return;
            }
            
            MovementCoroutine = StartCoroutine(keepMoving(roadPath));
            
        }
        
        float attack = 1f;
        if (Vector3.Distance(transform.position, knight.position) < attack)
        {
            StateEnum = State.Attack;
        }
        float stopChasing = 10f;
        if (Vector3.Distance(transform.position, knight.position) > stopChasing)
        {
            StateEnum = State.Idle;
        }
    }

    private void AttackKnight()
    {
        KUIController.setHealth(-10);
        float startChasing = 1f;
        if (Vector3.Distance(transform.position, knight.position) > startChasing)
        {
            StateEnum = State.ChaseTarget;
        }
    }

    private void BeFriend()
    {
        float tempBrain = zombUIController.getBrain;
        if (tempBrain < 50)
        {
            StateEnum = State.Idle;
        }
        //use astar to follow knight but the astar thing isn't working so fuck it
        //transform.position = knight.transform.position;
    }
    IEnumerator keepMoving(List<Spot> my_path)
    {
        for (int i = 0; i < my_path.Count; i++)
        {
            direction = new Vector3Int(my_path[i].X, my_path[i].Y, 0);
            Vector3 temp = walkableTilemap.GetCellCenterWorld(direction);
            transform.position = temp;
            yield return new WaitForSeconds(.3f);
        }
    }
}
