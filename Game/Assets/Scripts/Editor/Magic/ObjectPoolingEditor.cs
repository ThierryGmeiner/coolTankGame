using UnityEngine;
using UnityEditor;
using Magic;

namespace MEditor
{
    [CustomEditor(typeof(ObjectPooling))]
    public class ObjectPoolingEditor : Editor
    {
        ObjectPooling pooler;

        private void OnEnable() {
            pooler = (ObjectPooling)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);

            GUI.enabled = false;
            EditorGUILayout.FloatField("Poolable objects", (pooler.InactiveObjects.Count + pooler.ActiveObjects.Count));
            EditorGUILayout.FloatField("Inactive objects", pooler.InactiveObjects.Count);
            EditorGUILayout.FloatField("Active objects", pooler.ActiveObjects.Count);
            GUI.enabled = true;
        }
    }
}