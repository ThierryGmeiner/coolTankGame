using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.AI
{
    public class AStar
    {
        private AStarNode startNode;
        private AStarNode targetNode;
        private readonly AStarGrid grid;
        private readonly float colliderRadius = 0.5f;
        public const float STRAIGHT_MOVE_COST = 10;
        public const float DIAGOANAL_MOVE_COST = 14;
     
        public AStar(AStarGrid grid) {
            this.grid = grid;
            colliderRadius = grid.NodeRadius;
        }

        public AStarNode StartNode { 
            get { return startNode; } 
            set { 
                if (value.IsWalkable) startNode = value;
                else Debug.LogError($"new {nameof(startNode)} isn't walkable");
            }
        }

        public AStarNode TargetNode { 
            get { return targetNode; } 
            set {
                if (value.IsWalkable) targetNode = value;
                else Debug.LogError($"new {nameof(targetNode)} isn't walkable");
            } 
        }

        public AStarGrid Grid { get { return grid; } }

        // ####################################################
        // find path:
        // finde path with the a* algorithm
        // ####################################################
        
        public Path FindPath(Vector3 startPos, Vector3 targetPos) 
            => FindPath(grid.GetNodeFromPosition(startPos), grid.GetNodeFromPosition(targetPos));

        // finds a path with the aStar algorithm
        public Path FindPath(AStarNode start, AStarNode target) {
            startNode = start; targetNode = target;
            AStarNode currentNode = startNode;

            if (StartOrTargetNodeIsNotValide()) return new Path(new AStarNode[0], false);
            // search every loop the cheapest node and update them
            for (int i = 0; currentNode != targetNode && i < 10000; i++) {
                UpdateNeighbors(currentNode);
                currentNode = grid.GetCheapestNode();
            }
            Path path = new Path(GetPath(startNode, targetNode).ToArray(), false);
            grid.Clear();
            return path;
        }

        public List<AStarNode> GetPath(AStarNode startNode, AStarNode lastNode) {
            List<AStarNode> path = new List<AStarNode>();
            if (lastNode == startNode) {
                path.Add(lastNode);
                return path;
            }
            path = GetPath(startNode, lastNode.LastNodeInPath);
            path.Add(lastNode);
            return path;
        }

        public void UpdateNeighbors(AStarNode currentNode) {
            AStarNode[] neighbors = Get8Neighbor(currentNode).ToArray();
            currentNode.AllNeighborsAreDiscovered = true;

            foreach (AStarNode neighbor in neighbors) {
                if (!currentNode.IsWalkable) continue;
                UpdateNode(neighbor, currentNode, targetNode);
            }
        }

        public void UpdateNode(AStarNode node, AStarNode updatingNeighbor, AStarNode target) {
            node.hCost = CalculateHCost(node, target);
            float gCost = CalculateGCost(node, updatingNeighbor);
            if (NewCostIsLower(gCost, node.gCost) || OldCostIsUndefined(node.gCost)) {
                node.LastNodeInPath = updatingNeighbor;
                node.gCost = gCost;
            }
        }

        public float CalculateGCost(AStarNode node, AStarNode updatingNode) {
            float gCostIncreas = NodeIsStraightToNeighbor(node, updatingNode) ? STRAIGHT_MOVE_COST : DIAGOANAL_MOVE_COST;
            return updatingNode.gCost + gCostIncreas;
        }

        public float CalculateHCost(AStarNode node, AStarNode targetNode) {
            if (node.hCost > 0) return node.hCost;

            int distanceX = Math.Abs(Mathf.RoundToInt(targetNode.Position.x - node.Position.x));
            int distanceY = Math.Abs(Mathf.RoundToInt(targetNode.Position.z - node.Position.z));
            int diagonalSteps = Math.Min(distanceX, distanceY);
            int straightSteps = Math.Max(distanceX, distanceY) - diagonalSteps;
            float hCost = straightSteps * STRAIGHT_MOVE_COST + diagonalSteps * DIAGOANAL_MOVE_COST;
            return hCost;
        }

        // ####################################################
        // Optimize path:
        // filters the important nodes of the aStar path out
        // ####################################################

        public Path FindOptimizedPath(Vector3 start, Vector3 target)
            => FindOptimizedPath(grid.GetNodeFromPosition(start), grid.GetNodeFromPosition(target));

        public Path FindOptimizedPath(AStarNode start, AStarNode target)  => FindOptimizedPath(FindPath(start, target));

        public Path FindOptimizedPath(Path unoptimizedPath) {
            List<AStarNode> optimizedPath = new List<AStarNode>();
            AStarNode currentSectionStart = startNode;

            if (unoptimizedPath.Nodes.Length == 0) return new Path(unoptimizedPath.Nodes, true);

            // search in every loop one section
            while (currentSectionStart != targetNode) {
                currentSectionStart = FindNewSection(unoptimizedPath.Nodes, currentSectionStart);
                optimizedPath.Add(currentSectionStart);
            }
            return new Path(optimizedPath.ToArray(), true);
        }

        private AStarNode FindNewSection(AStarNode[] path, AStarNode oldSectionStart) {
            AStarNode newSectionStart = GetNextSection(path, oldSectionStart, Array.IndexOf(path, oldSectionStart) + 1);

            // fixes the bug when the search sometimes gets stuck on a node
            if (oldSectionStart == newSectionStart) newSectionStart = path[Array.IndexOf(path, newSectionStart) + 1];
            return newSectionStart;
        }

        private AStarNode GetNextSection(AStarNode[] path, AStarNode searchNode, int index) {
            if (path[index] == targetNode) 
                return path[index];
            if (Magic.Conditions.DirectPathIsBlocked(path[index].Position, searchNode.Position, colliderRadius, grid.unwalkableMask)) 
                return path[index - 1];
            return GetNextSection(path, searchNode, index + 1);
        }

        // ####################################################
        // some public functions:
        // ####################################################

        public Path CnvertWayPointsToPaths(Transform[] wayPoints) {
            Path finalPath = new Path(new AStarNode[0], optimized: true);
            for (int i = 0; i < wayPoints.Length; i++) {
                Vector3 start = wayPoints[i].position;
                Vector3 target = i + 1 < wayPoints.Length ? wayPoints[i + 1].position : wayPoints[0].position;
                finalPath = finalPath + FindOptimizedPath(start, target);
            }
            return finalPath;
        }

        public AStarNode GetCoveredNode(Vector3 startPos, GameObject enemy, int fillValue) 
            => GetCoveredField(grid.GetNodeFromPosition(startPos), enemy, fillValue);

        public AStarNode GetCoveredField(AStarNode startPos, GameObject enemy, int fillValue) {
            List<AStarNode> currentNodes = new List<AStarNode>() { startPos };
            List<AStarNode> nextNodes;

            // loop through iterations
            while (fillValue > 0) {
                nextNodes = new List<AStarNode>();
                fillValue--;

                // loop throu activeNodes and search siutable node
                foreach (AStarNode node in currentNodes) {
                    if (IsNextToCover(node) && Physics.Linecast(node.Position, enemy.transform.position, grid.unwalkableMask)) {
                        grid.Clear();
                        return node;
                    }
                    // get new neighbors
                    List<AStarNode> neighborNodes = FloodFillNeighbors(node, fillValue);
                    foreach (AStarNode neighbor in neighborNodes) {
                        nextNodes.Add(neighbor);
                    }
                } 
                currentNodes = nextNodes;
            }

            grid.Clear();
            return startPos;
        }

        private bool IsNextToCover(AStarNode node) {
            AStarNode[] neighbors = Get4Neighbors(node).ToArray();

            foreach (AStarNode neigbhor in neighbors) {
                if (!neigbhor.IsWalkable) {
                    return true;
                }
            }
            return false;
        }

        private List<AStarNode> FloodFillNeighbors(AStarNode node, int fillValue) {
            AStarNode[] neighbors = Get4Neighbors(node).ToArray();

            List<AStarNode> confirmedNeighbors = new List<AStarNode>();
            foreach (AStarNode neighbor in neighbors) {
                if (neighbor.IsWalkable && neighbor.FloodFillValue == 0) {
                    neighbor.FloodFillValue = fillValue;
                    confirmedNeighbors.Add(neighbor);
                }
            }
            return confirmedNeighbors;
        }

        // ####################################################
        // some conditions:
        // ####################################################

        public bool NodeIsStraightToNeighbor(AStarNode node, AStarNode updatingNode)
            => node.Position.x == updatingNode.Position.x || node.Position.y == updatingNode.Position.y;

        private List<AStarNode> Get4Neighbors(AStarNode node) {
            Vector2Int[] array = {
                new Vector2Int(node.ArrayIndex.x - 1, node.ArrayIndex.y),
                new Vector2Int(node.ArrayIndex.x + 1, node.ArrayIndex.y),
                new Vector2Int(node.ArrayIndex.x, node.ArrayIndex.y - 1),
                new Vector2Int(node.ArrayIndex.x, node.ArrayIndex.y + 1),
            };

            List<AStarNode> nodes = new List<AStarNode>();
            foreach (var i in array) {
                if (NodeIsOutsideOfGrid(i, grid)) continue;
                else nodes.Add(grid.Grid[i.x, i.y]);
            }
            return nodes;
        }

        private List<AStarNode> Get8Neighbor(AStarNode node) {
            Vector2Int[] array = { 
                new Vector2Int(node.ArrayIndex.x - 1, node.ArrayIndex.y), 
                new Vector2Int(node.ArrayIndex.x + 1, node.ArrayIndex.y),
                new Vector2Int(node.ArrayIndex.x, node.ArrayIndex.y - 1), 
                new Vector2Int(node.ArrayIndex.x, node.ArrayIndex.y + 1),
                new Vector2Int(node.ArrayIndex.x - 1, node.ArrayIndex.y - 1), 
                new Vector2Int(node.ArrayIndex.x + 1, node.ArrayIndex.y - 1),
                new Vector2Int(node.ArrayIndex.x - 1, node.ArrayIndex.y + 1), 
                new Vector2Int(node.ArrayIndex.x + 1, node.ArrayIndex.y + 1), 
            };

            List<AStarNode> nodes = new List<AStarNode>();
            foreach (var i in array) {
                if (NodeIsOutsideOfGrid(i, grid)) continue;
                else nodes.Add(grid.Grid[i.x, i.y]);
            } 
            return nodes;
        }

        private bool StartOrTargetNodeIsNotValide() => !targetNode.IsWalkable || !startNode.IsWalkable || startNode == targetNode;
        private bool NewCostIsLower(float newCost, float oldCost) => newCost < oldCost;
        private bool OldCostIsUndefined(float oldCost) => oldCost <= 0;
        public bool NodeIsOutsideOfGrid(AStarNode node, AStarGrid grid) => NodeIsOutsideOfGrid(node.ArrayIndex, grid);
        public bool NodeIsOutsideOfGrid(Vector2Int arrayIndex, AStarGrid grid)
            => arrayIndex.x < 0 || arrayIndex.y < 0 || arrayIndex.x >= grid.Grid.GetLength(0) || arrayIndex.y >= grid.Grid.GetLength(1);
    }
}