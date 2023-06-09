using UnityEngine;
using UnityEditor;

namespace Magic
{
    [CustomEditor(typeof(RandomTimer))]
    public class RandomTimerEditor : Editor
    {
        RandomTimer timer;

        private void OnEnable() {
            timer = (RandomTimer)target;
        }

        public override void OnInspectorGUI() {
            GUI.enabled = false;
            GUILayout.BeginHorizontal();
                EditorGUILayout.FloatField("Min/Max start time", timer.MinSartingTime);
                EditorGUILayout.FloatField(timer.MaxStartingTime);
            GUILayout.EndHorizontal();
            EditorGUILayout.FloatField("Remaining time", timer.timeInSeconds);
            GUI.enabled = true;
        }
    }
}