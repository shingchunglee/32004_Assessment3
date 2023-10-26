using System;
using UnityEngine;

public class Map
{
    private static Map instance;
    private static readonly object lockObject = new object();
    
    private Map()
    {
    }
    
    public static Map Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new Map();
                        instance.init();
                    }
                }
            }
            return instance;
        }
    }

    private int[,] levelMap = { 
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7}, 
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4}, 
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4}, 
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4}, 
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3}, 
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5}, 
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4}, 
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3}, 
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4}, 
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4}, 
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3}, 
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0}, 
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0}, 
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0}, 
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0}, 
    };
    // Dunno what to name them, see CAST quadrants  ^original is S
    private int[,] levelMapT;
    private int[,] levelMapA;
    private int[,] levelMapC;

    private int ySize => levelMap.GetLength(0);
    private int xSize => levelMap.GetLength(1);

    private void init()
    {
        levelMapT = new int[ySize, xSize];
        levelMapA = new int[ySize, xSize];
        levelMapC = new int[ySize, xSize];
        for (int i= 0; i<ySize; i++)
        {
            for (int j = 0; j<xSize; j++)
            {
                levelMapT[ySize-1-i, j] = levelMap[i,j];
                levelMapA[i, xSize-1-j] = levelMap[i,j];
                levelMapC[ySize-1-i, xSize-1-j] = levelMap[i,j];
            }
        }
        Debug.Log(levelMapT);
    }


    public bool isWall(int type) { return type == 1 || type == 2 || type == 3 || type == 4 || type == 7; }

    public Vector2 GetNeighbourCoordinates(Vector2 coordinates, Direction valueOfLastInput)
    {
        switch (valueOfLastInput)
        {
            case Direction.Up:
                if (coordinates.y >= (ySize * 2) - 1) return new Vector2(-1, -1);
                return new Vector2(coordinates.x, coordinates.y + 1);
            case Direction.Left:
                if (coordinates.x <= 0) return new Vector2(-1, -1);
                return new Vector2(coordinates.x - 1, coordinates.y);
            case Direction.Down:
                if (coordinates.y <= 0) return new Vector2(-1, -1);
                return new Vector2(coordinates.x, coordinates.y - 1);
            case Direction.Right:
                if (coordinates.x >= (xSize * 2) - 1) return new Vector2(-1, -1);
                return new Vector2(coordinates.x + 1, coordinates.y);
            default: 
                throw new ArgumentException("Invalid direction");
        }
    }

    public int GetTile(Vector2 levelCoordinate)
    {
        if (levelCoordinate.x == -1 || levelCoordinate.y == -1) return -1;
        return levelMap[(int)levelCoordinate.y, (int)levelCoordinate.x];
    }

    public Vector2 GetLevelCoordinates(Vector2 coordinates)
    {
        int Quadrant = GetQuadrant(coordinates);
        switch (Quadrant)
        {
            case 1:
                return new Vector2(Math.Abs(coordinates.x - xSize - (xSize - 1)), coordinates.y);
            case 2:
                return new Vector2(Math.Abs(coordinates.x - xSize - (xSize - 1)), Math.Abs(coordinates.y - ySize - (ySize - 1)));
            case 3:
                return new Vector2(coordinates.x, Math.Abs(coordinates.y - ySize - (ySize - 1)));
            case 4:
                return new Vector2(coordinates.x, coordinates.y);
            default:
                throw new Exception("Incorrect coordinates");
        }
    }

    public Vector3 GetSceneCoordinates(Vector2 coordinates)
    {
        return new Vector3(coordinates.x - xSize + 0.5f, coordinates.y - ySize + 0.5f);
    }

    private int GetQuadrant(Vector2 coordinates)
    {
        if (coordinates.x >= xSize && coordinates.y < ySize) return 1;
        if (coordinates.x >= xSize && coordinates.y >= ySize) return 2;
        if (coordinates.x < xSize && coordinates.y >= ySize) return 3;
        if (coordinates.x < xSize && coordinates.y < ySize) return 4;
        throw new Exception("Incorrect coordinates");
    }

    public bool isEdge(Vector2 coordinates)
    {
        return coordinates.x == 0 || coordinates.x == (2 * xSize) - 1 || coordinates.y == 0 || coordinates.y == (2 * ySize) - 1;
    }

    public Vector2 getOpposite(Vector2 coordinates)
    {
        return new Vector2(
            coordinates.x == 0 ? (2 * xSize) - 1 : coordinates.x == (2 * xSize) - 1 ? 0 : coordinates.x,
            coordinates.y == 0 ? (2 * ySize) - 1 : coordinates.y == (2 * ySize) -1 ? 0 : coordinates.y
        );
    }

    public Vector2 getTopLeftCoordinate()
    {
        return new Vector2(1, (2 * ySize) - 2);
    }
}