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

    #endregion


    private bool _isAvailable;

    public event Place OnPlace;
    public delegate void Place(WallGridPart wallGridPart);


    private void Start()
    {
        _isAvailable = true;
    }


    public void PlaceWall(GameObject wall)
    {
        if (_isAvailable)
        {
            wall.transform.position = _wallPlacePos.position;

            _isAvailable = false;
        }
    }


    public Vector2Int GridPos { get => _gridPos; }

    public Vector3 WallPlacePos { get => _wallPlacePos.position; }

    public bool IsAvailable { get => _isAvailable; }
}
