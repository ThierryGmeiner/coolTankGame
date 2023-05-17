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
        public const int STRAIGHT_MOVE_COST = 10;
        public const int DIAGOANAL_MOVE_COST = 14;
     
        public AStar(AStarGrid grid) {
            this.grid = grid;
        }

        public AStarNode StartNode { get => startNode; set { if (value.IsWalkable) startNode = value; } }
        public AStarNode TargetNode { get => targetNode; set { if (value.IsWalkable) targetNode = value; } }

        public AStarNode[] FindPath(Vector3 startPos, Vector3 targetPos)
            => FindPath(grid.GetNodeFromPosition(startPos), grid.GetNodeFromPosition(targetPos));

        // finds a path with the aStar algorithm
        public AStarNode[] FindPath(AStarNode start, AStarNode target) {
            startNode = start;
            targetNode = target;
            AStarNode currentNode = startNode;

            if (!targetNode.IsWalkable || !startNode.IsWalkable) return new AStarNode[0];
            // search every loop the cheapest node and update them
            for (int i = 0; currentNode != targetNode && i < 3000; i++) {
                UpdateNeighbors(currentNode);
                currentNode = grid.GetCheapestNode();
            }

            AStarNode[] path = GetPathViaBacktracking(startNode, targetNode).ToArray();
            grid.Clear();
            return path;
        }
        
        // filters out the important nodes of the aStar path
        public AStarNode[] FindOptimizedPath(AStarNode start, AStarNode target) {
            AStarNode[] path = FindPath(start, target);
            List<AStarNode> optimizedPath = new List<AStarNode>();
            AStarNode currentSectionStart = startNode;

            if (path.Length == 0) return path;
            // search in every loop one section
            while (currentSectionStart != targetNode) {
                currentSectionStart = FindNewSection(path, currentSectionStart);
                optimizedPath.Add(currentSectionStart);
            }
            return optimizedPath.ToArray();
        }

        private AStarNode FindNewSection(AStarNode[] path, AStarNode oldSectionStart) {
            AStarNode newSectionStart = FidnSectionViaBacktracking(path, oldSectionStart, Array.IndexOf(path, oldSectionStart) + 1);
            // fixes the problem that the search sometimes gets stuck on a node
            if (oldSectionStart == newSectionStart) newSectionStart = path[Array.IndexOf(path, newSectionStart) + 1];
            return newSectionStart;
        }

        private AStarNode FidnSectionViaBacktracking(AStarNode[] path, AStarNode searchNode, int index) {
            if (path[index] == targetNode) return path[index];
            if (Physics.Linecast(searchNode.Position, path[index].Position, grid.unwalkableMask)) return path[index - 1];
            return FidnSectionViaBacktracking(path, searchNode, index + 1);
        }

        public void UpdateNeighbors(AStarNode currentNode) {
            Vector2Int[] neighborPositions = SuroundingNodes(currentNode.ArrayIndex);
            currentNode.AllNeighborsAreDiscovered = true;

            foreach (Vector2Int position in neighborPositions) {
                if (!currentNode.IsWalkable || NodeIsOutsideOfGrid(position, grid)) continue;
                UpdateNode(grid.Grid[position.x, position.y], currentNode, targetNode);
            }
        }

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

        private static Vector2Int[] SuroundingNodes(Vector2Int olsPos) {
            Vector2Int[] array = { new Vector2Int(olsPos.x - 1, olsPos.y), new Vector2Int(olsPos.x + 1, olsPos.y),
                                        new Vector2Int(olsPos.x, olsPos.y - 1), new Vector2Int(olsPos.x, olsPos.y + 1),
                                        new Vector2Int(olsPos.x - 1, olsPos.y - 1), new Vector2Int(olsPos.x + 1, olsPos.y - 1),
                                        new Vector2Int(olsPos.x - 1, olsPos.y + 1), new Vector2Int(olsPos.x + 1, olsPos.y + 1), };
            return array;
        }

        private static bool NewCostIsLower(int newCost, int oldCost) => newCost < oldCost;
        private static bool OldCostIsUndefined(int oldCost) => oldCost == 0;
        public static bool NodeIsOutsideOfGrid(AStarNode node, AStarGrid grid) => NodeIsOutsideOfGrid(node.ArrayIndex, grid);
        public static bool NodeIsOutsideOfGrid(Vector2Int arrayIndex, AStarGrid grid)
            => arrayIndex.x < 0 || arrayIndex.y < 0 || arrayIndex.x >= grid.Grid.GetLength(0) || arrayIndex.y >= grid.Grid.GetLength(1);
    }
}