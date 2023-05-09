using UnityEngine;

namespace Game.AI
{
    public class AStar
    {
        private readonly AStarGrid grid;
        private AStarNode startNode;
        private AStarNode tartgetNode;

        public AStar(AStarGrid grid) {
            this.grid = grid;
        }

        public AStarNode[] GetPath(Vector3 startPos, Vector3 targetPos) {
            AStarNode[] path;
            startNode = grid.GetNodeFromPosition(startPos);
            tartgetNode = grid.GetNodeFromPosition(targetPos);

            // get simple path
            // simplify the way (via rays)
            // convert the simplified way to an array

            grid.Clear();
            return new AStarNode[0];
        }

        private AStarNode[] GetAStarPath() {
            AStarNode currentNode = startNode;
            
            while (currentNode != tartgetNode) {
                UpdateNeighbors(currentNode);
            }

            // get the path
            //    => let all nodes point back to the previous node
        }

        private void UpdateNeighbors(AStarNode currentNode) {
            UpdateNode(grid.Grid[(int)currentNode.Position.x - 1, (int)currentNode.Position.y], currentNode);
            UpdateNode(grid.Grid[(int)currentNode.Position.x + 1, (int)currentNode.Position.y], currentNode);
            UpdateNode(grid.Grid[(int)currentNode.Position.x, (int)currentNode.Position.y - 1], currentNode);
            UpdateNode(grid.Grid[(int)currentNode.Position.x, (int)currentNode.Position.y + 1], currentNode);
            UpdateNode(grid.Grid[(int)currentNode.Position.x - 1, (int)currentNode.Position.y - 1], currentNode);
            UpdateNode(grid.Grid[(int)currentNode.Position.x + 1, (int)currentNode.Position.y - 1], currentNode);
            UpdateNode(grid.Grid[(int)currentNode.Position.x - 1, (int)currentNode.Position.y + 1], currentNode);
            UpdateNode(grid.Grid[(int)currentNode.Position.x + 1, (int)currentNode.Position.y + 1], currentNode);
        }

        private void UpdateNode(AStarNode node, AStarNode updatingNeighbor) {
            if (NodeIsInsideOfGrid(node))
                node.UpdateCost(GCost(node, updatingNeighbor), HCost(node));
        }

        public int GCost(AStarNode node, AStarNode updatingNeighbor) {
            // if updating neighbor is diagonal the cost has to increas by 14 else by 10

            // if new c cost is lower 
            //    update the LastNodeInPath to the updating neighbor
            //    return new c cost
            // else
            //    return old value
        }

        public int HCost(AStarNode node) {

        }

        private bool NodeIsInsideOfGrid(AStarNode node)
            => node.Position.x >= 0
            && node.Position.y >= 0
            && node.Position.x < grid.Grid.Length
            && node.Position.y < grid.Grid.Length;
    }
}