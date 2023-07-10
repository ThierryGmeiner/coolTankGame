using System;
using UnityEngine;
using UnityEditor;
using Game.AI;
using Game.Entity.Tank;
using UnityEngine.UIElements;

namespace MEditor
{
    [CustomEditor(typeof(Tank))]
    public class TankEditor : Editor
    {
        Tank tank;

        private void OnEnable() {
            tank = (Tank)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            GUI.enabled = false;
            if (tank.Health != null) EditorGUILayout.TextField($"Health: {tank.Health.HitPoints} / {tank.Health.MaxHitPoints}");
            if (tank.Attack != null) EditorGUILayout.TextField($"Shots: {tank.Attack.RemainingShots} / {tank.Attack.MaxShotsUntilCooldown}");
            if (tank.Attack != null) EditorGUILayout.TextField($"IsReloading: {tank.Attack.IsReloading}");
            GUI.enabled = true;
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