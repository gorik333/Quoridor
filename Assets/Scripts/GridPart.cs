using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPart : MonoBehaviour
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

    private bool _isAllowMoveTo;
    private bool _isWIthPlayer;

    public event Move OnMoveTo;
    public delegate void Move(GridPart gridPart);


    public void UpdateState(bool isAllowMoveTo)
    {
        _isAllowMoveTo = isAllowMoveTo;

        _spriteRenderer.gameObject.SetActive(isAllowMoveTo);
    }


    public bool MoveTo()
    {
        if (_isAllowMoveTo)
            OnMoveTo(this);

        return _isAllowMoveTo;
    }


    public Vector2Int GridPos { get => _gridPos; }

    public Vector3 PawnMovePos { get => _pawnMovePos.position; }

    public bool IsWithPlayer { get => _isWIthPlayer; }
}
