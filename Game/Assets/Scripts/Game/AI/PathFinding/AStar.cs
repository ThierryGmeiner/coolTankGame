using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class AStar
    {
        private readonly AStarGrid grid;

        public AStarNode StartNode { get; set; }
        public AStarNode TargetNode { get; set; }

        public AStar(AStarGrid grid) {
            this.grid = grid;
        }

        public AStarNode[] GetOptimizedPath(Vector2 startPos, Vector2 targetPos) {
            StartNode = grid.GetNodeFromPosition(startPos);
            TargetNode = grid.GetNodeFromPosition(targetPos);

            // get simple path
            // simplify the way (via rays)
            // convert the simplified way to an array

            AStarNode[] path = GetAStarPath();


            grid.Clear();
            return new AStarNode[0];
        }

        public AStarNode[] GetAStarPath(Vector2 startPos, Vector2 targetPos) {
            StartNode = grid.GetNodeFromPosition(startPos);
            Debug.Log("targetPos: " + targetPos);
            TargetNode = grid.GetNodeFromPosition(targetPos);
            return GetAStarPath();
        }

        private AStarNode[] GetAStarPath() {
            List<AStarNode> path = new List<AStarNode>();
            AStarNode currentNode = StartNode;

            int iteration = 0;

            while (currentNode != TargetNode && iteration < 100) {
                Debug.Log(currentNode.ArrayIndex);
                UpdateNeighbors(currentNode);
                currentNode = grid.GetCheapestNode();
                iteration++;
            }
            return AStarHelper.GetPathViaBacktracking(StartNode, TargetNode).ToArray();
        }

        private void UpdateNeighbors(AStarNode currentNode) {
            Vector2Int[] neighborPositions = SuroundingNodes(currentNode.ArrayIndex);

            foreach(Vector2Int position in neighborPositions) {
                if (AStarHelper.NodeIsOutsideOfGrid(position, grid)) continue;
                
                AStarHelper.UpdateNode(grid.Grid[position.x, position.y], currentNode, TargetNode);
                grid.Grid[position.x, position.y].AllNeighborsAreDiscovered = true;
            }
            Debug.Log("updateNeighbors is finished");
        }

        private static Vector2Int[] SuroundingNodes(Vector2Int olsPos) {
            Vector2Int[] array = { new Vector2Int(olsPos.x - 1, olsPos.y), new Vector2Int(olsPos.x + 1, olsPos.y),
                                        new Vector2Int(olsPos.x, olsPos.y - 1), new Vector2Int(olsPos.x, olsPos.y + 1),
                                        new Vector2Int(olsPos.x - 1, olsPos.y - 1), new Vector2Int(olsPos.x + 1, olsPos.y - 1),
                                        new Vector2Int(olsPos.x - 1, olsPos.y + 1), new Vector2Int(olsPos.x + 1, olsPos.y + 1), };
            return array;
        }
    }
}