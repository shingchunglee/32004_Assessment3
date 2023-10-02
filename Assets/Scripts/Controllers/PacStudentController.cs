using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    // Start is called before the first frame update
    private int direction = 0;
    private int state = 0;
    private PacStudentAnimator animationComponent;
    private GameController gameController;
    private Movable movableComponent;
    [SerializeField] private bool isDead;
	// Use this for initialization
	void Start () {
        animationComponent = gameObject.GetComponent<PacStudentAnimator>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        movableComponent = gameObject.GetComponent<Movable>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!isDead) {
            bool moved = gameController.gameObjectMove(gameObject);
            if (moved)
            {
                updateAnimation(direction);
                direction = (direction+1) % 4;
                NextMove(direction);
            }
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
