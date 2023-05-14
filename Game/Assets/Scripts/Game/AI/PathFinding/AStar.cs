using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class AStar
    {
        private readonly AStarGrid grid;
        private AStarNode startNode;
        private AStarNode targetNode;

        public AStarNode StartNode { get => startNode; set { if (value.IsWalkable) startNode = value; } }
        public AStarNode TargetNode { get => targetNode; set { if (value.IsWalkable) targetNode = value; } }

        public AStar(AStarGrid grid) {
            this.grid = grid;
        }

        public AStarNode[] FindPath(Vector3 startPos, Vector3 targetPos) {
            startNode = grid.GetNodeFromPosition(startPos);
            targetNode = grid.GetNodeFromPosition(targetPos);

            AStarNode currentNode = startNode;
            int count = 0;
            while (currentNode != targetNode && count < 1000) {
                UpdateNeighbors(currentNode);
                currentNode = grid.GetCheapestNode();
                count++;
            }
            return GetPathViaBacktracking(startNode, targetNode).ToArray();
        }

        public void UpdateNeighbors(AStarNode currentNode) {
            Vector2Int[] neighborPositions = SuroundingNodes(currentNode.ArrayIndex);
            currentNode.AllNeighborsAreDiscovered = true;

            foreach (Vector2Int position in neighborPositions) {
                if (!currentNode.IsWalkable || NodeIsOutsideOfGrid(position, grid)) continue;
                UpdateNode(grid.Grid[position.x, position.y], currentNode, targetNode);
            }
        }

        private static Vector2Int[] SuroundingNodes(Vector2Int olsPos) {
            Vector2Int[] array = { new Vector2Int(olsPos.x - 1, olsPos.y), new Vector2Int(olsPos.x + 1, olsPos.y),
                                        new Vector2Int(olsPos.x, olsPos.y - 1), new Vector2Int(olsPos.x, olsPos.y + 1),
                                        new Vector2Int(olsPos.x - 1, olsPos.y - 1), new Vector2Int(olsPos.x + 1, olsPos.y - 1),
                                        new Vector2Int(olsPos.x - 1, olsPos.y + 1), new Vector2Int(olsPos.x + 1, olsPos.y + 1), };
            return array;
        }


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
            int distanceY = Math.Abs(Mathf.RoundToInt(targetNode.Position.z - node.Position.z));
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