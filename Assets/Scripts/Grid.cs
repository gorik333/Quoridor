using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private GameObject _pawnPrefab;

    [SerializeField]
    private Material[] _pawnMaterial;

    private List<Pawn> _pawn;
    private List<Vector2Int> _spawnPos;
    private List<GridPart> _gridPart;

    private Pawn _currentPawn;

    private int _pawnCount;

    public event OnMove Move;
    public delegate void OnMove();


    private void OnEnable()
    {
        InitializeGridParts();
        InitializePawnSpawnPos();
        InitializePawnList();
    }


    private void InitializeGridParts()
    {
        _gridPart = new List<GridPart>(GetComponentsInChildren<GridPart>());

        for (int i = 0; i < _gridPart.Count; i++)
        {
            _gridPart[i].OnMoveTo += MoveToGridPart;
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
        for (int i = 0; i < _gridPart.Count; i++)
        {
            for (int j = 0; j < count && j < _spawnPos.Count; j++)
            {
                if (_gridPart[i].GridPos == _spawnPos[j])
                {
                    SpawnPawn(_gridPart[i]);

                    break;
                }
            }
        }
    }


    private void SpawnPawn(GridPart gridPart)
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


    private void MoveToGridPart(GridPart gridPart)
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
        var nextGrid = AI.NextMove(_gridPart, pawn.PawnPos);

        MoveToGridPart(nextGrid);
    }


    private void UnlockPossibleMoves(Pawn pawn)
    {
        var pos = pawn.PawnPos; // 5 1

        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        possibleMoves.Add(new Vector2Int(pos.x + 1, pos.y)); // 6 1
        possibleMoves.Add(new Vector2Int(pos.x - 1, pos.y)); // 4 1
        possibleMoves.Add(new Vector2Int(pos.x, pos.y - 1)); // 5 0
        possibleMoves.Add(new Vector2Int(pos.x, pos.y + 1)); // 5 2

        for (int i = 0; i < _gridPart.Count; i++)
        {
            for (int j = 0; j < possibleMoves.Count; j++)
            {
                if (_gridPart[i].GridPos == possibleMoves[j])
                {
                    _gridPart[i].UpdateState(true);

                    break;
                }

                if (_gridPart[i].GridPos != possibleMoves[j])
                    _gridPart[i].UpdateState(false);
            }
        }
    }


    public Pawn GetPawn(int pawnIndex)
    {

        return _pawn[pawnIndex];
    }


    private GridPart GetGridPart(Vector2 pos)
    {
        for (int i = 0; i < _gridPart.Count; i++)
        {
            if (_gridPart[i].GridPos == pos)
            {
                return _gridPart[i];
            }
        }

        return null;
    }
}
