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

        public AStarNode[] GetOptimizedPath(Vector2 startPos, Vector2 targetPos) {
            startNode = grid.GetNodeFromPosition(startPos);
            targetNode = grid.GetNodeFromPosition(targetPos);

            // get simple path
            // simplify the way (via rays)
            // convert the simplified way to an array

            AStarNode[] path = GetAStarPath();


            grid.Clear();
            return new AStarNode[0];
        }

        public AStarNode[] GetAStarPath(Vector3 startPos, Vector3 targetPos) {
            startNode = grid.GetNodeFromPosition(startPos);
            targetNode = grid.GetNodeFromPosition(targetPos);
            return GetAStarPath();
        }

        public AStarNode[] GetAStarPath() {
            AStarNode currentNode = startNode;

            int iteration = 0;

            while (currentNode != targetNode && iteration < 10000) {
                UpdateNeighbors(currentNode);
                currentNode = grid.GetCheapestNode();
                iteration++;
            }
            return AStarHelper.GetPathViaBacktracking(startNode, targetNode).ToArray();
        }

        public void UpdateNeighbors(AStarNode currentNode) {
            Vector2Int[] neighborPositions = SuroundingNodes(currentNode.ArrayIndex);
            currentNode.AllNeighborsAreDiscovered = true;

            foreach (Vector2Int position in neighborPositions) {
                if (!currentNode.IsWalkable || AStarHelper.NodeIsOutsideOfGrid(position, grid)) continue;
                AStarHelper.UpdateNode(grid.Grid[position.x, position.y], currentNode, targetNode);
            }
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