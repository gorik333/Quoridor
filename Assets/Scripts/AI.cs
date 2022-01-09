using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AI
{
    public static MoveGridPart NextMove(List<MoveGridPart> gridParts, Vector2Int pawnPos)
    {
        MoveGridPart move = null;

        var possibleMove = new Vector2Int(pawnPos.x, pawnPos.y - 1);

        for (int i = 0; i < gridParts.Count; i++)
        {
            if (gridParts[i].GridPos == possibleMove)
            {
                move = gridParts[i];

                break;
            }
        }

        return move;
    }
}
