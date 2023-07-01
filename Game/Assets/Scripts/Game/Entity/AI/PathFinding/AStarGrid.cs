using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class AStarGrid : MonoBehaviour
    {
        [SerializeField] public LayerMask unwalkableMask = 7;
        [SerializeField] private Vector2 gridWorldSize = new Vector2(10, 10);
        [SerializeField] private float nodeRadius = 0.5f;
        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        [Header("PathFinding")]
        [SerializeField] private bool drawBoard = false;

        public AStarNode[,] Grid { get; set; } = new AStarNode[0, 0];
        public float NodeRadius { get => nodeRadius; }

        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
            if (drawBoard) {
                foreach (AStarNode node in Grid) {
                    Gizmos.color = node.IsWalkable ? Color.white : Color.red;
                    Gizmos.DrawCube(new Vector3(node.Position.x, 0, node.Position.z), Vector3.one * (nodeDiameter - 0.15f));
                }
            }
        }

        private void Awake() {
            gameObject.name = "A*";
            if (transform.position.x != 0 || transform.position.z != 0) {
                Debug.LogError($"x and z possition of {nameof(AStarGrid)} has to bee zero");
            }
                
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        public AStarNode GetNodeFromPosition(Vector3 position) {
            float precentX = (position.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float precentY = (position.z + gridWorldSize.y / 2) / gridWorldSize.y;
            precentX = Mathf.Clamp01(precentX);
            precentY = Mathf.Clamp01(precentY);
            int x = Mathf.RoundToInt((gridSizeX - 1) * precentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * precentY);
            return Grid[x, y];
        }

        public AStarNode GetCheapestNode() {
            List<AStarNode> listOfCheapestNodes = new List<AStarNode>();
            AStarNode startNode = new AStarNode(true, Vector2.zero);
            startNode.hCost = int.MaxValue;
            listOfCheapestNodes.Add(startNode);

            foreach (AStarNode node in Grid) {
                if (node.AllNeighborsAreDiscovered || node.fCost == 0 || !node.IsWalkable) continue;
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

        public AStarNode GetClosestWalkableNeighbor(AStarNode node, Vector3 clickPosition) {
            List<AStarNode> neighbors = GetWalkableNodes(Get8Neighbor(node));

            if (neighbors.Count == 0) return node;

            AStarNode closestNeighbor = neighbors[0];
            float distance = Vector3.Distance(closestNeighbor.Position, clickPosition);

            for (int i = 1; i < neighbors.Count; i++) {
                float currentDistance = Vector3.Distance(neighbors[i].Position, clickPosition);
                if (currentDistance > distance) {
                    distance = currentDistance;
                    closestNeighbor = neighbors[i];
                }
            }
            return closestNeighbor;
        }

        public List<AStarNode> GetWalkableNodes(List<AStarNode> nodes) {
            List<AStarNode> walkableNodes = new List<AStarNode>();
            for (int i = 0; i < nodes.Count; i++) {
                if (nodes[i].IsWalkable) {
                    walkableNodes.Add(nodes[i]);
                }
            } return walkableNodes;
        }

        public AStarNode GetCoveredNode(Vector3 startPos, GameObject enemy, int fillValue)
            => GetCoveredField(GetNodeFromPosition(startPos), enemy, fillValue);

        public AStarNode GetCoveredField(AStarNode startPos, GameObject enemy, int fillValue) {
            List<AStarNode> currentNodes = new List<AStarNode>() { startPos };
            List<AStarNode> nextNodes;

            // loop through iterations
            while (fillValue > 0) {
                nextNodes = new List<AStarNode>();
                fillValue--;

                // loop throu activeNodes and search siutable node
                foreach (AStarNode node in currentNodes) {
                    if (Physics.Linecast(node.Position, enemy.transform.position, unwalkableMask)) {
                        Clear();
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
            Clear();
            return startPos;
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

        public void Clear() {
            foreach (AStarNode node in Grid) node.Clear();
        }

        private void CreateGrid() {
            Grid = new AStarNode[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft 
                = new Vector3(transform.position.x - gridWorldSize.x / 2, 0, transform.position.z - gridWorldSize.y / 2);
            for (int x = 0; x < gridSizeX; x++) {
                for (int y = 0; y < gridSizeY; y++) {
                    Vector3 worldPoint
                        = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(new Vector3(worldPoint.x, 0, worldPoint.z), nodeRadius, unwalkableMask));
                    Grid[x, y] = new AStarNode(walkable, new Vector3(worldPoint.x, 0, worldPoint.z), new Vector2Int(x, y));
                }
            }
        }

        public List<AStarNode> Get4Neighbors(AStarNode node) {
            Vector2Int[] array = {
                new Vector2Int(node.ArrayIndex.x - 1, node.ArrayIndex.y),
                new Vector2Int(node.ArrayIndex.x + 1, node.ArrayIndex.y),
                new Vector2Int(node.ArrayIndex.x, node.ArrayIndex.y - 1),
                new Vector2Int(node.ArrayIndex.x, node.ArrayIndex.y + 1),
            };

            List<AStarNode> nodes = new List<AStarNode>();
            foreach (var i in array) {
                if (NodeIsOutsideOfGrid(i)) continue;
                else nodes.Add(Grid[i.x, i.y]);
            }
            return nodes;
        }

        public List<AStarNode> Get8Neighbor(AStarNode node) {
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
                if (NodeIsOutsideOfGrid(i)) continue;
                else nodes.Add(Grid[i.x, i.y]);
            }
            return nodes;
        }

        public bool NodeIsOutsideOfGrid(AStarNode node) => NodeIsOutsideOfGrid(node.ArrayIndex);
        public bool NodeIsOutsideOfGrid(Vector2Int arrayIndex)
            => arrayIndex.x < 0 || arrayIndex.y < 0 || arrayIndex.x >= Grid.GetLength(0) || arrayIndex.y >= Grid.GetLength(1);
    }
}