using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AI
{
    public static GridPart NextMove(List<GridPart> gridParts, Vector2Int pawnPos)
    {
        GridPart move = gridParts[0];

        var possibleMove = new Vector2Int(pawnPos.x, pawnPos.y - 1);

        for (int i = 0; i < gridParts.Count; i++)
        {
            if (gridParts[i].GridPos == possibleMove)
            {
                move = gridParts[i];
            }
        }

        return move;
    }
}
