using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public static class AStarHelper {

        public const int STRAIGHT_MOVE_COST = 10;
        public const int DIAGOANAL_MOVE_COST = 14;

        public static void UpdateNode(AStarNode node, AStarNode updatingNeighbor, AStarNode target) {
            node.hCost = CalculateHCost(node, target);

            int gCost = CalculateGCost(node, updatingNeighbor);
            if (NewCostIsLower(gCost, node.gCost) || OldCostIsUndefined(node.gCost)) {
                node.LastNodeInPath = updatingNeighbor;
                node.gCost = gCost;
            }
        }

        public static int CalculateGCost(AStarNode node, AStarNode updatingNode) {
            int gCostIncreas = NodeIsStraightToNeighbor(node, updatingNode) ? STRAIGHT_MOVE_COST : DIAGOANAL_MOVE_COST;
            return updatingNode.gCost + gCostIncreas;
        }

        public static bool NodeIsStraightToNeighbor(AStarNode node, AStarNode updatingNode)
            => node.Position.x == updatingNode.Position.x || node.Position.y == updatingNode.Position.y;

        public static int CalculateHCost(AStarNode node, AStarNode targetNode) {
            if (node.hCost > 0) return node.hCost;

            int distanceX = Math.Abs(Mathf.RoundToInt(targetNode.Position.x - node.Position.x));
            int distanceY = Math.Abs(Mathf.RoundToInt(targetNode.Position.y - node.Position.y));
            int diagonalSteps = Math.Min(distanceX, distanceY);
            int straightSteps = Math.Max(distanceX, distanceY) - diagonalSteps;
            int hCost = straightSteps * STRAIGHT_MOVE_COST + diagonalSteps * DIAGOANAL_MOVE_COST;
            return hCost;
        }

        public static List<AStarNode> GetPathViaBacktracking(AStarNode startNode, AStarNode lastNode) {
            List<AStarNode> path = new List<AStarNode>();
            if (lastNode == startNode) {
                path.Add(lastNode);
                return path;
            }
            path = GetPathViaBacktracking(startNode, lastNode.LastNodeInPath);
            path.Add(lastNode);
            return path;
        }

        private static bool NewCostIsLower(int newCost, int oldCost) => newCost < oldCost;
        private static bool OldCostIsUndefined(int oldCost) => oldCost == 0;
        public static bool NodeIsOutsideOfGrid(AStarNode node, AStarGrid grid) => NodeIsOutsideOfGrid(node.ArrayIndex, grid);
        public static bool NodeIsOutsideOfGrid(Vector2Int arrayIndex, AStarGrid grid)
            => arrayIndex.x < 0 || arrayIndex.y < 0 || arrayIndex.x >= grid.Grid.GetLength(0) || arrayIndex.y >= grid.Grid.GetLength(1);
    }
}