using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGridPart : MonoBehaviour
{
    #region Serialize data

    [SerializeField]
    private Vector2Int _gridPos;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Sprite _allowSprite;

    [SerializeField]
    private Transform _pawnMovePos;

    #endregion

    [SerializeField]
    private bool _showDirection;

    [SerializeField]
    private bool _isWithPawn;

    private List<Direction> _availableDirection;

    public event Move OnMoveTo;
    public delegate void Move(MoveGridPart moveGridPart);


    private void OnEnable()
    {
        _availableDirection = new List<Direction>();

        if (_gridPos.x != 9)
            _availableDirection.Add(Direction.Right);

        if (_gridPos.x != 1)
            _availableDirection.Add(Direction.Left);

        if (_gridPos.y != 9)
            _availableDirection.Add(Direction.Top);

        if (_gridPos.y != 1)
            _availableDirection.Add(Direction.Bottom);
    }


    private void Update()
    {
        if (_showDirection)
        {
            for (int i = 0; i < _availableDirection.Count; i++)
            {
                Debug.Log(_availableDirection[i]);
            }

            _showDirection = false;
        }
    }


    public void UpdateState(bool isAllowMoveTo)
    {
        _spriteRenderer.gameObject.SetActive(isAllowMoveTo);
    }


    public void MoveTo()
    {
        //OnMoveTo(this);

        IsWithPawn = true;
    }


    public void BlockDirection(Direction direction)
    {
        if (_availableDirection.Contains(direction))
            _availableDirection.Remove(direction);
    }


    public bool IsDirectionAvailable(Direction direction)
    {
        if (_availableDirection.Contains(direction))
            return true;

        return false;
    }


    public Vector2Int GridPos { get => _gridPos; }

    public Vector3 PawnMovePos { get => _pawnMovePos.position; }

    public bool IsWithPawn { get => _isWithPawn; set => _isWithPawn = value; }
}
