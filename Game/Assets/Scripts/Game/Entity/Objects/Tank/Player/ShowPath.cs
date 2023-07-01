using System.Collections;
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
        [SerializeField] private float appearanceTimeInSec = 0.05f;

        TankMovement movement;
        private const float SPAWN_HEIGHT = 0.1f;

        private void Awake() {
            distance = Mathf.Clamp(distance, 0.1f, float.MaxValue);
        }

        private void Start() {
            movement = GetComponent<Tank>().Movement;
            movement.OnSetPath += (Path path) => {
                // stop the old functioncal to prevent taht the previous line get drawn further
                StopCoroutine(nameof(SetParticles));
                StartCoroutine(SetParticles(path));
            };
        }

        private IEnumerator SetParticles(Path path) {
            if (path.Nodes.Length != 0) {
                Vector3[] fullPath = GetPointsInPath(path);
                StartCoroutine(objectPooler.DeaktivateOverTime(objectPooler.ActiveObjects, appearanceTimeInSec));

                foreach (Vector3 pos in fullPath) {
                    GameObject particle = objectPooler.RequestActivatedObject();
                    particle.transform.position = new Vector3(pos.x, SPAWN_HEIGHT, pos.z);

                    yield return new WaitForSeconds(appearanceTimeInSec);
                }
            }
        }

        private Vector3[] GetPointsInPath(Path path) {
            List<Vector3> positions = new List<Vector3>();

            positions = AddToList(positions, MathM.PositionsInDevidedLine(transform.position, path.Nodes[movement.pathIndex].Position, distance));
            
            for (int i = movement.pathIndex; i < path.Nodes.Length; i++) {
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