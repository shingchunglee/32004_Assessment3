using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    private PacStudentAnimator animationComponent;
    private Movable movableComponent;
    private AudioPlayable audioPlayableComponent;
    [SerializeField] private ParticleSystem walkParticles;
    private ParticleSystem.EmissionModule walkEmission;
    [SerializeField] private CoordinatesController coordinatesController;
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private GhostStateController ghostStateController;
    [SerializeField] private LifeController lifeController;
    [SerializeField] private LifeIndicatorController lifeIndicatorController;
    public GameObject wallParticleEffectPrefab;
    public GameObject deathParticleEffectPrefab;
    private BoxCollider2D boxCollider;
    private Animator animation;
    [SerializeField] private bool isDead;

    // private int[] demoMoves = {3,3,3,3,3,2,2,2,2,1,1,1,1,1,0,0,0,0};
    // private int currentPath = 0;
    private Direction? lastInput = null;
    private Direction? currentInput;
    // private Vector2 coordinates = new Vector2(1, 1);
    private Map map = Map.Instance;

	// Use this for initialization
	void Start ()
    {
        animation = gameObject.GetComponent<Animator>();
        animationComponent = gameObject.GetComponent<PacStudentAnimator>();
        movableComponent = gameObject.GetComponent<Movable>();
        audioPlayableComponent = gameObject.GetComponent<AudioPlayable>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        walkEmission = walkParticles.emission;

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && !gameController.isPause) {
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
                WalkUpdate(); 
            // }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Vector2 collisionPoint = transform.position;
            Instantiate(wallParticleEffectPrefab, collisionPoint, Quaternion.identity);

            audioPlayableComponent.PlayWallCollisionSound();
        }
        if (other.CompareTag("BonusScore"))
        {
            scoreController.UpdateScore(100);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Pellet"))
        {
            scoreController.UpdateScore(10);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("PowerPill"))
        {
            ghostStateController.SetScared();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Ghost"))
        {
            GhostController ghostController = other.GetComponent<GhostController>();
            if (ghostController.state == 0)
            {
                Die();
            }
            if (ghostController.state == 1 || ghostController.state == 2)
            {
                KillGhost(ghostController);
            }
        }
    }

    private void KillGhost(GhostController ghostController)
    {
        ghostStateController.Die(ghostController);
        scoreController.UpdateScore(300);
    }

    private void Die()
    {
        isDead = true;
        movableComponent.StopMoving();
        walkEmission.enabled = false;
        animationComponent.Dead();
        lifeController.LoseLife();
        lifeIndicatorController.UpdateLifeObjects(lifeController.lives);

        Vector3 particleEffectPosition = transform.position;
        Instantiate(deathParticleEffectPrefab, particleEffectPosition, Quaternion.identity);
    }

    public void DeadAnimationEnd()
    {
        Reset();
    }

    private void Reset()
    {
        isDead = false;
        lastInput = null;
        currentInput = null;
        coordinatesController.coordinates = map.GetTopLeftCoordinate();
        walkEmission.enabled = false;
        gameObject.transform.position = map.GetSceneCoordinates(coordinatesController.coordinates);
        animationComponent.Normal();
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

    private void WalkUpdate()
    {
        if ( movableComponent.isTweening ) return;

        if ( lastInput is Direction valueOfLastInput )
        {
            // try
            // {
                Vector2 newCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, valueOfLastInput);
                int tile = map.GetTile(map.GetLevelCoordinates(newCoordinates));
                if (!map.isWall(tile))
                {
                    Walk(valueOfLastInput, newCoordinates);
                }
                else 
                {
                    if ( currentInput is Direction valueOfCurrentInput ) {
                        newCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, valueOfCurrentInput);
                        int currentInputTile = map.GetTile(map.GetLevelCoordinates(newCoordinates));
                        if (!map.isWall(currentInputTile))
                        {
                            Walk(valueOfCurrentInput, newCoordinates);
                        } else
                        {
                            animation.speed = 0;
                            walkEmission.enabled = false;
                        }
                    }
                }
        }
    }

    private void Walk(Direction input, Vector2 newCoordinates)
    {
        animation.speed = 1;
        currentInput = input;
        UpdateBoxCollider(input);
        UpdateMove(input);
        UpdateAnimation(input);
        if (map.isEdge(coordinatesController.coordinates) && map.GetNeighbourCoordinates(coordinatesController.coordinates, input) == new Vector2(-1,-1)) 
        {
            coordinatesController.coordinates = map.getOpposite(coordinatesController.coordinates);
            gameObject.transform.position = map.GetSceneCoordinates(coordinatesController.coordinates);
            newCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, input);
        }

        walkEmission.enabled = true;
        movableComponent.AddTween(() => {coordinatesController.coordinates = newCoordinates;});
        audioPlayableComponent.PlayWalkSound();
    }

    private void UpdateBoxCollider(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                boxCollider.offset = new Vector2(0f, 0.1f); 
                boxCollider.size = new Vector2(0.8f, 0.9f);
                return;
            case Direction.Left:
                boxCollider.offset = new Vector2(-0.1f, 0f); 
                boxCollider.size = new Vector2(0.9f, 0.8f);
                movableComponent.Left();
                return;
            case Direction.Down:
                boxCollider.offset = new Vector2(0f, -0.1f); 
                boxCollider.size = new Vector2(0.8f, 0.9f);
                movableComponent.Down();
                return;
            case Direction.Right:
                boxCollider.offset = new Vector2(0.1f, 0f); 
                boxCollider.size = new Vector2(0.9f, 0.8f);
                movableComponent.Right();
                return;
            default:
                return;
        }
    }
    
    
}
