using UnityEngine;
using UnityEditor;
using Magic;

namespace MEditor
{
    [CustomEditor(typeof(MovementOnAxis))]
    public class MovementOnAxisEditor : Editor
    {
        private MovementOnAxis move;

        private void OnEnable() {
            move = (MovementOnAxis)target;
            move.DoReset();
        }

        private void OnSceneGUI() {
            move.CalculateTargets();
            float size = 0.2f;

            if (move.MoveX) {
                Handles.color = Color.red;
                foreach (var pos in move.targetX) {
                    Handles.SphereHandleCap(0, pos, Quaternion.identity, size, EventType.Repaint);
                }
            }
            if (move.MoveY) {
                Handles.color = Color.green;
                foreach (var pos in move.targetY) {
                    Handles.SphereHandleCap(0, pos, Quaternion.identity, size, EventType.Repaint);
                }
            }
            if (move.MoveZ) {
                Handles.color = Color.blue;
                foreach (var pos in move.targetZ) {
                    Handles.SphereHandleCap(0, pos, Quaternion.identity, size, EventType.Repaint);
                }
            }
        }

        public override void OnInspectorGUI() {
                
            ShowButtons_MovableAxis();
            EditorGUILayout.Space(7);
            ShowButtons_Speed();
            EditorGUILayout.Space(7);
            ShowButtons_Shift();
            EditorGUILayout.Space(7);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Reset")) {
                move.DoReset();
            }
            if (GUILayout.Button("Set on ground")) {
                RaycastHit hit;
                Physics.Raycast(new Ray(move.transform.position, Vector3.down), out hit);
                // hit.point is equal to Vector.zero when the ray dosnt hit a point
                if (hit.point != Vector3.zero) {
                    move.transform.position = new Vector3(hit.point.x, hit.point.y + move.transform.localScale.y, hit.point.z);
                    move.DoReset();
                }
            }
        }

        private void ShowButtons_MovableAxis() {
            ShowButtons_MovableAxis_xAxis();
            ShowButtons_MovableAxis_yAxis();
            ShowButtons_MovableAxis_zAxis();
        }

        void ShowButtons_MovableAxis_xAxis() {
            EditorGUILayout.BeginHorizontal();
            
            SerializedProperty x = serializedObject.FindProperty(nameof(move.MoveX));
            EditorGUILayout.PropertyField(x);
            if (move.MoveX) {
                SerializedProperty xDistance = serializedObject.FindProperty(nameof(move.DistanceX));
                EditorGUILayout.PropertyField(xDistance);
            }
            
            EditorGUILayout.EndHorizontal();
            
            if (move.MoveX) {
                SerializedProperty xCurve = serializedObject.FindProperty(nameof(move.CurveX));
                EditorGUILayout.PropertyField(xCurve);
            }
        }

        void ShowButtons_MovableAxis_yAxis() {
            EditorGUILayout.BeginHorizontal();
            
            SerializedProperty y = serializedObject.FindProperty(nameof(move.MoveY));
            EditorGUILayout.PropertyField(y);
            if (move.MoveY) {
                SerializedProperty yDistance = serializedObject.FindProperty(nameof(move.DistanceY));
                EditorGUILayout.PropertyField(yDistance);
            }
            
            EditorGUILayout.EndHorizontal();
            
            if (move.MoveY) {
                SerializedProperty yCurve = serializedObject.FindProperty(nameof(move.CurveY));
                EditorGUILayout.PropertyField(yCurve);
            }
        }

        void ShowButtons_MovableAxis_zAxis() {
            EditorGUILayout.BeginHorizontal();

            SerializedProperty z = serializedObject.FindProperty(nameof(move.MoveZ));
            EditorGUILayout.PropertyField(z);
            if (move.MoveZ) {
                SerializedProperty zDistance = serializedObject.FindProperty(nameof(move.DistanceZ));
                EditorGUILayout.PropertyField(zDistance);
            }
            
            EditorGUILayout.EndHorizontal();
            
            if (move.MoveZ) {
                SerializedProperty zCurve = serializedObject.FindProperty(nameof(move.CurveZ));
                EditorGUILayout.PropertyField(zCurve);
            }
        }

        void ShowButtons_Speed() {
            SerializedProperty speed = serializedObject.FindProperty(nameof(move.speed));
            EditorGUILayout.PropertyField(speed);
        }

        void ShowButtons_Shift() {
            SerializedProperty shiftedStart = serializedObject.FindProperty(nameof(move.ShiftedStart));
            EditorGUILayout.PropertyField(shiftedStart);
            if (move.ShiftedStart != ShiftMode.non) {
                SerializedProperty shiftedAxis = serializedObject.FindProperty(nameof(move.ShiftedAxis));
                EditorGUILayout.PropertyField(shiftedAxis);
                SerializedProperty startShiftCurve = serializedObject.FindProperty(nameof(move.startShiftCurve));
                EditorGUILayout.PropertyField(startShiftCurve);
            }
        }
    }
}