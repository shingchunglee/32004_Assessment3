using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostController : MonoBehaviour
{
    private int state = 0;
	private GhostAnimatior animationComponent;
    private GameController gameController;
    private Movable movableComponent;
    private float demoStatusDuration = 3.0f;
    private int[] demoMoves = {3,2,1,0};
    private int currentPath = 0;
    private float lastTime;
	// Use this for initialization
	void Start () {
        animationComponent = gameObject.GetComponent<GhostAnimatior>();
        movableComponent = gameObject.GetComponent<Movable>();

        Application.targetFrameRate = 144;
        int direction = demoMoves[currentPath];
        UpdateMove(direction);
        UpdateAnimation(direction);
        movableComponent.AddTween();
	}

    // Update is called once per frame
    void Update()
    {
        lastTime += Time.deltaTime;
        if (movableComponent.finishedTween)
        {
            currentPath ++;
            currentPath = currentPath % demoMoves.Length;
            movableComponent.resetTween();
            int direction = demoMoves[currentPath];
            UpdateMove(direction);
            UpdateAnimation(direction);
            if (lastTime > demoStatusDuration) {
                updateState(state);
                state = (state + 1) % 4;
                lastTime = 0f;
            }
            movableComponent.AddTween();
        }
    }

    private void updateState(int state)
    {
        if (state == 0)
        {
            animationComponent.Normal();
        }
        if (state == 1)
        {
            animationComponent.Scared();
        }
        if (state == 2)
        {
            animationComponent.Recovering();
        }
        if (state == 3)
        {
            animationComponent.Dead();
        }
    }

    private void UpdateAnimation(int direction)
    {
        if (direction == 0)
        {
            animationComponent.Up();
        }
        if (direction == 1)
        {
            animationComponent.Left();
        }
        if (direction == 2)
        {
            animationComponent.Down();
        }
        if (direction == 3)
        {
            animationComponent.Right();
        }
    }

    private void UpdateMove(int direction)
    {    
        if (direction == 0)
        {
            movableComponent.Up();
        }
        if (direction == 1)
        {
            movableComponent.Left();
        }
        if (direction == 2)
        {
            movableComponent.Down();
        }
        if (direction == 3)
        {
            movableComponent.Right();
        }
    }
}
