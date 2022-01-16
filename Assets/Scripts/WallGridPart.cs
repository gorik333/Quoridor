using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGridPart : MonoBehaviour
{
    #region Serialize data

    [SerializeField]
    private Vector2Int _gridPos;

    [SerializeField]
    private Transform _wallPlacePos;

    [SerializeField]
    private BoxCollider _wallCollider;

    #endregion

    private bool _isVerticalAllow;
    private bool _isHorizontalAllow;

    public event Place OnPlace;
    public delegate void Place(WallGridPart wallGridPart, bool isVertical);


    private void Start()
    {
        AllowPlacement();
    }


    public void PlaceWall(GameObject wall, bool isVertical)
    {
        OnPlace(this, isVertical);

        wall.transform.position = _wallPlacePos.position;

        DisallowPlacement(isVertical);
    }


    private void AllowPlacement()
    {
        _isHorizontalAllow = true;
        _isVerticalAllow = true;

        _wallCollider.enabled = true;
    }


    public void DisallowPlacement(bool isVertical)
    {
        if (isVertical)
            _isVerticalAllow = false;
        else
            _isHorizontalAllow = false;

        if (!_isVerticalAllow && !_isHorizontalAllow)
            _wallCollider.enabled = false;
    }


    public bool IsPlacementAllow(bool isVertical)
    {
        if (isVertical)
            return _isVerticalAllow;
        else
            return _isHorizontalAllow;
    }


    public Vector2Int GridPos { get => _gridPos; }

    public Vector3 WallPlacePos { get => _wallPlacePos.position; }

    public bool IsVerticalAllow { get => _isVerticalAllow; }

    public bool IsHorizontalAllow { get => _isHorizontalAllow; }
}
