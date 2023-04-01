using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerGenerator : MonoBehaviour
{
    public enum Grid
    {
        FLOOR,
        WALL,
        EMPTY
    }

    //Variables
    public Grid[,] gridHandler;
    public List<WalkerObject> Walkers;
    public Tilemap tileMap;
    public Tile Floor;
    public Tile Wall;
    public int MapWidth = 30;
    public int MapHeight = 30;

    public Grid[,] upscaledGridHandler;

    public int MaximumWalkers = 10;
    public int TileCount = default;
    public float FillPercentage = 0.4f;
    public float WaitTime = 0.05f;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        gridHandler = new Grid[MapWidth, MapHeight];

        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                gridHandler[x, y] = Grid.EMPTY;
            }
        }

        Walkers = new List<WalkerObject>();

        Vector3Int TileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);

        WalkerObject curWalker = new WalkerObject(new Vector2(TileCenter.x, TileCenter.y), GetDirection(), 0.5f);
        gridHandler[TileCenter.x, TileCenter.y] = Grid.FLOOR;
        tileMap.SetTile(TileCenter, Floor);
        Walkers.Add(curWalker);

        TileCount++;

        StartCoroutine(CreateFloors());
    }

    Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    IEnumerator CreateFloors()
    {
        while ((float)TileCount / (float)gridHandler.Length < FillPercentage)
        {
            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in Walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

                if (gridHandler[curPos.x, curPos.y] != Grid.FLOOR)
                {
                    tileMap.SetTile(curPos, Floor);
                    TileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.FLOOR;
                    hasCreatedFloor = true;
                }
            }

            //Walker Methods
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                yield return new WaitForSeconds(WaitTime);
            }
        }

        StartCoroutine(CreateWalls());
    }

    void ChanceToRemove()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                Walkers.RemoveAt(i);
                break;
            }
        }
    }

    void ChanceToRedirect()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange)
            {
                WalkerObject curWalker = Walkers[i];
                curWalker.Direction = GetDirection();
                Walkers[i] = curWalker;
            }
        }
    }

    void ChanceToCreate()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count < MaximumWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = Walkers[i].Position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
                Walkers.Add(newWalker);
            }
        }
    }

    void UpdatePosition()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            WalkerObject FoundWalker = Walkers[i];
            FoundWalker.Position += FoundWalker.Direction;
            FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, 1, gridHandler.GetLength(0) - 2);
            FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, 1, gridHandler.GetLength(1) - 2);
            Walkers[i] = FoundWalker;
        }
    }

    IEnumerator CreateWalls()
    {
        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
            {
                if (gridHandler[x, y] == Grid.FLOOR)
                {
                    bool hasCreatedWall = false;

                    if (gridHandler[x + 1, y] == Grid.EMPTY)
                    {
                        tileMap.SetTile(new Vector3Int(x + 1, y, 0), Wall);
                        gridHandler[x + 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x - 1, y] == Grid.EMPTY)
                    {
                        tileMap.SetTile(new Vector3Int(x - 1, y, 0), Wall);
                        gridHandler[x - 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y + 1] == Grid.EMPTY)
                    {
                        tileMap.SetTile(new Vector3Int(x, y + 1, 0), Wall);
                        gridHandler[x, y + 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y - 1] == Grid.EMPTY)
                    {
                        tileMap.SetTile(new Vector3Int(x, y - 1, 0), Wall);
                        gridHandler[x, y - 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }

                    if (hasCreatedWall)
                    {
                        yield return new WaitForSeconds(WaitTime);
                    }
                }
            }
        }
        //StartCoroutine(UpscaleAndSmoothenMap());
    }

    // IEnumerator UpscaleAndSmoothenMap()
    // {
    //     upscaledGridHandler = new Grid[MapWidth * 20, MapHeight * 20];

    //     for (int x = 0; x < gridHandler.GetLength(1); x++)
    //     {
    //         for (int y = 0; y < gridHandler.GetLength(0); y++)
    //         {
    //             Debug.Log("x = " + x + ",y = " + y +":"  +gridHandler[x,y]);
    //             Debug.Log("x = " + y + ",y = " + x +":"  +gridHandler[y,x]);
    //         }
    //     }
        


    //     //Upscale map by x20
    //     for (int x = 0; x < gridHandler.GetLength(0); x++)
    //     {
    //         for (int y = 0; y < gridHandler.GetLength(1); y++)
    //         {

    //             if (gridHandler[x,y] == Grid.FLOOR)
    //                 {
    //                     Debug.Log("x = " + x + ",y = " + y +":"  +"FLOOR");
    //                     for (int i = 0; i < 20; i++)
    //                     {
    //                         for (int j = 0; j < 20; j++)
    //                         {
    //                             upscaledGridHandler[x * 20 + i,y * 20 +j] = gridHandler[x,y];
    //                         }
    //                     }
    //                 }
    //             else if (gridHandler[x,y] == Grid.EMPTY)
    //                 {
    //                     Debug.Log("x = " + x + ",y = " + y +":"  +"EMPTY");
    //                     for (int i = 0; i < 20; i++)
    //                     {
    //                         for (int j = 0; j < 20; j++)
    //                         { 
    //                             upscaledGridHandler[x * 20 + i,y * 20 +j] = gridHandler[x,y];
    //                         }
    //                     }
    //                 }
    //             else 
    //             {
    //                 Debug.Log("x = " + x + ",y = " + y +":"  +"WALL");
    //                 bool hasSmoothed = false;

    //                 int randomSmoothening = Mathf.FloorToInt(UnityEngine.Random.value * 5.99f);//5 levels of smoothening
                    
    //                 string side = null;

    //                 if (x > 0 && gridHandler[x - 1, y] == Grid.FLOOR)
    //                     side = "right";

    //                 if (x < gridHandler.GetLength(0) - 1 && gridHandler[x + 1, y] == Grid.FLOOR)
    //                     side = "left";

    //                 if (y > 0 && gridHandler[x, y - 1] == Grid.FLOOR)
    //                     side = "top";

    //                 if (y < gridHandler.GetLength(1) - 1 && gridHandler[x, y + 1] == Grid.FLOOR)
    //                     side = "bottom";

    //                 if (x > 0 && y > 0 && gridHandler[x - 1, y - 1] == Grid.FLOOR)
    //                     side = "top-right";

    //                 if (x < gridHandler.GetLength(0) - 1 && y > 0 && gridHandler[x + 1, y - 1] == Grid.FLOOR)
    //                     side = "top-left";

    //                 if (x > 0 && y < gridHandler.GetLength(1) - 1 && gridHandler[x - 1, y + 1] == Grid.FLOOR)
    //                     side = "bottom-right";

    //                 if (x < gridHandler.GetLength(0) - 1 && y < gridHandler.GetLength(1) - 1 && gridHandler[x + 1, y + 1] == Grid.FLOOR)
    //                     side = "bottom-left";
                    
    //                 Grid[,] matrix = SmoothMatrix(randomSmoothening, side);

    //                 for (int i = 0; i < 20; i++)
    //                 {
    //                     for (int j = 0; j < 20; j++)
    //                     {
    //                         upscaledGridHandler[x * 20 + i,y * 20 +j] = matrix[x,y];
    //                     }
    //                 }
    //                 if (hasSmoothed)
    //                 {
    //                     yield return new WaitForSeconds(WaitTime);
    //                 }  
    //             }
    //         }
    //     }
        
        
    //     for (int x = 0; x < upscaledGridHandler.GetLength(0); x++)
    //     {
    //         for (int y = 0; y < upscaledGridHandler.GetLength(1); y++)
    //         {
    //             Vector3Int curPos = new Vector3Int(x,y, 0);
    //             switch (upscaledGridHandler[x,y])
    //             {
    //                 case Grid.FLOOR:
    //                     tileMap.SetTile(curPos, Floor);
    //                     break;
    //                 case Grid.WALL:
    //                     tileMap.SetTile(curPos, Wall);
    //                     break;
    //                 case Grid.EMPTY:
    //                     tileMap.SetTile(curPos, Wall);
    //                     break;
    //             }
    //         }
    //     }
    // }


    // Grid[,] SmoothMatrix(int level, string side)
    // {
    //     Grid[,] matrix = new Grid[20, 20];
    //     int mid = 9;
    //     int width = 2 * mid + 1;
    //     int start = 0;
    //     int end = 19;
    //     for (int x = 0; x < 20; x++)
    //     {
    //         for (int y = 0; y < 20; y++)
    //         {
    //             matrix[x, y] = Grid.WALL;
    //         }
    //     }

    //     switch (side)
    //     {
    //         case "left":
    //             for (int i = start; i <= end; i++)
    //             {
    //                 for (int j = end - level; j <= end; j++)
    //                 {
    //                     matrix[i, j] = Grid.FLOOR;
    //                 }
    //             }
    //             break;

    //         case "right":
    //             for (int i = start; i <= end; i++)
    //             {
    //                 for (int j = start; j <= start + level; j++)
    //                 {
    //                     matrix[i, j] = Grid.FLOOR;
    //                 }
    //             }
    //             break;

    //         case "top":
    //             for (int i = start; i <= start + level; i++)
    //             {
    //                 for (int j = start; j <= end; j++)
    //                 {
    //                     matrix[i, j] = Grid.FLOOR;
    //                 }
    //             }
    //             break;

    //         case "bottom":
    //             for (int i = end - level; i <= end; i++)
    //             {
    //                 for (int j = start; j <= end; j++)
    //                 {
    //                     matrix[i, j] = Grid.FLOOR;
    //                 }
    //             }
    //             break;

    //         case "top-left":
    //             for (int i = start; i <= start + level; i++)
    //             {
    //                 for (int j = end - level; j <= end; j++)
    //                 {
    //                     matrix[i, j] = Grid.FLOOR;
    //                 }
    //             }
    //             break;

    //         case "top-right":
    //             for (int i = start; i <= start + level; i++)
    //             {
    //                 for (int j = start; j <= start + level; j++)
    //                 {
    //                     matrix[i, j] = Grid.FLOOR;
    //                 }
    //             }
    //             break;

    //         case "bottom-left":
    //             for (int i = end - level; i <= end; i++)
    //             {
    //                 for (int j = end - level; j <= end; j++)
    //                 {
    //                     matrix[i, j] = Grid.FLOOR;
    //                 }
    //             }
    //             break;

    //         case "bottom-right":
    //             for (int i = end - level; i <= end; i++)
    //             {
    //                 for (int j = start; j <= start + level; j++)
    //                 {
    //                     matrix[i, j] = Grid.FLOOR;
    //                 }
    //             }
    //             break;
    //     }
    //     return matrix;
    // }
}