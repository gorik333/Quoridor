using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class AI
    {
        /*
        public MoveGridPart NextMove(MoveGridPart position, bool isPlayerPawn, int depth = 5)
        {
            MoveGridPart move = new MoveGridPart();
            //if(!opponent.closerToFinish)
                //move = RunState.NextMove(position, depth, alpha, beta, pawnType);
            //else
                //move = WallState.NextMove();



            return move;
        }
        */



        #region

        /*

        public int minimax(MoveGridPart position, int depth, bool maximizingPlayer)
        {

            //if (depth == 0 || game.isOver)
            //    return static evaluation of position;

            if (maximizingPlayer)
            {
                int maxEval = Int16.MinValue;
                foreach (child of position)                      //child of position - position that can be achieved by a single move. Moves + walls
                {
                    int eval = minimax(child, depth - 1, false);                //perhabs position is a field?
                    maxEval = Math.Max(maxEval, eval);
                }
                return maxEval;
            }
            else
            {
                int minEval = Int16.MaxValue;
                foreach (child of position)
                {
                    int eval = minimax(child, depth - 1, true);
                    minEval = Math.Min(minEval, eval);
                }
                    
                return minEval;
            }


        }

        static int Evaluation(MoveGridPart position)
        {
            int stepsToFinish = 0;
            //stepsToFinish = position.howManyGridPartsToFinish;
            //but reverse. less moves to finish = better.

            return stepsToFinish;
        }


        */

        #endregion







        public MoveGridPart NextMove(MoveGridPart startPos , List<MoveGridPart> field, bool isPlayerPawn = false)
        {
            Pathfinding dijkstra = new Pathfinding();
            //Debug.Log("Test grid pos: " + opponentPos.GridPos.x + ", " + opponentPos.GridPos.y);

            MoveGridPart opponentPos = field.Find(grid => grid.IsWithPawn && grid != startPos);
            MoveGridPart testGrid = field.Find(grid => grid.GridPos.x == opponentPos.GridPos.x && grid.GridPos.y == opponentPos.GridPos.y);
            Queue<MoveGridPart> playerPath = RunMove(opponentPos, field, dijkstra, true);
            Queue<MoveGridPart> computerPath = RunMove(startPos, field, dijkstra, false);
            



            //Debug.Log("Player shortest path " + playerPath.Count);


            
            if (playerPath.Count >= computerPath.Count)
            
                return computerPath.Dequeue();
            else
            {
                Debug.Log("Need to place a Wall. Opponent is winning. His shortest path is " +playerPath.Count );
                return computerPath.Dequeue();

            }
            


        }

        public Queue<MoveGridPart> RunMove(MoveGridPart startPos, List<MoveGridPart> field, Pathfinding dijkstra, bool isPlayerPawn)
        {
            int finalY = 1;

            if (isPlayerPawn)
            {
                finalY = 9;
                startPos.IsWithPawn = false;
            }



            List<MoveGridPart> finalPositions = new List<MoveGridPart>();

            for (int x = 1; x <= 9; x++)
            {
                finalPositions.Add(field.Find(gridPart => gridPart.GridPos.y == finalY && gridPart.GridPos.x == x));
            }

            int minPathCount = Int32.MaxValue;
            Queue<MoveGridPart> path = new Queue<MoveGridPart>();


            foreach (MoveGridPart finalPosition in finalPositions)
            {
                Queue<MoveGridPart> pathToCheck = dijkstra.Dijkstra3(finalPosition, startPos, field);

                if (pathToCheck.Count < minPathCount)
                {
                    minPathCount = pathToCheck.Count;
                    path = pathToCheck;
                }

            }

            if(isPlayerPawn)
                startPos.IsWithPawn = true;
            return path;
        }

    }
}



#region minimax
/*
public float minimax(MoveGridPart position, int depth, int alpha, int beta, bool maximizongPlayer)
{

    if (depth == 0 || game.isOver)
        return static evaluation of position;

    if (maximizingPlayer)
    {
        int maxEval = Int16.MinValue;
        foreach (child of position)          //child of position - position that can be achieved by a single move 
            eval = minimax(child, depth - 1, alpha, beta, false)
            maxEval = max(maxEval, eval)
            alpha = max(apha, eval)
            if (beta <= alpha)
            break;
        return maxEval;
    }
    else
    {
        minEval = Int16.MaxValue;
        foreach (child of posityin)
            eval = minimax(child, depth - 1, alpha, beta, true);
        minEval = min(minEval, eval)
            beta = min(beta, eval)
            if (beta <= alpha)
            break;
        return minEval;
    }


}*/
#endregion
