using System.Collections.Generic;
using UnityEngine;

public struct Move
{
    public Vector2Int Pos;
    public Direction MoveDirection;

    public Move(Vector2Int pos, Direction direction)
    {
        Pos = pos;
        MoveDirection = direction;
    }
}

public static class PossibleMove
{
    public static List<Vector2Int> GetPossibleMoves(List<MoveGridPart> moveGridParts, MoveGridPart currentMoveGrid, Direction[] priority = null)
    {
        var pos = currentMoveGrid.GridPos;

        var possibleMoves = new List<Move>();

        if (currentMoveGrid.IsDirectionAvailable(Direction.Right))
        {
            var rightPos = new Vector2Int(pos.x + 1, pos.y);
            var nextGrid = GetGrid(moveGridParts, rightPos);

            if (nextGrid.IsWithPawn && nextGrid.IsDirectionAvailable(Direction.Right))
            {
                rightPos = new Vector2Int(pos.x + 2, pos.y);

                nextGrid = GetGrid(moveGridParts, rightPos);
            }

            if (!nextGrid.IsWithPawn)
                possibleMoves.Add(new Move(rightPos, Direction.Right)); // x + 1, y
        }
        if (currentMoveGrid.IsDirectionAvailable(Direction.Left))
        {
            var leftPos = new Vector2Int(pos.x - 1, pos.y);
            var nextGrid = GetGrid(moveGridParts, leftPos);

            if (nextGrid.IsWithPawn && nextGrid.IsDirectionAvailable(Direction.Left))
            {
                leftPos = new Vector2Int(pos.x - 2, pos.y);

                nextGrid = GetGrid(moveGridParts, leftPos);
            }

            if (!nextGrid.IsWithPawn)
                possibleMoves.Add(new Move(leftPos, Direction.Left)); // x - 1, y
        }
        if (currentMoveGrid.IsDirectionAvailable(Direction.Top))
        {
            var topPos = new Vector2Int(pos.x, pos.y + 1);
            var nextGrid = GetGrid(moveGridParts, topPos);

            if (nextGrid.IsWithPawn && nextGrid.IsDirectionAvailable(Direction.Top))
            {
                topPos = new Vector2Int(pos.x, pos.y + 2);

                nextGrid = GetGrid(moveGridParts, topPos);
            }

            if (!nextGrid.IsWithPawn)
                possibleMoves.Add(new Move(topPos, Direction.Top)); // x, y + 1
        }
        if (currentMoveGrid.IsDirectionAvailable(Direction.Bottom))
        {
            var bottomPos = new Vector2Int(pos.x, pos.y - 1);
            var nextGrid = GetGrid(moveGridParts, bottomPos);

            if (nextGrid.IsWithPawn && nextGrid.IsDirectionAvailable(Direction.Bottom))
            {
                bottomPos = new Vector2Int(pos.x, pos.y - 2);

                nextGrid = GetGrid(moveGridParts, bottomPos);
            }

            if (!nextGrid.IsWithPawn)
                possibleMoves.Add(new Move(bottomPos, Direction.Bottom)); // x, y - 1
        }

        List<Vector2Int> result = new List<Vector2Int>();

        result.Add(Vector2Int.zero);

        int max = (int)Direction.Left;

        if (possibleMoves.Count > 1 && priority != null)
        {
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                for (int j = 0; j < priority.Length; j++)
                {
                    if ((int)possibleMoves[i].MoveDirection < max)
                    {

                        result[0] = possibleMoves[i].Pos;

                        max = (int)possibleMoves[i].MoveDirection;
                    }
                }
            }
        }

        if (priority == null)
        {
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                result.Add(possibleMoves[i].Pos);
            }
        }

        //Debug.Log(

        return result;
    }


    private static MoveGridPart GetGrid(List<MoveGridPart> moveGridParts, Vector2Int pos)
    {
        for (int i = 0; i < moveGridParts.Count; i++)
        {
            if (moveGridParts[i].GridPos == pos)
            {
                return moveGridParts[i];
            }
        }

        return null;
    }
}