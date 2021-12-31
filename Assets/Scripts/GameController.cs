using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private const float TIME_TO_MOVE = 30f;

    [SerializeField]
    private Grid _grid;

    private int _playerCount;
    private int _currentPlayer;

    private Coroutine _prevMove;


    private void OnEnable()
    {
        _grid.Move += PlayerMoved;
    }


    private void Start()
    {
        SetPlayersCount(2);

        /*_prevMove = StartCoroutine(*/
        MoveOrder()/*)*/;
    }


    public void SetPlayersCount(int count)
    {
        if (count < 0)
            _playerCount = 0;

        _grid.SpawnPawns(count);

        _playerCount = count;
    }


    private void PlayerMoved()
    {
        if (_prevMove != null)
            StopCoroutine(_prevMove);

        /*_prevMove = StartCoroutine(*/
        MoveOrder()/*)*/;
    }


    private void MoveOrder()
    {
        //while (true)

        if (_currentPlayer >= _playerCount)
        {
            _currentPlayer = 0;
        }

        _grid.PawnMoveQueue(_currentPlayer++);

        //yield return new WaitForSeconds(TIME_TO_MOVE);

        // mb next player to move, or without time limit
        //}
    }
}
