using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWall : MonoBehaviour
{
    private const float ROTATE_Y = 90f;

    #region Serialize data

    [SerializeField]
    private GameObject _ghostWallPrefab;

    [SerializeField]
    private GameObject _wallPrefab;

    #endregion

    private Camera _camera;
    private Transform _currentWall;
    private WallGridPart _currentGrid;

    private bool _isCanPlace;
    private bool _isVertical;

    private RaycastHit _raycastHit;


    private void Start()
    {
        _camera = Camera.main;
    }


    public void OnBeginDrag(bool isVertical)
    {
        _isVertical = isVertical;

        SpawnWallPrefab();
    }


    private void SpawnWallPrefab()
    {
        if (_currentWall == null)
        {
            _currentWall = Instantiate(_ghostWallPrefab, transform.position, GetWallRotation()).GetComponent<Transform>();
        }
    }


    public void OnDrag()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _raycastHit))
        {
            CheckWallPlacement(_raycastHit.collider, _raycastHit.point);
        }
    }


    private void CheckWallPlacement(Collider collider, Vector3 position)
    {
        if (collider.GetComponentInParent<WallGridPart>() != null)
            _currentGrid = collider.GetComponentInParent<WallGridPart>();
        else
            _currentGrid = null;


        if (_currentGrid != null)
        {
            _isCanPlace = true;

            _currentWall.transform.position = _currentGrid.WallPlacePos;
        }
        else
        {
            _isCanPlace = false;

            _currentWall.transform.position = position;
        }
    }


    public void OnEndDrag()
    {
        if (_isCanPlace && _currentGrid.IsAvailable)
        {
            PlaceWall();
        }
        else
        {
            DestroyGhostWall();
        }
    }


    private void PlaceWall()
    {
        var originalWall = Instantiate(_wallPrefab, _currentWall.position, GetWallRotation());

        _currentGrid.PlaceWall(originalWall);

        DestroyGhostWall();
    }


    private void DestroyGhostWall()
    {
        Destroy(_currentWall.gameObject);
    }


    private Quaternion GetWallRotation()
    {
        var eulerAngles = Vector3.zero;

        if (_isVertical)
            eulerAngles.y += ROTATE_Y;

        return Quaternion.Euler(eulerAngles);
    }
}
