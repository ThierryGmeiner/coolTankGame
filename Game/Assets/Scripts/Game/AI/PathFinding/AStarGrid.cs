using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class AStarGrid : MonoBehaviour
    {
        [SerializeField] public LayerMask unwalkableMask = 7;
        [SerializeField] private Vector2 gridWorldSize = new Vector2(20, 20);
        [SerializeField] private float nodeRadius = 0.5f;
        [SerializeField] private Transform player;
        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        public AStarNode[,] Grid { get; set; }

        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (Grid != null) {
                foreach (AStarNode node in Grid) {
                    Gizmos.color = node.IsWalkable ? Color.white : Color.red;
                    Gizmos.DrawCube(new Vector3(node.Position.x, 0, node.Position.y), Vector3.one * (nodeDiameter - 0.3f));
                }
            }
        }

        private void Start() {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        public AStarNode GetNodeFromPosition(Vector2 position) {
            float precentX = (position.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float precentY = (position.y + gridWorldSize.y / 2) / gridWorldSize.y;
            precentX = Mathf.Clamp01(precentX);
            precentY = Mathf.Clamp01(precentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * precentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * precentY);
            return Grid[x, y];
        }

        public Vector2Int GetIndexFromNode(AStarNode node) {
            for (int x = 0; x < Grid.GetLength(0); x++) {
                for (int y = 0; y < Grid.GetLength(1); y++) {
                    if (node.Equals(Grid[x, y])) {
                        return new Vector2Int(x, y);
                    }
                }
            }
            return new Vector2Int(-1, -1);
        }

        public AStarNode GetCheapestNode() {
            List<AStarNode> listOfCheapestNodes = new List<AStarNode>();
            AStarNode startNode = new AStarNode(true, Vector2.zero);
            startNode.hCost = int.MaxValue;
            listOfCheapestNodes.Add(startNode);

            foreach (AStarNode node in Grid) {
                if (node.AllNeighborsAreDiscovered || node.fCost == 0) continue;
                if (node.fCost < listOfCheapestNodes[0].fCost) { listOfCheapestNodes.Clear(); listOfCheapestNodes.Add(node); }
                else if (node.fCost == listOfCheapestNodes[0].fCost) listOfCheapestNodes.Add(node);
            }
            return GetCheapestNode(listOfCheapestNodes);
        }

        private AStarNode GetCheapestNode(List<AStarNode> nodeList) {
            if (nodeList.Count == 1) return nodeList[0];
            
            // if there are more then one node, tanke the one with the lowest hCost
            AStarNode cheapestNode = nodeList[0];
            for (int i = 1; i < nodeList.Count; i++) {
                if (nodeList[i].hCost < cheapestNode.hCost) {
                    cheapestNode = nodeList[i];
                }
            }
            return cheapestNode;
        }

        public void Clear() {
            foreach (AStarNode node in Grid) node.Clear();
        }

        private void CreateGrid() {
            Grid = new AStarNode[gridSizeX, gridSizeY];
            Vector2 worldBottomLeft 
                = new Vector2(transform.position.x - gridWorldSize.x / 2, transform.position.z - gridWorldSize.y / 2);

            for (int x = 0; x < gridSizeX; x++) {
                for (int y = 0; y < gridSizeX; y++) {
                    Vector2 worldPoint
                        = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(new Vector3(worldPoint.x, 0, worldPoint.y), nodeRadius, unwalkableMask));
                    Grid[x, y] = new AStarNode(walkable, worldPoint, new Vector2Int(x, y));
                }
            }
        }
    }
}