using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinatesController : MonoBehaviour
{
    public Vector2 coordinates = new Vector2(0, 0);

    public Vector3 GetCoordinatesInVector3()
    {
        return new Vector3(coordinates.x, coordinates.y);
    }
}
