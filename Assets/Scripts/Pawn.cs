using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField]
    private Transform _pawnModel;

    [SerializeField]
    private Vector2Int _currentPos;

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

        MoveToPart(gridPart);
    }


    public Vector2Int PawnPos { get => _currentPos; }

    public bool IsPlayer { get => _isPlayer; }
}
