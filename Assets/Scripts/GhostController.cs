using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private Coroutine demoMoveCoroutine;
    private int direction = 0;
    private int state = 0;
	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        GameController gameControllerComponent = gameController.GetComponent<GameController>();
        bool moved = gameControllerComponent.gameObjectMove(gameObject);
        if (moved)
        {
            updateAnimation(direction);
            direction = (direction+1) % 4;
            if (direction == 0) 
            {
                updateState(state);
                state = (state + 1) % 4;
            }
            NextMove(direction);
        }
    }

    private void updateState(int state)
    {
        GhostAnimation ghostAnimationController = gameObject.GetComponent<GhostAnimation>();
        if (state == 0)
        {
            ghostAnimationController.Normal();
        }
        if (state == 1)
        {
            ghostAnimationController.Scared();
        }
        if (state == 2)
        {
            ghostAnimationController.Recovering();
        }
        if (state == 3)
        {
            ghostAnimationController.Dead();
        }
    }

    private void updateAnimation(int direction)
    {
        GhostAnimation ghostAnimationController = gameObject.GetComponent<GhostAnimation>();
        if (direction == 0)
        {
            ghostAnimationController.Up();
        }
        if (direction == 1)
        {
            ghostAnimationController.Left();
        }
        if (direction == 2)
        {
            ghostAnimationController.Down();
        }
        if (direction == 3)
        {
            ghostAnimationController.Right();
        }
    }

    private void NextMove(int direction)
    {
        Move ghostMoveController = gameObject.GetComponent<Move>();
        
        if (direction == 0)
        {
            ghostMoveController.Up();
        }
        if (direction == 1)
        {
            ghostMoveController.Left();
        }
        if (direction == 2)
        {
            ghostMoveController.Down();
        }
        if (direction == 3)
        {
            ghostMoveController.Right();
        }
    }
}
