using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField]
    private Transform _pawnModel;

    [SerializeField]
    private Vector2Int _currentPos;

    private int _verticalWallPlaced;
    private int _horizontalWallPlaced;

    private string _pawnColor;

    private int _startY;
    private int _endY;
    private bool _isPlayer;


    private void Awake()
    {
        _currentPos = new Vector2Int();
    }


    public void MoveToPart(MoveGridPart gridPart)
    {
        _currentPos = gridPart.GridPos;

        transform.position = gridPart.PawnMovePos;
    }


    public void SetUp(Material material, MoveGridPart gridPart, bool isPlayer)
    {
        _isPlayer = isPlayer;

        _pawnModel.GetComponent<MeshRenderer>().material = material;

        _pawnColor = material.name;

        MoveToPart(gridPart);
    }


    public Vector2Int PawnPos { get => _currentPos; }

    public string PawnColor { get => _pawnColor; }

    public bool IsPlayer { get => _isPlayer; }

    public int StartY { get => _startY; set => _startY = value; }

    public int EndY { get => _endY; set => _endY = value; }

    public int HorizontalWallPlaced { get => _horizontalWallPlaced; set => _horizontalWallPlaced = value; }

    public int VerticalWallPlaced { get => _verticalWallPlaced; set => _verticalWallPlaced = value; }
}
