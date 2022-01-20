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
        public List<List<MoveGridPart>> GetFieldStatesForRunMoves(MoveGridPart curPosition, List<MoveGridPart> curField)
        {
            List<MoveGridPart> neighbours = new List<MoveGridPart>();
            var move = PossibleMove.GetPossibleMoves(curField, curPosition);

            foreach (Vector2Int v in move)
            {
                MoveGridPart g = curField.Find(x => x.GridPos.x == v.x && x.GridPos.y == v.y);
                if (g != null)
                    neighbours.Add(g);
            }

            List<List<MoveGridPart>> list = new List<List<MoveGridPart>>();


            foreach (MoveGridPart possibleMove in neighbours)
            {
                List<MoveGridPart> copy = new List<MoveGridPart>(curField);

                copy.Find(x => x == curPosition).IsWithPawn = false;
                copy.Find(x => x == possibleMove).IsWithPawn = true;
                list.Add(copy);
            }


            return list;

        }


        #region


        MoveGridPart selectedMove = new MoveGridPart();
        List<MoveGridPart> selectedFieldState = new List<MoveGridPart>();

        public int minimax(MoveGridPart curPos, List<MoveGridPart> position, int depth, bool maximizingPlayer)
        {

            if (depth == 0) //|| game.isOver)
            {
                //selectedFieldState = fieldStates;
                return Evaluation(position, curPos, false);    //return 
            }

            if (maximizingPlayer)
            {
                int maxEval = Int16.MinValue;
                foreach (List<MoveGridPart> child in GetFieldStatesForRunMoves(curPos, position))                  //perhabs position is a field    //child of position - position that can be achieved by a single move. Moves + walls
                {
                    MoveGridPart childPawnPos = child.FindLast(p => p.IsWithPawn == true);
                    int eval = minimax(childPawnPos, child, depth - 1, false);
                    if (eval > maxEval)
                        selectedFieldState = child;      //do we need it?

                    maxEval = Math.Max(maxEval, eval);
                }
                return maxEval;
            }
            else
            {
                int minEval = Int16.MaxValue;
                foreach (List<MoveGridPart> child in GetFieldStatesForRunMoves(curPos, position))
                {
                    MoveGridPart childPawnPos = child.Find(p => p.IsWithPawn == true);
                    int eval = minimax(childPawnPos, child, depth - 1, true);
                    if (eval < minEval)
                        selectedFieldState = child;

                    minEval = Math.Min(minEval, eval);
                }

                return minEval;
            }


        }



        int Evaluation(List<MoveGridPart> field, MoveGridPart curPosition, bool isPlayerPawn)
        {

            Pathfinding dijkstra = new Pathfinding();
            Queue<MoveGridPart> path = RunMove(curPosition, field, dijkstra, isPlayerPawn);
            //stepsToFinish = position.howManyGridPartsToFinish;
            //but reverse. less moves to finish = better.

            int stepsToFinish = 81 - path.Count;

            Debug.Log("Steps to finish: " + stepsToFinish);
            return stepsToFinish;
        }




        #endregion


        public MoveGridPart NextMove(MoveGridPart startPos, List<MoveGridPart> field, bool isPlayerPawn = false)
        {
            Pathfinding dijkstra = new Pathfinding();

            MoveGridPart opponentPos = field.Find(grid => grid.IsWithPawn && grid != startPos);
            MoveGridPart testGrid = field.Find(grid => grid.GridPos.x == opponentPos.GridPos.x && grid.GridPos.y == opponentPos.GridPos.y);
            Queue<MoveGridPart> computerPath = RunMove(startPos, field, dijkstra, false);


            return computerPath.Dequeue();
        }

        public Queue<MoveGridPart> RunMove(MoveGridPart startPos, List<MoveGridPart> field, Pathfinding dijkstra, bool isPlayerPawn)
        {
            int finalY = 1;

            if (isPlayerPawn)
            {
                startPos.IsWithPawn = false;

                finalY = 9;
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

            if (isPlayerPawn)
            {
                startPos.IsWithPawn = true;
            }

            return path;
        }


        public Queue<MoveGridPart> RunMove(MoveGridPart startPos, List<MoveGridPart> field, Pathfinding dijkstra, bool isPlayerPawn, int y)
        {
            int finalY = y;

            startPos.IsWithPawn = false;

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

                if (pathToCheck == null)
                    continue;

                if (pathToCheck.Count < minPathCount)
                {
                    minPathCount = pathToCheck.Count;
                    path = pathToCheck;
                }

            }

            startPos.IsWithPawn = true;

            return path;
        }
    }
}
