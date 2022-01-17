using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BestMoves
{
    public Vector2Int MovePos;
    public int MoveCountToWin;

    public BestMoves(Vector2Int movePos, int moveCountToWin)
    {
        MovePos = movePos;
        MoveCountToWin = moveCountToWin;
    }
}


public static class AI
{
    //private static Direction _mainDirection = Direction.Bottom;

    private const int MAX_MOVES_COUNT = 100;
    private const int MAX_ATTEMPTS = 2;

    public static MoveGridPart NextMove(List<MoveGridPart> moveGridParts, MoveGridPart startMoveGrid)
    {
        MoveGridPart move = null;

        var bestMoves = new List<BestMoves>();

        var movesNeeded = 0;

        for (int c = 0; c < MAX_ATTEMPTS; c++)
        {
            var firstMove = Vector2Int.zero;
            var currentMoveGrid = startMoveGrid;

            for (int k = 0; k < MAX_MOVES_COUNT; k++)
            {
                for (int i = 0; i < moveGridParts.Count; i++)
                {
                    var possibleMoves = PossibleMove.GetPossibleMoves(moveGridParts, currentMoveGrid, Direction.Bottom);

                    if (firstMove == Vector2.zero)
                    {
                        var rand = Random.Range(0, possibleMoves.Count);

                        firstMove = possibleMoves[rand];
                    }

                    for (int j = 0; j < possibleMoves.Count; j++)
                    {
                        if (moveGridParts[i].GridPos == possibleMoves[j] && !moveGridParts[i].IsWithPawn)
                        {
                            currentMoveGrid = moveGridParts[i];

                            //Debug.Log(currentMoveGrid.GridPos);

                            movesNeeded++;

                            break;
                        }
                    }

                    if (currentMoveGrid.GridPos.y == 1)
                    {
                        break;
                    }

                }

                if (currentMoveGrid.GridPos.y == 1)
                {
                    //Debug.Log(movesNeeded);

                    bestMoves.Add(new BestMoves(firstMove, movesNeeded));

                    movesNeeded = 0;

                    break;
                }
            }
        }


        var bestMove = Vector2Int.zero;

        var minMoves = int.MaxValue;

        for (int i = 0; i < bestMoves.Count; i++)
        {
            if (bestMoves[i].MoveCountToWin < minMoves)
            {
                minMoves = bestMoves[i].MoveCountToWin;
            }
        }


        for (int i = 0; i < bestMoves.Count; i++)
        {
            if (bestMoves[i].MoveCountToWin == minMoves)
            {
                bestMove = bestMoves[i].MovePos;


                break;
            }
        }

        for (int i = 0; i < moveGridParts.Count; i++)
        {
            //Debug.Log(bestMove);

            if (bestMove == moveGridParts[i].GridPos)
            {
                move = moveGridParts[i];


                break;
            }
        }


        return move;
    }
}
