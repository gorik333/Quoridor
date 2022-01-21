using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DragWall : MonoBehaviour
{
    private const float ROTATE_Y = 90f;

    #region Serialize data

    [SerializeField]
    private GameObject _ghostWallPrefab;

    [SerializeField]
    private GameObject _wallPrefab;

    [SerializeField]
    private GridController _gridController;

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
        if (_isCanPlace && _currentGrid.IsPlacementAllow(_isVertical))
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
        int currentPlayer = GameController.Instance.CurrentPlayer;
        var currentPawn = _gridController.GetCurrentPawn(currentPlayer);
        var currentMovePart = _gridController.GetMoveGrid(currentPawn.PawnPos);
        var wall = _currentGrid;

        var canPlace = false;

        if (wall.IsVertical && currentPawn.VerticalWallPlaced <= 9)
        {
            currentPawn.VerticalWallPlaced++;

            canPlace = true;
        }

        if (!wall.IsVertical && currentPawn.HorizontalWallPlaced <= 9)
        {
            currentPawn.HorizontalWallPlaced++;

            canPlace = true;
        }

        if (canPlace)
        {
            var ai = new Assets.Scripts.AI.AI();
            var pathFinding = new Assets.Scripts.AI.Pathfinding();

            if (wall != null)
            {
                var copy = _gridController.GridControllerCopy();

                copy.PlaceWall(wall, _isVertical, false);

                MoveGridPart playerGrid = copy.MoveGridPart.FindAll(e => e.IsWithPawn && e.GridPos != currentMovePart.GridPos).FirstOrDefault();

                var playerPawn = copy.CurrentPawn.FindAll(e => e.PawnPos == playerGrid.GridPos).FirstOrDefault();
                var computerPawn = copy.CurrentPawn.FindAll(e => e.PawnPos == currentMovePart.GridPos).FirstOrDefault();

                var nextMoveEnemy = ai.RunMove(currentMovePart, copy.MoveGridPart, pathFinding, false, playerPawn.StartY);
                var nextMovePlayer = ai.RunMove(playerGrid, copy.MoveGridPart, pathFinding, true, computerPawn.StartY);

                if (nextMoveEnemy != null && nextMovePlayer != null)
                {
                    if (nextMoveEnemy.Count != 0 && nextMovePlayer.Count != 0)
                    {
                        _currentGrid.PlaceWall(_isVertical);

                        DestroyGhostWall();
                    }
                    else
                    {
                        DestroyGhostWall();

                        _gridController.ResetWall(wall.GridPos, wall.IsVertical);
                    }
                }
            }
        }
        else
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
