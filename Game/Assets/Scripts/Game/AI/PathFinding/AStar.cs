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

        public AStarNode[] GetPath(Vector3 startPos, Vector3 targetPos) {
            StartNode = grid.GetNodeFromPosition(startPos);
            TargetNode = grid.GetNodeFromPosition(targetPos);

            // get simple path
            // simplify the way (via rays)
            // convert the simplified way to an array

            AStarNode[] path = GetAStarPath();


            grid.Clear();
            return new AStarNode[0];
        }

        public AStarNode[] GetAStarPath() {
            List<AStarNode> path = new List<AStarNode>();
            AStarNode currentNode = StartNode;

            while (currentNode != TargetNode) {
                UpdateNeighbors(currentNode);
                currentNode = grid.GetCheapestNode();
            }



            return path.ToArray();
        }

        private void UpdateNeighbors(AStarNode currentNode) {
            Vector2Int[] neighborPositions = SuroundingNodes(currentNode.ArrayIndex);

            foreach(Vector2Int position in neighborPositions) {
                if (AStarHelper.NodeIsOutsideOfGrid(position, grid)) continue;
                
                UpdateNode(grid.Grid[position.x, position.y], currentNode);
                grid.Grid[position.x, position.y].AllNeighborsAreDiscovered = true;
            }
        }

        public void UpdateNode(AStarNode node, AStarNode updatingNeighbor) {
            node.hCost = AStarHelper.CalculateHCost(node, TargetNode);

            int gCost = AStarHelper.CalculateGCost(node, updatingNeighbor);
            if (NewCostIsLower(gCost, node.gCost) || OldCostIsUndefined(node.gCost)) {
                node.LastNodeInPath = updatingNeighbor;
                node.gCost = gCost;
            }
        }

        private static bool NewCostIsLower(int newCost, int oldCost) => newCost < oldCost;
        private static bool OldCostIsUndefined(int oldCost) => oldCost == 0;
        private static Vector2Int[] SuroundingNodes(Vector2Int olsPos) {
            Vector2Int[] array = { new Vector2Int(olsPos.x - 1, olsPos.y), new Vector2Int(olsPos.x + 1, olsPos.y),
                                        new Vector2Int(olsPos.x, olsPos.y - 1), new Vector2Int(olsPos.x, olsPos.y + 1),
                                        new Vector2Int(olsPos.x - 1, olsPos.y - 1), new Vector2Int(olsPos.x + 1, olsPos.y - 1),
                                        new Vector2Int(olsPos.x - 1, olsPos.y + 1), new Vector2Int(olsPos.x + 1, olsPos.y + 1), };
            return array;
        }
    }
}