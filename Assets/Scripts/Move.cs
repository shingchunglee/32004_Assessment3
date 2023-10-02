using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float xVelocity { get; private set; } = 0f;
    public float yVelocity { get; private set; } = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Down()
    {
        xVelocity = 0;
        yVelocity = -1/8f;
    }
    public void Left()
    {
        xVelocity = -1/8f;
        yVelocity = 0;
    }
    public void Up()
    {
        xVelocity = 0;
        yVelocity = 1/8f;
    }
    public void Right()
    {
        xVelocity = 1/8f;
        yVelocity = 0;
    }
}