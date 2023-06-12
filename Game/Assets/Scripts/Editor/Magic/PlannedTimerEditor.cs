using UnityEngine;
using UnityEditor;

namespace Magic
{
    [CustomEditor(typeof(PlannedTimer))]
    public class PlannedTimerEditor : Editor
    {
        PlannedTimer timer;

        private void OnEnable() {
            timer = (PlannedTimer)target;
        }

        public override void OnInspectorGUI() {
            GUILayout.BeginHorizontal();
                if (GUILayout.Button("Stop")) timer.StopTimer();
                if (GUILayout.Button("Start")) timer.StartTimer();
                if (GUILayout.Button("End")) timer.ReduceTime(float.MaxValue);
            GUILayout.EndHorizontal();

            GUILayout.Space(12);

            GUI.enabled = false;
            EditorGUILayout.FloatField("Start time", timer.StartTimeInSeconds);
            EditorGUILayout.FloatField("Remaining time", timer.timeInSeconds);
            GUI.enabled = true;
        }
    }
}