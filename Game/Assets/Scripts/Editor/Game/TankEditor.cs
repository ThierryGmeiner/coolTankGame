using UnityEngine;
using UnityEditor;
using Game.AI;
using Game.Entity.Tank;

namespace MEditor
{
    [CustomEditor(typeof(Tank))]
    public class TankEditor : Editor
    {
        Tank tank;

        private void OnEnable() {
            tank = (Tank)target;
        }

        private void OnSceneGUI() {
            if (tank.Movement != null && tank.Movement.Path != null) {
                Path path = tank.Movement.Path;
                Handles.color = Color.white;
                for (int i = 0; i < path.Nodes.Length; i++) {
                    if (i + 1 < path.Nodes.Length) {
                        Handles.DrawLine(path.Nodes[i].Position, path.Nodes[i + 1].Position);
                    }
                }
            }
        }
    }
}