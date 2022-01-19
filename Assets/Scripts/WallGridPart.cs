using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGridPart : MonoBehaviour
{
    private const float ROTATE_Y = 90f;

    #region Serialize data

    [SerializeField]
    private Vector2Int _gridPos;

    [SerializeField]
    private Transform _wallPlacePos;

    [SerializeField]
    private BoxCollider _wallCollider;

    [SerializeField]
    private GameObject _wallPrefab;

    #endregion

    [SerializeField]
    private bool _isVerticalAllow;

    [SerializeField]
    private bool _isHorizontalAllow;

    private GameObject _wallSpawned;

    public event Place OnPlace;
    public delegate void Place(WallGridPart wallGridPart, bool isVertical, bool isMove = true);

    public event Delete OnDelete;
    public delegate void Delete(WallGridPart wallGridPart, bool isVertical);


    public WallGridPart(Vector2Int gridPos, Transform wallPlacePos, BoxCollider wallCollider, GameObject wallPrefab, bool isVerticalAllow, bool isHorizontalAllow)
    {
        _gridPos = gridPos;
        _wallPlacePos = wallPlacePos;
        _wallCollider = wallCollider;
        _wallPrefab = wallPrefab;
        _isVerticalAllow = isVerticalAllow;
        _isHorizontalAllow = isHorizontalAllow;
    }


    private void Start()
    {
        OnStart();
    }


    public void OnStart()
    {
        AllowPlacement();
    }


    public void PlaceWall(bool isVertical)
    {
        var originalWall = Instantiate(_wallPrefab, _wallPrefab.transform.position, GetWallRotation(isVertical));

        _wallSpawned = originalWall;

        OnPlace(this, isVertical);

        originalWall.transform.position = _wallPlacePos.position;

        _isHorizontalAllow = false;
        _isVerticalAllow = false;
        _wallCollider.enabled = false;
    }


    public void DeleteWall(bool isVertical)
    {
        if (_wallSpawned != null)
            Destroy(_wallSpawned);

        OnDelete?.Invoke(this, isVertical);
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


    private Quaternion GetWallRotation(bool isVertical)
    {
        var eulerAngles = Vector3.zero;

        if (isVertical)
            eulerAngles.y += ROTATE_Y;

        return Quaternion.Euler(eulerAngles);
    }


    public Vector2Int GridPos { get => _gridPos; }

    public Vector3 WallPlacePos { get => _wallPlacePos.position; }

    public bool IsVerticalAllow { get => _isVerticalAllow; }

    public bool IsHorizontalAllow { get => _isHorizontalAllow; }
    public BoxCollider WallCollider { get => _wallCollider; set => _wallCollider = value; }
    public Transform WallPlacePos1 { get => _wallPlacePos; set => _wallPlacePos = value; }
    public GameObject WallPrefab { get => _wallPrefab; set => _wallPrefab = value; }
}
