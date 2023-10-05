using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private GameObject[] tiles;
    [SerializeField] private GameObject tile1;
    [SerializeField] private GameObject tile2;
    [SerializeField] private GameObject tile3;
    [SerializeField] private GameObject tile4;
    [SerializeField] private GameObject tile5;
    [SerializeField] private GameObject tile6;
    [SerializeField] private GameObject tile7;
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
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0}, };

    
    // Start is called before the first frame update
    void Start()
    {
        GameObject manualMap = GameObject.FindWithTag("ManualMap");
        if (manualMap != null) {
            Destroy(manualMap);
        }

        tiles = new GameObject[] { tile1, tile2, tile3, tile4, tile5, tile6, tile7 };

        int ySize = levelMap.GetLength(0);
        int xSize = levelMap.GetLength(1);

        for (int y = 0; y < ySize; y++) {
            for (int x = 0; x < xSize; x++) {

                float[] coordinates = GetCoordinates(x, y, xSize, ySize);
                if (levelMap[y,x] == 0) {
                    continue;
                }

                PlaceTile(x, y, coordinates, xSize, ySize, levelMap[y,x]);

            }
        }

        float cameraSize = xSize * ((float)Screen.height / Screen.width);
        cameraSize = ySize > cameraSize ? ySize : cameraSize;
        gameObject.GetComponent<Camera>().orthographicSize = cameraSize;
    }
    private void PlaceTile(int x, int y, float[] coordinates, int xSize, int ySize, int tileNo)
    {
        GameObject tile = tiles[tileNo - 1];
        
        Vector3 rotation = FindRotation(x, y, xSize, ySize, tileNo);
        GameObject newTile = Instantiate(tile, new Vector3(coordinates[0], coordinates[1], 0), Quaternion.identity);
        newTile.transform.Rotate(rotation);
        GameObject xFlip = Instantiate(tile, new Vector3(coordinates[0] * -1, coordinates[1], 0), Quaternion.identity);
        xFlip.transform.Rotate(rotation);
        xFlip.transform.Rotate(new Vector3(0,180,0), Space.World);
        GameObject yFlip = Instantiate(tile, new Vector3(coordinates[0], coordinates[1] * -1, 0), Quaternion.identity);
        yFlip.transform.Rotate(rotation);
        yFlip.transform.Rotate(new Vector3(180,0,0), Space.World);
        GameObject xyFlip = Instantiate(tile, new Vector3(coordinates[0] * -1, coordinates[1] * -1, 0), Quaternion.identity);
        xyFlip.transform.Rotate(rotation);
        xyFlip.transform.Rotate(new Vector3(180,180,0), Space.World);
    }

    private Vector3 FindRotation(int x, int y, int xSize, int ySize, int tileNo)
    {
        int[] neighbours = GetNeighbours(x, y, xSize, ySize);
        Vector3 rotationVector = new Vector3(0, 0, 0);
        bool[] outerWalls = neighbours.Select((n, index) => IsSide(index) && IsOuterWall(n)).ToArray();
        int outerWallCount = outerWalls.Count(n => n);
        bool[] innerWalls = neighbours.Select((n, index) => IsSide(index) && IsInnerWall(n)).ToArray();
        int innerWallCount = innerWalls.Count(n => n);
        switch (tileNo)
        {
            case 1:
                if (outerWallCount == 2){
                    if (outerWalls[3] && outerWalls[6]) {rotationVector = new Vector3(0, 0, 0);}
                    else if (outerWalls[4] && outerWalls[6]) {rotationVector = new Vector3(0, 0, 90);}
                    else if (outerWalls[4] && outerWalls[1]) {rotationVector = new Vector3(0, 0, 180);}
                    else if (outerWalls[3] && outerWalls[1]) {rotationVector = new Vector3(0, 0, 270);}
                }
                if (outerWallCount == 1){
                    if (IsOuterWallOrEmpty(neighbours[3]) && IsOuterWallOrEmpty(neighbours[6])) {rotationVector = new Vector3(0, 0, 0);}
                    else if (IsOuterWallOrEmpty(neighbours[4]) && IsOuterWallOrEmpty(neighbours[6])) {rotationVector = new Vector3(0, 0, 90);}
                    else if (IsOuterWallOrEmpty(neighbours[4]) && IsOuterWallOrEmpty(neighbours[1])) {rotationVector = new Vector3(0, 0, 180);}
                    else if (IsOuterWallOrEmpty(neighbours[3]) && IsOuterWallOrEmpty(neighbours[1])) {rotationVector = new Vector3(0, 0, 270);} 
                }
                if (outerWallCount >= 3) {
                    if (!IsOuterWall(neighbours[5])) {rotationVector = new Vector3(0, 0, 0);}
                    else if (!IsOuterWall(neighbours[7])) {rotationVector = new Vector3(0, 0, 90);}
                    else if (!IsOuterWall(neighbours[2])) {rotationVector = new Vector3(0, 0, 180);}
                    else if (!IsOuterWall(neighbours[0])) {rotationVector = new Vector3(0, 0, 270);}
                }
                return rotationVector;
            case 2:
                if (outerWallCount >= 2){
                    if (outerWalls[3] && outerWalls[4]) {rotationVector = new Vector3(0, 0, 90);}
                    else if (outerWalls[1] && outerWalls[6]) {rotationVector = new Vector3(0, 0, 0);}
                    else 
                    {
                        if (outerWalls[3] || outerWalls[4]) {rotationVector = new Vector3(0, 0, 90);}
                        else if (outerWalls[1] || outerWalls[6]) {rotationVector = new Vector3(0, 0, 0);}
                    }
                }
                if (outerWallCount == 1){
                    if (outerWalls[3] || outerWalls[4]) {rotationVector = new Vector3(0, 0, 90);}
                    else if (outerWalls[1] || outerWalls[6]) {rotationVector = new Vector3(0, 0, 0);}
                } 
                return rotationVector;
            case 3:
                if (innerWallCount == 2){
                    if (IsInnerWall(neighbours[3]) && IsInnerWall(neighbours[6])) {rotationVector = new Vector3(0, 0, 0);}
                    else if (IsInnerWall(neighbours[4]) && IsInnerWall(neighbours[6])) {rotationVector = new Vector3(0, 0, 90);}
                    else if (IsInnerWall(neighbours[4]) && IsInnerWall(neighbours[1])) {rotationVector = new Vector3(0, 0, 180);}
                    else if (IsInnerWall(neighbours[3]) && IsInnerWall(neighbours[1])) {rotationVector = new Vector3(0, 0, 270);} 
                }
                if (innerWallCount == 1){
                    if (IsInnerWallOrEmpty(neighbours[3]) && IsInnerWallOrEmpty(neighbours[6])) {rotationVector = new Vector3(0, 0, 0);}
                    else if (IsInnerWallOrEmpty(neighbours[4]) && IsInnerWallOrEmpty(neighbours[6])) {rotationVector = new Vector3(0, 0, 90);}
                    else if (IsInnerWallOrEmpty(neighbours[4]) && IsInnerWallOrEmpty(neighbours[1])) {rotationVector = new Vector3(0, 0, 180);}
                    else if (IsInnerWallOrEmpty(neighbours[3]) && IsInnerWallOrEmpty(neighbours[1])) {rotationVector = new Vector3(0, 0, 270);}                 
                }
                if (innerWallCount >= 3) {
                    if (!IsInnerWall(neighbours[5])) {rotationVector = new Vector3(0, 0, 0);}
                    else if (!IsInnerWall(neighbours[7])) {rotationVector = new Vector3(0, 0, 90);}
                    else if (!IsInnerWall(neighbours[2])) {rotationVector = new Vector3(0, 0, 180);}
                    else if (!IsInnerWall(neighbours[0])) {rotationVector = new Vector3(0, 0, 270);}
                }
                return rotationVector;
            case 4:
                if (innerWallCount >= 2){
                    if (IsInnerWall(neighbours[3]) && IsInnerWall(neighbours[4])) {rotationVector = new Vector3(0, 0, 90);}
                    else if (IsInnerWall(neighbours[1]) && IsInnerWall(neighbours[6])) {rotationVector = new Vector3(0, 0, 0);}
                    else 
                    {
                        if (IsInnerWallOrEmpty(neighbours[3]) && IsInnerWallOrEmpty(neighbours[4])) {rotationVector = new Vector3(0, 0, 90);}
                        else if (IsInnerWallOrEmpty(neighbours[1]) && IsInnerWallOrEmpty(neighbours[6])) {rotationVector = new Vector3(0, 0, 0);}
                    }
                }
                if (innerWallCount == 1){
                    if (IsInnerWallOrEmpty(neighbours[3]) || IsInnerWallOrEmpty(neighbours[4])) {rotationVector = new Vector3(0, 0, 90);}
                    else if (IsInnerWallOrEmpty(neighbours[1]) || IsInnerWallOrEmpty(neighbours[6])) {rotationVector = new Vector3(0, 0, 0);}
                } 
                return rotationVector;
            case 5:
            case 6:
            case 7:
            default:
                return new Vector3(0, 0, 0);
        }
    }

    private bool IsSide(int index)
    {
        return index == 1 || index == 3 || index == 4 || index == 6;
    }

    private bool IsOuterWall(int tile)
    {
        return tile == 1 || tile == 2 || tile == 7;
    }
    
    private bool IsInnerWall(int tile)
    {
        return tile == 3 || tile == 4 || tile == 7;
    }

    private bool IsOuterWallOrEmpty(int tile)
    {
        return tile == 1 || tile == 2 || tile == 7 || tile == -1;
    }
    
    private bool IsInnerWallOrEmpty(int tile)
    {
        return tile == 3 || tile == 4 || tile == 7 || tile == -1;;
    }

    private float[] GetCoordinates(int x, int y, int xSize, int ySize)
    {
        return new float[] { (-1 * (xSize - x - 1)) - 0.5f, (ySize - y - 1) + 0.5f};
    }

    private int[] GetNeighbours(int x, int y, int xSize, int ySize)
    {
        // top left, top, top right, left, right, bottom left, bottom, bottom right
        
        return new int[] {
            x == 0 || y == 0 ? -1 : levelMap[y - 1, x - 1],
            y == 0 ? -1 : levelMap[y - 1, x],
            x == xSize - 1 || y == 0 ? -1 : levelMap[y - 1, x + 1],
            x == 0 ? -1 : levelMap[y, x - 1],
            x == xSize - 1 ? -1 : levelMap[y, x + 1],
            x == 0 || y == ySize - 1 ? -1 : levelMap[y + 1, x - 1],
            y == ySize - 1 ? -1 : levelMap[y + 1, x],
            x == xSize - 1 || y == ySize - 1 ? -1 : levelMap[y + 1, x + 1]
        };
    }
}
