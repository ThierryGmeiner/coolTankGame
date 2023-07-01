using System.Collections.Generic;
using UnityEngine;
using Game.AI;
using Magic;

namespace Game.Entity.Tank
{
    [RequireComponent(typeof(Tank))]
    public class ShowPath : MonoBehaviour
    {
        [SerializeField] private ObjectPooling objectPooler;
        [SerializeField] private Color color;
        [SerializeField] private float distance;

        TankMovement movement;
        private const float SPAWN_HEIGHT = 0.1f;

        private void Awake() {
            distance = Mathf.Clamp(distance, 0.1f, float.MaxValue);
        }

        private void Start() {
            movement = GetComponent<Tank>().Movement;
            movement.OnSetPath += SetParticles;
        }

        private void SetParticles(Path path) {
            if (path.Nodes.Length == 0) return;

            Vector3[] fullPath = GetPointsInPath(path);

            foreach (Vector3 pos in fullPath) {
                GameObject particle = objectPooler.RequestActivatedObject();
                particle.transform.position = new Vector3(pos.x, SPAWN_HEIGHT, pos.z);
            }
        }

        private Vector3[] GetPointsInPath(Path path) {
            List<Vector3> positions = new List<Vector3>();

            positions = AddToList(positions, MathM.PositionsInDevidedLine(transform.position, path.Nodes[0].Position, distance));
            
            for (int i = 0; i < path.Nodes.Length; i++) {
                if (path.Nodes[i] == path.Target) break;
                positions = AddToList(positions, MathM.PositionsInDevidedLine(path.Nodes[i].Position, path.Nodes[i + 1].Position, distance));
            }
            return positions.ToArray();
        }

        private List<Vector3> AddToList(List<Vector3> list ,Vector3[] vectors) {
            foreach (Vector3 vec in vectors) {
                list.Add(vec);
            } return list;
        }
    }
}