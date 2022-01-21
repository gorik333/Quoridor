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

    private bool _previousIsPlayer;

    public List<Pawn> CurrentPawn { get => _pawn; set => _pawn = value; }

    public List<MoveGridPart> MoveGridPart { get => _moveGridPart; set => _moveGridPart = value; }

    public List<WallGridPart> WallGridPart { get => _wallGridPart; set => _wallGridPart = value; }

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
            _wallGridPart[i].OnDelete += UnblockNearMoveGrid;
        }
    }


    private void InitializeMoveGridParts()
    {
        _moveGridPart = new List<MoveGridPart>(GetComponentsInChildren<MoveGridPart>());
    }


    public void PlaceWall(WallGridPart wallGridPart, bool isVertical, bool isMove = true)
    {
        var pos = wallGridPart.GridPos;

        BlockNearMoveGrid(wallGridPart.GridPos, isVertical);
        BlockNearWallAvailablePlace(wallGridPart.GridPos, isVertical);

        if (isMove)
            OnMove?.Invoke();
    }


    public void ResetWall(Vector2Int wallPos, bool isVertical)
    {
        var wall = GetWallGrid(wallPos);

        UnlockNearWallDrid(wallPos, isVertical);

        UnblockNearMoveGrid(wall, isVertical);

        wall.IsVerticalAllow = isVertical;
        wall.IsHorizontalAllow = !isVertical;
    }


    private void UnlockNearWallDrid(Vector2Int wallPos, bool isVertical)
    {
        if (!isVertical)
        {
            var firstPos = new Vector2Int(wallPos.x + 1, wallPos.y);
            var secondPos = new Vector2Int(wallPos.x - 1, wallPos.y);

            var firstWall = GetWallGrid(firstPos);
            var secondWall = GetWallGrid(secondPos);

            if (firstWall != null)
                firstWall.IsHorizontalAllow = true;
            if (secondWall != null)
                secondWall.IsHorizontalAllow = true;
        }
        else
        {
            var firstPos = new Vector2Int(wallPos.x, wallPos.y + 1);
            var secondPos = new Vector2Int(wallPos.x, wallPos.y - 1);

            var firstWall = GetWallGrid(firstPos);
            var secondWall = GetWallGrid(secondPos);

            if (firstWall != null)
                firstWall.IsVerticalAllow = true;
            if (secondWall != null)
                secondWall.IsVerticalAllow = true;
        }
    }


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


    private void UnblockNearMoveGrid(WallGridPart wall, bool isVertical)
    {
        var wallPos = wall.GridPos;

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
                    _moveGridPart[i].UnblockDirection(blockedWays[j].GridDirection);

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

        _spawnPos.Add(new Vector2Int(5, 9)); // BOTTOM CENTER GRID PART  5 = W, 1 = H  
        _spawnPos.Add(new Vector2Int(5, 1)); // TOP CENTER GRID PART     5 = W, 9 = H
    }


    private void InitializePawnList()
    {
        _pawn = new List<Pawn>();
    }


    public void SpawnPawns(int count, bool isWithComputer = false)
    {
        for (int i = 0; i < _moveGridPart.Count; i++)
        {
            for (int j = 0; j < count && j < _spawnPos.Count; j++)
            {
                if (_moveGridPart[i].GridPos == _spawnPos[j])
                {
                    _moveGridPart[i].IsWithPawn = true;

                    SpawnPawn(_moveGridPart[i], _spawnPos[j].y, isWithComputer);

                    break;
                }
            }
        }
    }


    private void SpawnPawn(MoveGridPart gridPart, int y, bool isWithComputer = false)
    {
        var rotation = _pawnPrefab.transform.rotation;

        var pawn = Instantiate(_pawnPrefab, transform.position, rotation).GetComponent<Pawn>();

        var isPlayer = Random.value > 0.5f;

        if (isWithComputer)
        {
            if (_pawnCount == 1)
                isPlayer = !_previousIsPlayer;
        }
        else
        {
            isPlayer = true;
        }

        pawn.StartY = y;

        if (y == 9)
            pawn.EndY = 1;
        if (y == 1)
            pawn.EndY = 9;

        pawn.SetUp(_pawnMaterial[_pawnCount % _pawnMaterial.Length], gridPart, isPlayer);
        pawn.name = $"{pawn.GetType().Name}_{_pawnCount}";

        _pawn.Add(pawn);
        _pawnCount++;
        _previousIsPlayer = isPlayer;
    }


    public void MoveToGridPart(MoveGridPart nextGridPart)
    {
        _currentPawn.MoveToPart(nextGridPart);

        nextGridPart.IsWithPawn = true;

        if (_currentPawn.EndY == _currentPawn.PawnPos.y)
        {
            EndGame.Instance.OnWin(_currentPawn.PawnColor);
        }

        OnMove?.Invoke();
    }


    public void PawnMoveQueue(int pawnIndex)
    {
        _currentPawn = _pawn[pawnIndex];

        if (_currentPawn.IsPlayer)
            UnlockPossibleMoves(_currentPawn);
        else
            StartCoroutine(ComputerMove(_currentPawn));
    }


    public void PlayerMove(MoveGridPart nextGridPart)
    {
        MoveGridPart currentMoveGrid = GetMoveGrid(_currentPawn.PawnPos);

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

        MoveGridPart currentMovePart = GetMoveGrid(pawn.PawnPos);

        var isMove = Random.value > 0.3f;
        var isVertical = Random.value > 0.5f;

        if (pawn.VerticalWallPlaced >= 10/* && isVertical*/)
            isMove = true;
        else if (pawn.VerticalWallPlaced < 10/* && isVertical*/)
            pawn.VerticalWallPlaced++;

        //if (pawn.HorizontalWallPlaced >= 10 && !isVertical)
        //    isMove = true;
        //else if (pawn.HorizontalWallPlaced < 10 && !isVertical)
        //    pawn.HorizontalWallPlaced++;


        if (!isMove)
        {
            var wall = GetRandomWallGridPart(isVertical);
            var ai = new Assets.Scripts.AI.AI();
            var pathFinding = new Assets.Scripts.AI.Pathfinding();

            if (wall != null)
            {
                var copy = GridControllerCopy();

                copy.PlaceWall(wall, isVertical, false);

                MoveGridPart playerGrid = copy.MoveGridPart.FindAll(e => e.IsWithPawn && e.GridPos != currentMovePart.GridPos).FirstOrDefault();

                var playerPawn = copy.CurrentPawn.FindAll(e => e.PawnPos == playerGrid.GridPos).FirstOrDefault();
                var computerPawn = copy.CurrentPawn.FindAll(e => e.PawnPos == currentMovePart.GridPos).FirstOrDefault();

                var nextMoveEnemy = ai.RunMove(currentMovePart, copy.MoveGridPart, pathFinding, false, playerPawn.StartY);
                var nextMovePlayer = ai.RunMove(playerGrid, copy.MoveGridPart, pathFinding, true, computerPawn.StartY);

                if (nextMoveEnemy != null && nextMovePlayer != null)
                {
                    if (nextMoveEnemy.Count != 0 && nextMovePlayer.Count != 0)
                    {
                        wall.PlaceWall(isVertical);
                    }
                    else
                    {
                        isMove = true;
                    }
                }
            }
            else
                isMove = true;
        }

        if (isMove)
        {
            currentMovePart.IsWithPawn = false;

            var moves = PossibleMove.GetPossibleMoves(_moveGridPart, currentMovePart);

            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i] == Vector2Int.zero)
                    moves.Remove(moves[i]);
            }

            var randMove = Random.Range(0, moves.Count);
            var nextMoveGrid = GetMoveGrid(moves[randMove]);

            MoveToGridPart(nextMoveGrid);
        }
    }


    public GridController GridControllerCopy()
    {
        GridController copy = (GridController)this.MemberwiseClone();

        copy.MoveGridPart = new List<MoveGridPart>(_moveGridPart);
        copy.WallGridPart = new List<WallGridPart>(_wallGridPart);
        copy.CurrentPawn = _pawn;

        return copy;
    }


    private WallGridPart GetWallGrid(Vector2Int pos)
    {
        for (int i = 0; i < _wallGridPart.Count; i++)
        {
            if (_wallGridPart[i].GridPos == pos)
                return _wallGridPart[i];
        }


        return null;
    }


    private void UnlockPossibleMoves(Pawn pawn)
    {
        MoveGridPart currentMoveGrid = GetMoveGrid(pawn.PawnPos);

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


    public MoveGridPart GetMoveGrid(Vector2Int pos)
    {
        for (int i = 0; i < _moveGridPart.Count; i++)
        {
            if (_moveGridPart[i].GridPos == pos)
            {
                return _moveGridPart[i];
            }
        }

        return null;
    }


    public Pawn GetCurrentPawn(int currentPawn)
    {
        return _pawn[currentPawn];
    }


    public WallGridPart GetRandomWallGridPart(bool isVertical)
    {
        var wallGridParts = new List<WallGridPart>();

        for (int i = 0; i < _wallGridPart.Count; i++)
        {
            if (_wallGridPart[i].IsHorizontalAllow && !isVertical)
            {
                if (!wallGridParts.Contains(_wallGridPart[i]))
                    wallGridParts.Add(_wallGridPart[i]);
            }
            else if (_wallGridPart[i].IsVerticalAllow && isVertical)
            {
                if (!wallGridParts.Contains(_wallGridPart[i]))
                    wallGridParts.Add(_wallGridPart[i]);
            }
        }

        if (wallGridParts.Count > 0)
        {
            var rand = Random.Range(0, wallGridParts.Count);

            var result = wallGridParts[rand];

            return result;
        }
        else
            return null;
    }
}
