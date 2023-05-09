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

        public AStarNode[,] Grid { get; private set; }

        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (Grid != null) {
                foreach (AStarNode node in Grid) {
                    Gizmos.color = node.IsWalkable ? Color.white : Color.red;
                    Gizmos.DrawCube(node.Position, Vector3.one * (nodeDiameter - 0.3f));
                }
            }
        }

        private void Start() {
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

        public void Clear() {
            foreach (AStarNode node in Grid) {
                node.Clear();
            }
        }

        private void CreateGrid() {
            Grid = new AStarNode[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft 
                = new Vector3(transform.position.x - gridWorldSize.x / 2, transform.position.y, transform.position.z - gridWorldSize.y / 2);

            for (int x = 0; x < gridSizeX; x++) {
                for (int y = 0; y < gridSizeX; y++) {
                    Vector3 worldPoint
                        = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                    Grid[x, y] = new AStarNode(walkable, worldPoint);
                }
            }
        }
    }
}