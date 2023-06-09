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
            GUI.enabled = false;
            EditorGUILayout.FloatField("Start time", timer.StartTimeInSeconds);
            EditorGUILayout.FloatField("Remaining time", timer.timeInSeconds);
            GUI.enabled = true;
        }
    }
}