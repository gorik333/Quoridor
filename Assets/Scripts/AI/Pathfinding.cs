using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//using Priority_Queue;

namespace Assets.Scripts.AI
{
    public class Pathfinding
    {



        public void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            //GridPart startNode = startPos;
            //GridPart targetNode = targetPos;

            List<MoveGridPart> openSet = new List<MoveGridPart>();
            HashSet<MoveGridPart> closeSet = new HashSet<MoveGridPart>();

            //openSet.Add(startNode);

            //while(openSet.Count > 0)
            {

            }




        }




        public void Dijkstra(List<MoveGridPart> gridParts, MoveGridPart currentPos)
        {

            List<MoveGridPart> visited = new List<MoveGridPart>();
            List<MoveGridPart> unvisited = new List<MoveGridPart>(gridParts.Where(x=> x != currentPos));

            MoveGridPart current = currentPos;

            List<MoveGridPart> finalVerticles = new List<MoveGridPart>(gridParts.Where(x => x.GridPos.y == 1));

            Queue<MoveGridPart> Queue = new Queue<MoveGridPart>();
            

            foreach(MoveGridPart g in unvisited)
            {


                foreach(MoveGridPart neighbour in GetNeighbours(current, gridParts))
                {


                }


            }



        }




        /*
        private List<MoveGridPart> Dijkstra2(List<MoveGridPart> gridParts, MoveGridPart currentPos)
        {


            List<MoveGridPart> openList = new List<MoveGridPart>() { currentPos };
            List<MoveGridPart> closedList = new List<MoveGridPart>();
            List<MoveGridPart> endNodeList = new List<MoveGridPart>(gridParts.Where(x => x.GridPos.y == 1));
            MoveGridPart endNode = new MoveGridPart();

            while (openList.Count > 0)
            {
                MoveGridPart currentNode = openList[0];
                if (endNodeList.Contains(currentNode))
                {
                    //reaches final node
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach(MoveGridPart neighbourNode in GetNeighbours(currentNode, gridParts))
                {
                    if (closedList.Contains(neighbourNode)) continue;

                    //int cost = Calculate



                }


            }



        }

        */

        private List<MoveGridPart> CalculatePath(MoveGridPart endNode)
        {
            return null;
        }





        public Queue<MoveGridPart> Dijkstra3(MoveGridPart startPos, MoveGridPart goalPos, List<MoveGridPart> gridPArts)
        {
            Dictionary<MoveGridPart, MoveGridPart> nextPosToGoal = new Dictionary<MoveGridPart, MoveGridPart>();
            Dictionary<MoveGridPart, int> costToReachPos = new Dictionary<MoveGridPart, int>();

            PriorityQueue<MoveGridPart> frontier = new PriorityQueue<MoveGridPart>();

            //Debug.Log("Start position: " + startPos.GridPos.x + " " + startPos.GridPos.y);
            

            frontier.Enqueue(startPos, 0);
            costToReachPos[startPos] = 0;

            while(frontier.Count > 0)
            {
                MoveGridPart curPos = frontier.Dequeue();

                if (curPos == goalPos)
                    break;

                List<MoveGridPart> neigbours = GetNeighbours(curPos, gridPArts);
                foreach(MoveGridPart neighbour in neigbours)
                {
                    //Debug.Log("Cost " + neighbour.Cost);

                    int newCost = costToReachPos[curPos] + neighbour.Cost;

                    if(costToReachPos.ContainsKey(neighbour) == false || newCost < costToReachPos[neighbour])
                    {
                        costToReachPos[neighbour] = newCost;
                        int priority = newCost;
                        frontier.Enqueue(neighbour, priority);
                        nextPosToGoal[neighbour] = curPos;
                        //neighbour.Text = costToReachTile[neighbour].ToString();
                    }
                }
            }
            if (nextPosToGoal.ContainsKey(goalPos) == false)
                return null;

            Queue<MoveGridPart> path = new Queue<MoveGridPart>();
            MoveGridPart curPathTile = goalPos;
            while(curPathTile != startPos)
            {
                curPathTile = nextPosToGoal[curPathTile];
                path.Enqueue(curPathTile);
            }


            //foreach(MoveGridPart h in path)
            //{
            //    Debug.Log(h.GridPos.x + "  ---   " + h.GridPos.y);
            //}
            return path;
        }







		
        public List<MoveGridPart> GetNeighbours(MoveGridPart current, List<MoveGridPart> gridPArts)
        {
            List<MoveGridPart> neighbours = new List<MoveGridPart>();
            //MoveGridPart c = gridPArts.Find(grid => grid.GridPos.x == 4 && grid.GridPos.y == 1);
            var move = PossibleMove.GetPossibleMoves(gridPArts, current);

            
            foreach(Vector2Int v in move)
            {
                MoveGridPart g = gridPArts.Find(x => x.GridPos.x == v.x && x.GridPos.y == v.y);
                if(g != null)
                    neighbours.Add(g);
            }
            
            /*
            //if(current.neighbours.isNotBlocked)
            MoveGridPart Right = gridPArts.Where(part => part.GridPos.x == current.GridPos.x + 1 && part.GridPos.y == current.GridPos.y).FirstOrDefault();
            MoveGridPart Left = gridPArts.Where(part => part.GridPos.x == current.GridPos.x - 1 && part.GridPos.y == current.GridPos.y).FirstOrDefault();
            MoveGridPart Buttom = gridPArts.Where(part => part.GridPos.x == current.GridPos.x && part.GridPos.y == current.GridPos.y - 1).FirstOrDefault();
            MoveGridPart Top = gridPArts.Where(part => part.GridPos.x == current.GridPos.x && part.GridPos.y == current.GridPos.y + 1).FirstOrDefault();

            if (Right != null)
                neighbours.Add(Right);

            if (Left != null)
                neighbours.Add(Left);

            if (Buttom != null)
                neighbours.Add(Buttom);

            if (Top != null)
                neighbours.Add(Top);
            */
            



            return neighbours;
        }





	}
}
