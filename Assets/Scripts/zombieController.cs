using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class zombieController : MonoBehaviour
{
    //gameobjects 
    public Transform knight;

    public Tilemap walkableTilemap;

    private Vector3 lastKnightPos;
    
    //setting up astar objects
    private Vector3Int[,] walkableArea;
    private Astar astar;
    private BoundsInt bounds;
    private Vector3Int direction;
    private List<Spot> roadPath = new List<Spot>();
    
    //functions to find grid position of knight + zombie 
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
    
    void Start()
    {
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
        //check if knight has moved and start coroutine
        if (knight.position != lastKnightPos)
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
        }
        //lastKnightPos = knight.position;
    }

    private void FixedUpdate()
    {
        lastKnightPos = knight.position;
    }

    IEnumerator keepMoving(List<Spot> my_path)
    {
        for (int i = 0; i < my_path.Count; i++)
        {
            direction = new Vector3Int(roadPath[i].X, roadPath[i].Y, 0);
            Vector3 temp = walkableTilemap.GetCellCenterWorld(direction);
            transform.position = temp;
            yield return new WaitForSeconds(.3f);
        }
    }
}
