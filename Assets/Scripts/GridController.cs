using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MoveGridPart;

public enum Direction
{
    Right,
    Left,
    Top,
    Bottom,
    None
}

public struct GridParam
{
    public Vector2Int GridPos;
    public Direction GridDirection;

    public GridParam(Vector2Int gridPos, Direction direction)
    {
        GridPos = gridPos;
        GridDirection = direction;
    }
}

public class GridController : MonoBehaviour
{
    [SerializeField]
    private GameObject _pawnPrefab;

    [SerializeField]
    private Material[] _pawnMaterial;

    private List<Pawn> _pawn;
    private List<Vector2Int> _spawnPos;
    private List<MoveGridPart> _moveGridPart;
    private List<WallGridPart> _wallGridPart;

    private Pawn _currentPawn;

    private int _pawnCount;

    public event Move OnMove;
    public delegate void Move();


    private void OnEnable()
    {
        InitializeMoveGridParts();
        InitializeWallGripParts();
        InitializePawnSpawnPos();
        InitializePawnList();
    }


    private void InitializeWallGripParts()
    {
        _wallGridPart = new List<WallGridPart>(GetComponentsInChildren<WallGridPart>());

        for (int i = 0; i < _wallGridPart.Count; i++)
        {
            _wallGridPart[i].OnPlace += PlaceWall;
        }
    }


    private void InitializeMoveGridParts()
    {
        _moveGridPart = new List<MoveGridPart>(GetComponentsInChildren<MoveGridPart>());

        for (int i = 0; i < _moveGridPart.Count; i++)
        {
            _moveGridPart[i].OnMoveTo += MoveToGridPart;
        }
    }


    private void PlaceWall(WallGridPart wallGridPart, bool isVertical)
    {
        BlockNearMoveGrid(wallGridPart.GridPos, isVertical);
        BlockNearWallAvailablePlace(wallGridPart.GridPos, isVertical);

        OnMove?.Invoke();
    }


    //public bool IsPlacementWallAllowed(Vector2Int pos)
    //{


    //    return false;
    //}


    private void BlockNearMoveGrid(Vector2Int wallPos, bool isVertical)
    {
        var blockedWays = new List<GridParam>();

        if (isVertical)
        {
            blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y + 1), Direction.Right));
            blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y), Direction.Right));

            blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y + 1), Direction.Left));
            blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y), Direction.Left));
        }
        else
        {
            blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y + 1), Direction.Bottom));
            blockedWays.Add(new GridParam(new Vector2Int(wallPos.x, wallPos.y), Direction.Top));

            blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y + 1), Direction.Bottom));
            blockedWays.Add(new GridParam(new Vector2Int(wallPos.x + 1, wallPos.y), Direction.Top));
        }

        for (int i = 0; i < _moveGridPart.Count; i++)
        {
            for (int j = 0; j < blockedWays.Count; j++)
            {
                if (_moveGridPart[i].GridPos == blockedWays[j].GridPos)
                {
                    _moveGridPart[i].BlockDirection(blockedWays[j].GridDirection);

                    break;
                }
            }
        }
    }


    private void BlockNearWallAvailablePlace(Vector2Int wallPos, bool isVertical)
    {
        var blockPlaces = new List<Vector2Int>();

        if (isVertical)
        {
            blockPlaces.Add(new Vector2Int(wallPos.x, wallPos.y - 1));
            blockPlaces.Add(new Vector2Int(wallPos.x, wallPos.y + 1));
        }
        else
        {
            blockPlaces.Add(new Vector2Int(wallPos.x + 1, wallPos.y));
            blockPlaces.Add(new Vector2Int(wallPos.x - 1, wallPos.y));
        }

        for (int i = 0; i < _wallGridPart.Count; i++)
        {
            for (int j = 0; j < blockPlaces.Count; j++)
            {
                if (_wallGridPart[i].GridPos == blockPlaces[j])
                {
                    _wallGridPart[i].DisallowPlacement(isVertical);

                    break;
                }
            }
        }
    }


    private void InitializePawnSpawnPos()
    {
        _spawnPos = new List<Vector2Int>();

        _spawnPos.Add(new Vector2Int(5, 1)); // BOTTOM CENTER GRID PART  5 = W, 1 = H  
        _spawnPos.Add(new Vector2Int(5, 9)); // TOP CENTER GRID PART     5 = W, 9 = H
    }


    private void InitializePawnList()
    {
        _pawn = new List<Pawn>();
    }


    public void SpawnPawns(int count)
    {
        for (int i = 0; i < _moveGridPart.Count; i++)
        {
            for (int j = 0; j < count && j < _spawnPos.Count; j++)
            {
                if (_moveGridPart[i].GridPos == _spawnPos[j])
                {
                    _moveGridPart[i].IsWithPawn = true;
                    SpawnPawn(_moveGridPart[i]);

                    break;
                }
            }
        }
    }


    private void SpawnPawn(MoveGridPart gridPart)
    {
        var rotation = _pawnPrefab.transform.rotation;

        var pawn = Instantiate(_pawnPrefab, transform.position, rotation).GetComponent<Pawn>();

        var isPlayer = true;

        if (_pawnCount == 1)
            isPlayer = false;

        pawn.SetUp(_pawnMaterial[_pawnCount % _pawnMaterial.Length], gridPart, isPlayer);
        pawn.name = $"{pawn.GetType().Name}_{_pawnCount}";

        _pawn.Add(pawn);

        _pawnCount++;
    }


    public void MoveToGridPart(MoveGridPart nextGridPart)
    {
        _currentPawn.MoveToPart(nextGridPart);

        nextGridPart.IsWithPawn = true;

        OnMove?.Invoke();
    }


    public void PawnMoveQueue(int pawnIndex)
    {
        _currentPawn = _pawn[pawnIndex];

        if (_currentPawn.IsPlayer)
            UnlockPossibleMoves(_currentPawn);
        else
            StartCoroutine( ComputerMove(_currentPawn));
    }


    public void PlayerMove(MoveGridPart nextGridPart)
    {
        MoveGridPart currentMoveGrid = GetGridPart(_currentPawn);

        var possibleMoves = PossibleMove.GetPossibleMoves(_moveGridPart, currentMoveGrid);

        for (int i = 0; i < possibleMoves.Count; i++)
        {
            if (nextGridPart.GridPos == possibleMoves[i])
            {
                currentMoveGrid.IsWithPawn = false;

                MoveToGridPart(nextGridPart);

                break;
            }
        }

    }


    private IEnumerator ComputerMove(Pawn pawn)
    {
        yield return new WaitForSeconds(0);
        MoveGridPart currentMovePart = GetGridPart(pawn);

        currentMovePart.IsWithPawn = false;

        //var nextGridPart = AI.NextMove(_moveGridPart, currentMovePart);

        Assets.Scripts.AI.AI ai = new Assets.Scripts.AI.AI();

        MoveGridPart opponentPos = _moveGridPart.Where(x => x.IsWithPawn && x != currentMovePart).FirstOrDefault();
        //Debug.Log("Opponent position " + _moveGridPart.Where(x => x.IsWithPawn && x != currentMovePart).ToList().Count + opponentPos.GridPos.x + opponentPos.GridPos.y);

        var nextGridPart = ai.NextMove(currentMovePart, _moveGridPart);                      //g.Dijkstra3(currentMovePart, _moveGridPart.Where(x => x.GridPos.y == 1 && x.GridPos.x == 5).FirstOrDefault(), _moveGridPart);

        MoveToGridPart(nextGridPart);
    }


    private void UnlockPossibleMoves(Pawn pawn)
    {
        MoveGridPart currentMoveGrid = GetGridPart(pawn);

        var possibleMoves = PossibleMove.GetPossibleMoves(_moveGridPart, currentMoveGrid);

        for (int i = 0; i < _moveGridPart.Count; i++)
        {
            for (int j = 0; j < possibleMoves.Count; j++)
            {
                if (_moveGridPart[i].GridPos == possibleMoves[j])
                {
                    _moveGridPart[i].UpdateState(true);

                    break;
                }
                else
                    _moveGridPart[i].UpdateState(false);
            }
        }
    }


    public Pawn GetPawn(int pawnIndex)
    {

        return _pawn[pawnIndex];
    }


    private MoveGridPart GetGridPart(Pawn pawn)
    {
        var pos = pawn.PawnPos;

        for (int i = 0; i < _moveGridPart.Count; i++)
        {
            if (_moveGridPart[i].GridPos == pos)
            {
                return _moveGridPart[i];
            }
        }

        return null;
    }
}
