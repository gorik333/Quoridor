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


        public MoveGridPart NextMove(MoveGridPart startPos , List<MoveGridPart> field, bool isPlayerPawn = false)
        {
            Pathfinding dijkstra = new Pathfinding();
            //Debug.Log("Test grid pos: " + opponentPos.GridPos.x + ", " + opponentPos.GridPos.y);

            MoveGridPart opponentPos = field.Find(grid => grid.IsWithPawn && grid != startPos);
            MoveGridPart testGrid = field.Find(grid => grid.GridPos.x == opponentPos.GridPos.x && grid.GridPos.y == opponentPos.GridPos.y);
            //Queue<MoveGridPart> playerPath = RunMove(testGrid, field, dijkstra, true);
            Queue<MoveGridPart> computerPath = RunMove(startPos, field, dijkstra, false);
            



            //Debug.Log("Player shortest path " + computerPath.Count);
            //Debug.Log("Start position " + opponentPos.GridPos.x + ", " + opponentPos.GridPos.y);
            //return computerPath.Dequeue(); //field.Find(x => x.GridPos.x == startPos.GridPos.x && x.GridPos.y == startPos.GridPos.y - 1);

            
            //if (playerPath.Count >= computerPath.Count)
            
                return computerPath.Dequeue();
            //else
            //{
             //   Debug.Log("Need to place a Wall. Opponent is winning");
             //   return computerPath.Dequeue();

            //}
            


        }

        public Queue<MoveGridPart> RunMove(MoveGridPart startPos, List<MoveGridPart> field, Pathfinding dijkstra, bool isPlayerPawn)
        {
            int finalY = 1;

            if (isPlayerPawn)
                finalY = 9;


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


            return path;
        }

    }
}
