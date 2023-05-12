using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public static class AStarHelper {

        public const int STRAIGHT_MOVE_COST = 10;
        public const int DIAGOANAL_MOVE_COST = 14;

        public static int CalculateGCost(AStarNode node, AStarNode updatingNeighbor) {
            int gCostIncreas = NodeIsStraightToNeighbor(node, updatingNeighbor) ? STRAIGHT_MOVE_COST : DIAGOANAL_MOVE_COST;
            int newGCost = updatingNeighbor.gCost + gCostIncreas;
            return newGCost;
        }

        public static bool NodeIsStraightToNeighbor(AStarNode node, AStarNode updatingNeighbor)
            => node.Position.x == updatingNeighbor.Position.x || node.Position.y == updatingNeighbor.Position.y;

        public static int CalculateHCost(AStarNode node, AStarNode targetNode) {
            if (node.hCost > 0) return node.hCost;

            int distanceX = Math.Abs(Mathf.RoundToInt(targetNode.Position.x - node.Position.x));
            int distanceY = Math.Abs(Mathf.RoundToInt(targetNode.Position.y - node.Position.y));
            int diagonalSteps = Math.Min(distanceX, distanceY);
            int straightSteps = Math.Max(distanceX, distanceY) - diagonalSteps;
            int hCost = straightSteps * STRAIGHT_MOVE_COST + diagonalSteps * DIAGOANAL_MOVE_COST;
            return hCost;
        }

        public AStarNode[] BackTrackPath(AStarGrid grid, AStarNode startNode, AStarNode targetNode) {
            List<AStarNode> path = new List<AStarNode>();

            // recursion ?????!!!!!
            for ()

            return path.ToArray();
        }

        public static bool NodeIsOutsideOfGrid(AStarNode node, AStarGrid grid) => NodeIsOutsideOfGrid(node.ArrayIndex, grid);
        public static bool NodeIsOutsideOfGrid(Vector2Int arrayIndex, AStarGrid grid)
            => arrayIndex.x < 0 || arrayIndex.y < 0 || arrayIndex.x >= grid.Grid.GetLength(0) || arrayIndex.y >= grid.Grid.GetLength(1);
    }
}