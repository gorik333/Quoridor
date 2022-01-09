using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public event OnMove Move;
    public delegate void OnMove();


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


    private void PlaceWall(WallGridPart wallGridPart)
    {

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


    private void MoveToGridPart(MoveGridPart gridPart)
    {
        _currentPawn.MoveToPart(gridPart);

        Move?.Invoke();
    }


    public void PawnMoveQueue(int pawnIndex)
    {
        _currentPawn = _pawn[pawnIndex];

        if (_currentPawn.IsPlayer)
            UnlockPossibleMoves(_currentPawn);
        else
            ComputerMove(_currentPawn);
    }


    private void ComputerMove(Pawn pawn)
    {
        var nextGrid = AI.NextMove(_moveGridPart, pawn.PawnPos);

        MoveToGridPart(nextGrid);
    }


    private void UnlockPossibleMoves(Pawn pawn)
    {
        var pos = pawn.PawnPos; // x y

        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        possibleMoves.Add(new Vector2Int(pos.x + 1, pos.y)); // x + 1, y
        possibleMoves.Add(new Vector2Int(pos.x - 1, pos.y)); // x - 1, y
        possibleMoves.Add(new Vector2Int(pos.x, pos.y - 1)); // x, y - 1
        possibleMoves.Add(new Vector2Int(pos.x, pos.y + 1)); // x, y + 1

        for (int i = 0; i < _moveGridPart.Count; i++)
        {
            for (int j = 0; j < possibleMoves.Count; j++)
            {
                if (_moveGridPart[i].GridPos == possibleMoves[j])
                {
                    _moveGridPart[i].UpdateState(true);

                    break;
                }

                if (_moveGridPart[i].GridPos != possibleMoves[j])
                    _moveGridPart[i].UpdateState(false);
            }
        }
    }


    public Pawn GetPawn(int pawnIndex)
    {

        return _pawn[pawnIndex];
    }


    private MoveGridPart GetGridPart(Vector2 pos)
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
}
