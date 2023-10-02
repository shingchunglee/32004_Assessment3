using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
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

        int ySize = levelMap.GetLength(0);
        int xSize = levelMap.GetLength(1);

        for (int y = 0; y < ySize; y++) {
            for (int x = 0; x < xSize; x++) {
                if (levelMap[y,x] == 0) {
                    continue;
                }
                if (levelMap[y,x] == 1) {
                    PlaceTile1(x, y, xSize, ySize);
                }
            }
        }
    }

    private void PlaceTile1(int x, int y, int xSize, int ySize)
    {
        float[] coordinates = GetCoordinates(x, y, xSize, ySize);
        Instantiate(tile1, new Vector3(coordinates[0], coordinates[1], 0), Quaternion.identity);
    }

    private float[] GetCoordinates(int x, int y, int xSize, int ySize)
    {
        return new float[] { (-1 * (xSize - x)) - 0.5f, (ySize - y) + 0.5f};
    }

    // Update is called once per frame
    // void Update()
    // {

    // }
}
