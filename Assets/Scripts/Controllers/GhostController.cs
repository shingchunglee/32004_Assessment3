using System;
using UnityEngine;
using System.Linq;

public enum GhostType 
{
    Green,
    Orange,
    Red,
    Blue
}

public class GhostController : MonoBehaviour
{
    [SerializeField] private CoordinatesController pacStudentCoordinateController;
    [SerializeField] private GameController gameController;
    public int state = 0;
	private GhostAnimatior animationComponent;
    private Movable movableComponent;
    // private float demoStatusDuration = 3.0f;
    // private int[] demoMoves = {3,2,1,0};
    private int currentPath = 0;
    private float lastTime;
    private Map map = Map.Instance;
    public GhostType ghostType;
    [SerializeField] private CoordinatesController coordinatesController;
    private Direction? lastMove;

    // Use this for initialization
    void Start () {
        animationComponent = gameObject.GetComponent<GhostAnimatior>();
        movableComponent = gameObject.GetComponent<Movable>();

        Reset();
	}

    // Update is called once per frame
    void Update()
    {
        if (!gameController.isPause) {
            WalkUpdate();
        }
    }

    private void WalkUpdate()
    {
        if ( movableComponent.isTweening ) return;
        if (state == 0) 
        {
            if (map.IsGhostSpawn(coordinatesController.coordinates))
            {
                Vector2 target = map.GetClosestGhostSpawnGoal(coordinatesController.coordinates);
                Direction bestSpawnMove = Direction.Up;
                Vector2 spawnNewCoordinates = coordinatesController.coordinates;

                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    Vector2 checkCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, direction);
                    if (map.isWall(map.GetTile(map.GetLevelCoordinates(checkCoordinates))) || checkCoordinates == new Vector2(-1, -1)) {
                        continue;
                    }

                    if (Vector2.Distance(checkCoordinates, target) <= Vector2.Distance(coordinatesController.coordinates, target)) 
                    {
                        bestSpawnMove = direction;
                        spawnNewCoordinates = checkCoordinates;
                        break;
                    }
                }
                lastMove = bestSpawnMove;
                Walk(bestSpawnMove, spawnNewCoordinates);
                return;
            }
            else
            {
                (Direction bestMove, Vector2 newCoordinates) = GetBestMove(ghostType);

                lastMove = bestMove;

                Walk(bestMove, newCoordinates);
            }
        }
        else if (state == 1 || state == 2)
        {
            (Direction bestMove, Vector2 newCoordinates) = Ghost1BestMove();

            lastMove = bestMove;

            Walk(bestMove, newCoordinates);
        }
        else
        {
            return;
        }
    }

    private (Direction bestMove, Vector2 newCoordinates) GetBestMove(GhostType ghostType)
    {
        switch (ghostType)
        {
            case GhostType.Green:
                return Ghost1BestMove();
            case GhostType.Orange:
                return Ghost2BestMove();
            case GhostType.Red:
                return Ghost3BestMove();
            case GhostType.Blue:
                return Ghost4BestMove();
            default:
                return Ghost1BestMove();
        }
    }

    private (Direction bestMove, Vector2 newCoordinates) Ghost4BestMove()
    {
        // throw new NotImplementedException();
        Direction? bestMove = null;
        Vector2 newCoordinates = new Vector2(0, 0);

        if (map.IsSide(coordinatesController.coordinates))
        {
            Vector2 checkRightCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, (Direction)GetRight(lastMove) );
            if (!map.isWall(map.GetTile(map.GetLevelCoordinates(checkRightCoordinates))) || !(checkRightCoordinates == new Vector2(-1, -1))) {
                    bestMove = (Direction)GetRight(lastMove);
                    newCoordinates = checkRightCoordinates;
            }
            Vector2 checkForwardCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, lastMove.Value);
            if (!map.isWall(map.GetTile(map.GetLevelCoordinates(checkForwardCoordinates))) || !(checkForwardCoordinates == new Vector2(-1, -1))) {
                    bestMove = lastMove.Value;
                    newCoordinates = checkForwardCoordinates;
            }
            Vector2 checkLeftCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, (Direction)GetLeft(lastMove) );
            if (!map.isWall(map.GetTile(map.GetLevelCoordinates(checkLeftCoordinates))) || !(checkLeftCoordinates == new Vector2(-1, -1))) {
                    bestMove = (Direction)((int)(lastMove - 1) % 4);
                    newCoordinates = checkLeftCoordinates;
            }
        }
        else
        {
            Vector2 target = map.GetClosestWall(coordinatesController.coordinates);

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (ReverseLastMove(lastMove) == direction) continue;

                Vector2 checkCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, direction);
                if (map.isWall(map.GetTile(map.GetLevelCoordinates(checkCoordinates))) || checkCoordinates == new Vector2(-1, -1)) {
                    continue;
                }

                if (Vector2.Distance(checkCoordinates, target) <= Vector2.Distance(coordinatesController.coordinates, target)) 
                {
                    bestMove = direction;
                    newCoordinates = checkCoordinates;
                    break;
                }
            }
        }

        // if (bestMove == null) {
        //     // if (lastMove.HasValue) 
        //     // {
        //     //     bestMove = lastMove;
        //     //     newCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, bestMove.Value);
        //     // } 
        //     // else 
        //     // {
        //     return ;
        //     // }

        // }
        return Ghost3BestMove();
        // return (bestMove, newCoordinates);
    }

    private (Direction bestMove, Vector2 newCoordinates) Ghost3BestMove()
    {
        Direction? bestMove = null;
        Vector2 newCoordinates = new Vector2(0, 0);
        System.Random random = new System.Random();
        foreach (Direction direction in Enum.GetValues(typeof(Direction)).OfType<Direction>().ToList().OrderBy(x => random.Next()))
        {
            if (ReverseLastMove(lastMove) == direction) continue;

            Vector2 checkCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, direction);
            if (map.isWall(map.GetTile(map.GetLevelCoordinates(checkCoordinates))) || checkCoordinates == new Vector2(-1, -1)) {
                continue;
            }
            
            bestMove = direction;
            newCoordinates = checkCoordinates;
            break;
        }

        if (bestMove == null) {
            bestMove = ReverseLastMove(lastMove);
            newCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, bestMove.Value);
        }
        return (bestMove ?? Direction.Up, newCoordinates);
    }

    private (Direction bestMove, Vector2 newCoordinates) Ghost2BestMove()
    {
        Direction? bestMove = null;
        Vector2 newCoordinates = new Vector2(0, 0);
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (ReverseLastMove(lastMove) == direction) continue;

            Vector2 checkCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, direction);
            if (map.isWall(map.GetTile(map.GetLevelCoordinates(checkCoordinates))) || checkCoordinates == new Vector2(-1, -1)) {
                continue;
            }

            if (Vector2.Distance(checkCoordinates, pacStudentCoordinateController.coordinates) <= Vector2.Distance(coordinatesController.coordinates, pacStudentCoordinateController.coordinates)) 
            {
                bestMove = direction;
                newCoordinates = checkCoordinates;
                break;
            }
        }

        if (bestMove == null) {
            bestMove = ReverseLastMove(lastMove);
            newCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, bestMove.Value);
        }
        return (bestMove ?? Direction.Up, newCoordinates);
    }

    private (Direction bestMove, Vector2 newCoordinates) Ghost1BestMove()
    {
        Direction? bestMove = null;
        Vector2 newCoordinates = new Vector2(0, 0);
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (ReverseLastMove(lastMove) == direction) continue;

            Vector2 checkCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, direction);
            if (map.isWall(map.GetTile(map.GetLevelCoordinates(checkCoordinates))) || checkCoordinates == new Vector2(-1, -1)) {
                continue;
            }

            if (Vector2.Distance(checkCoordinates, pacStudentCoordinateController.coordinates) >= Vector2.Distance(coordinatesController.coordinates, pacStudentCoordinateController.coordinates)) 
            {
                bestMove = direction;
                newCoordinates = checkCoordinates;
                break;
            }
        }

        if (bestMove == null) {
            bestMove = ReverseLastMove(lastMove);
            newCoordinates = map.GetNeighbourCoordinates(coordinatesController.coordinates, bestMove.Value);
        }
        return (bestMove ?? Direction.Up, newCoordinates);
    }

    private Direction? ReverseLastMove(Direction? lastMove)
    {
        if ( lastMove is Direction valueOflastMove )
        {
            switch (valueOflastMove)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Right:
                    return Direction.Left;
                default:
                    return null;
            }
        }
        return null;
    }

    private Direction? GetLeft(Direction? lastMove)
    {
        if ( lastMove is Direction valueOflastMove )
        {
            switch (valueOflastMove)
            {
                case Direction.Up:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Up;
                default:
                    return null;
            }
        }
        return null;
    }

    private Direction? GetRight(Direction? lastMove)
    {
        if ( lastMove is Direction valueOflastMove )
        {
            switch (valueOflastMove)
            {
                case Direction.Up:
                    return Direction.Right;
                case Direction.Left:
                    return Direction.Up;
                case Direction.Down:
                    return Direction.Left;
                case Direction.Right:
                    return Direction.Down;
                default:
                    return null;
            }
        }
        return null;
    }

    private void Walk(Direction input, Vector2 newCoordinates)
    {
        UpdateMove(input);
        UpdateAnimation(input);

        movableComponent.AddTween(() => {coordinatesController.coordinates = newCoordinates;});
    }

    public void updateState(int state)
    {
        this.state = state;
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

    private void Reset()
    {
        coordinatesController.coordinates = map.GetGhostSpawnCoordinates();
        gameObject.transform.position = map.GetSceneCoordinates(coordinatesController.coordinates);
    }
}
