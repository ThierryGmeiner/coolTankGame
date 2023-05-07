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

        public void Start() {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
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