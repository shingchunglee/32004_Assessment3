using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostController : MonoBehaviour
{ 
    private int direction = 0;
    private int state = 0;
	private GhostAnimatior animationComponent;
    private GameController gameController;
    private Movable moveableComponent;
    private float demoStatusDuration = 3.0f;
    private float lastTime;
	// Use this for initialization
	void Start () {
        animationComponent = gameObject.GetComponent<GhostAnimatior>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        moveableComponent = gameObject.GetComponent<Movable>();
	}

    // Update is called once per frame
    void Update()
    {
        lastTime += Time.deltaTime;
        GameObject gameController = GameObject.FindWithTag("GameController");
        GameController gameControllerComponent = gameController.GetComponent<GameController>();
        bool moved = gameControllerComponent.gameObjectMove(gameObject);
        if (moved)
        {
            updateAnimation(direction);
            direction = (direction+1) % 4;
            if (lastTime > demoStatusDuration) {
                updateState(state);
                state = (state + 1) % 4;
                lastTime = 0f;
            }
            NextMove(direction);
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

    private void updateAnimation(int direction)
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

    private void NextMove(int direction)
    {    
        if (direction == 0)
        {
            moveableComponent.Up();
        }
        if (direction == 1)
        {
            moveableComponent.Left();
        }
        if (direction == 2)
        {
            moveableComponent.Down();
        }
        if (direction == 3)
        {
            moveableComponent.Right();
        }
    }
}
