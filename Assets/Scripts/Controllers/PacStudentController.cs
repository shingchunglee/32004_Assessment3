using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PacStudentController : MonoBehaviour
{
    // Start is called before the first frame update
    private PacStudentAnimator animationComponent;
    private Movable movableComponent;
    private AudioPlayable audioPlayableComponent;
    [SerializeField] private ParticleSystem walkParticles;
    private ParticleSystem.EmissionModule walkEmission;
    private Animator animation;
    [SerializeField] private bool isDead;
    private int[] demoMoves = {3,3,3,3,3,2,2,2,2,1,1,1,1,1,0,0,0,0};
    private int currentPath = 0;
    private Direction? lastInput = null;
    private Direction currentInput;
    private Vector2 coordinates = new Vector2(15, 15);
    private Map map = Map.Instance;

	// Use this for initialization
	void Start ()
    {
        animation = gameObject.GetComponent<Animator>();
        animationComponent = gameObject.GetComponent<PacStudentAnimator>();
        movableComponent = gameObject.GetComponent<Movable>();
        audioPlayableComponent = gameObject.GetComponent<AudioPlayable>();
        gameObject.transform.position = map.GetSceneCoordinates(coordinates);

        walkEmission = walkParticles.emission;
        walkEmission.enabled = false;

        // Walk();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) {
            if (Input.GetKeyDown(KeyCode.W) && lastInput != Direction.Up)
            {
                lastInput = Direction.Up;
            }
            if (Input.GetKeyDown(KeyCode.A) && lastInput != Direction.Left)
            {
                lastInput = Direction.Left;
            }
            if (Input.GetKeyDown(KeyCode.S) && lastInput != Direction.Down)
            {
                lastInput = Direction.Down;
            }
            if (Input.GetKeyDown(KeyCode.D) && lastInput != Direction.Right)
            {
                lastInput = Direction.Right;
            }
            // if (movableComponent.finishedTween)
            // {
            //     currentPath ++;
            //     currentPath = currentPath % demoMoves.Length;
            //     movableComponent.resetTween();

                // int direction = demoMoves[currentPath];
                Walk(); 
            // }
        }
    }

    private void UpdateAnimation(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                animationComponent.Up();
                return;
            case Direction.Left:
                animationComponent.Left();
                return;
            case Direction.Down:
                animationComponent.Down();
                return;
            case Direction.Right:
                animationComponent.Right();
                return;
            default:
                return;
        }
    }

    private void UpdateMove(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                movableComponent.Up();
                return;
            case Direction.Left:
                movableComponent.Left();
                return;
            case Direction.Down:
                movableComponent.Down();
                return;
            case Direction.Right:
                movableComponent.Right();
                return;
            default:
                return;
        }
    }

    private void Walk()
    {
        if ( movableComponent.isTweening ) return;

        if ( lastInput is Direction valueOfLastInput )
        {
            try {
                Vector2 newCoordinates = map.GetNeighbourCoordinates(coordinates, valueOfLastInput);
                Debug.Log(newCoordinates);
                int tile = map.GetTile(map.GetLevelCoordinates(newCoordinates));
                Debug.Log(tile);
                if (!map.isWall(tile))
                {
                    animation.speed = 1;
                    currentInput = valueOfLastInput;
                    UpdateMove(valueOfLastInput);
                    UpdateAnimation(valueOfLastInput);
                    coordinates = newCoordinates;
                    walkEmission.enabled = true;
                    movableComponent.AddTween();
                    audioPlayableComponent.PlayWalkSound();
                } 
                else 
                {
                    newCoordinates = map.GetNeighbourCoordinates(coordinates, currentInput);
                    int currentInputTile = map.GetTile(map.GetLevelCoordinates(newCoordinates));
                    if (!map.isWall(currentInputTile))
                    {
                        animation.speed = 1;
                        UpdateMove(currentInput);
                        UpdateAnimation(currentInput);
                        coordinates = newCoordinates;
                        walkEmission.enabled = true;
                        movableComponent.AddTween();
                        audioPlayableComponent.PlayWalkSound();
                    } else
                    {
                        animation.speed = 0;
                        walkEmission.enabled = false;
                    }
                }
            } catch(IndexOutOfRangeException e) {
                Vector2 newCoordinates = map.GetNeighbourCoordinates(coordinates, currentInput);
                int currentInputTile = map.GetTile(map.GetLevelCoordinates(newCoordinates));
                if (!map.isWall(currentInputTile))
                {
                    animation.speed = 1;
                    UpdateMove(currentInput);
                    UpdateAnimation(currentInput);
                    coordinates = newCoordinates;
                    walkEmission.enabled = true;
                    movableComponent.AddTween();
                    audioPlayableComponent.PlayWalkSound();
                } else
                {
                    animation.speed = 0;
                    walkEmission.enabled = false;
                }
            }
            
            
            
            // currentInput = valueOfLastInput;
        }
        // if ( Map.walkable(coordinates) )
        // UpdateMove(direction);
        // UpdateAnimation(direction);
        // movableComponent.AddTween();
        // audioPlayableComponent.PlayWalkSound();
    }
}
