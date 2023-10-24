using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    // Start is called before the first frame update
    private PacStudentAnimator animationComponent;
    private Movable movableComponent;
    private AudioPlayable audioPlayableComponent;
    [SerializeField] private bool isDead;
    private int[] demoMoves = {3,3,3,3,3,2,2,2,2,1,1,1,1,1,0,0,0,0};
    private int currentPath = 0;
	// Use this for initialization
	void Start ()
    {
        animationComponent = gameObject.GetComponent<PacStudentAnimator>();
        movableComponent = gameObject.GetComponent<Movable>();
        audioPlayableComponent = gameObject.GetComponent<AudioPlayable>();

        Walk();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) {
            if (movableComponent.finishedTween)
            {
                currentPath ++;
                currentPath = currentPath % demoMoves.Length;
                movableComponent.resetTween();
                Walk(); 
            }
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

    private void Walk()
    {
        int direction = demoMoves[currentPath];
        UpdateMove(direction);
        UpdateAnimation(direction);
        movableComponent.AddTween();
        audioPlayableComponent.PlayWalkSound();
    }
}
