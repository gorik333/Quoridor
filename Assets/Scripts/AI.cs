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

    public static MoveGridPart NextMove(List<MoveGridPart> moveGridParts, MoveGridPart startMoveGrid)
    {
        MoveGridPart move = null;

        var bestMoves = new List<BestMoves>();

        var movesNeeded = 0;

        //Vector2Int possibleMove = new Vector2Int(currentMoveGrid.GridPos.x, currentMoveGrid.GridPos.y - 1);

        for (int k = 0; k < MAX_MOVES_COUNT; k++)
        {
            var currentMoveGrid = startMoveGrid;

            for (int i = 0; i < moveGridParts.Count; i++)
            {
                var possibleMoves = PossibleMove.GetPossibleMoves(moveGridParts, currentMoveGrid, Direction.Bottom);

                for (int j = 0; j < possibleMoves.Count; j++)
                {
                    if (moveGridParts[i].GridPos == possibleMoves[j] && !moveGridParts[i].IsWithPawn)
                    {
                        currentMoveGrid = moveGridParts[i];

                        Debug.Log(currentMoveGrid.GridPos);

                        movesNeeded++;

                        break;
                    }
                }

                if (currentMoveGrid.GridPos.y == 1)
                {
                    Debug.Log(movesNeeded);

                    move = moveGridParts[i];

                    break;
                }
            }
        }

        return move;
    }


    private static int MovesNeeded()
    {



        return 0;
    }
}
