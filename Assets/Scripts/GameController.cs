using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private const int PLAYER_COUNT = 2;

    [SerializeField]
    private GridController _grid;

    private int _playerCount;
    private int _currentPlayer;

    private Coroutine _prevMove;

    public static GameController Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }


    private void OnEnable()
    {
        _grid.OnMove += PlayerMoved;
    }


    public void OnStart(bool isWithComputer)
    {
        SetPlayersCount(isWithComputer);

        MoveOrder();
    }


    private void SetPlayersCount(bool isWithComputer)
    {
        _grid.SpawnPawns(PLAYER_COUNT, isWithComputer);

        _playerCount = PLAYER_COUNT;
    }


    private void PlayerMoved()
    {
        if (_prevMove != null)
            StopCoroutine(_prevMove);

        MoveOrder();
    }


    private void MoveOrder()
    {
        if (_currentPlayer >= _playerCount)
        {
            _currentPlayer = 0;
        }

        _grid.PawnMoveQueue(_currentPlayer++);

        if (_currentPlayer >= _playerCount)
        {
            _currentPlayer = 0;
        }
    }

   
    public int CurrentPlayer { get => _currentPlayer; }
}
