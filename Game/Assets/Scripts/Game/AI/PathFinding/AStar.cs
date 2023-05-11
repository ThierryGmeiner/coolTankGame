using System;
using UnityEngine;

namespace Game.AI
{
    public class AStar
    {
        private readonly AStarGrid grid;

        public AStarNode startNode { get; private set; }
        public AStarNode targetNode { get; private set; }

        public AStar(AStarGrid grid) {
            this.grid = grid;
        }

        public AStarNode[] GetPath(Vector3 startPos, Vector3 targetPos) {
            AStarNode[] path;
            startNode = grid.GetNodeFromPosition(startPos);
            targetNode = grid.GetNodeFromPosition(targetPos);

            // get simple path
            // simplify the way (via rays)
            // convert the simplified way to an array

            grid.Clear();
            return new AStarNode[0];
        }

        private AStarNode[] GetAStarPath() {
            AStarNode currentNode = startNode;

            while (currentNode != targetNode) {
                UpdateNeighbors(currentNode);
            }

            // get the path
            //    => let all nodes point back to the previous node

            return new AStarNode[0];
        }

        private void UpdateNeighbors(AStarNode currentNode) {
            Vector2Int[] newPositions = SuroundingNodes(currentNode.ArrayIndex);

            foreach(Vector2Int pos in newPositions) {
                if (AStarHelper.NodeIsOutsideOfGrid(pos, grid)) continue;
                else UpdateNode(grid.Grid[pos.x, pos.y], currentNode);
            }
        }

        public void UpdateNode(AStarNode node, AStarNode updatingNeighbor) {
            node.hCost = AStarHelper.CalculateHCost(node, targetNode);

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