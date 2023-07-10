using UnityEngine;
using UnityEditor;
using Magic;

namespace MEditor
{
    [CustomEditor(typeof(PlannedTimer))]
    public class PlannedTimerEditor : Editor
    {
        PlannedTimer timer;

        private void OnEnable() {
            timer = (PlannedTimer)target;
        }

        public override void OnInspectorGUI() {
            GUI.enabled = false;
            EditorGUILayout.TextField(timer.Name);
            GUI.enabled = true;

            GUILayout.Space(8);
            
            GUILayout.BeginHorizontal();
                if (GUILayout.Button("Stop")) timer.StopTimer();
                if (GUILayout.Button("Start")) timer.StartTimer();
                if (GUILayout.Button("End")) timer.ReduceTime(float.MaxValue);
            GUILayout.EndHorizontal();

            GUILayout.Space(12);

            GUI.enabled = false;
            EditorGUILayout.FloatField("Start time", timer.StartTimeSec);
            EditorGUILayout.FloatField("Remaining time", timer.timeSec);
            GUI.enabled = true;
        }
    }
}