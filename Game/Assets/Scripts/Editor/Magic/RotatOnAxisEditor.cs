using UnityEngine;
using UnityEditor;
using Magic;

namespace MEditor
{
    [CustomEditor(typeof(RotatOnAxis))]
    public class RotatOnAxisEditor : Editor
    {
        private RotatOnAxis rotate;

        private void OnEnable() {
            rotate = (RotatOnAxis)target;
        }

        private void OnSceneGUI() {

        }

        public override void OnInspectorGUI() {
            ShowButtons_MovableAxis();
            EditorGUILayout.Space(7);
            ShowButtons_Speed();
            EditorGUILayout.Space(7);
            ShowButtons_Shift();
            EditorGUILayout.Space(7);

            serializedObject.ApplyModifiedProperties();

        }

        private void ShowButtons_MovableAxis() {
            ShowButtons_MovableAxis_xAxis();
            ShowButtons_MovableAxis_yAxis();
            ShowButtons_MovableAxis_zAxis();
        }

        void ShowButtons_MovableAxis_xAxis() {
            EditorGUILayout.BeginHorizontal();

            SerializedProperty x = serializedObject.FindProperty(nameof(rotate.RotateX));
            EditorGUILayout.PropertyField(x);

            if (rotate.RotateX) {
                SerializedProperty xRotateClockwise = serializedObject.FindProperty(nameof(rotate.RotateClockwiseX));
                EditorGUILayout.PropertyField(xRotateClockwise);
            }

            EditorGUILayout.EndHorizontal();

            if (rotate.RotateX) {
                SerializedProperty xCurve = serializedObject.FindProperty(nameof(rotate.CurveX));
                EditorGUILayout.PropertyField(xCurve);
            }
        }

        void ShowButtons_MovableAxis_yAxis() {
            EditorGUILayout.BeginHorizontal();

            SerializedProperty y = serializedObject.FindProperty(nameof(rotate.RotateY));
            EditorGUILayout.PropertyField(y);

            if (rotate.RotateY) {
                SerializedProperty yRotateClockwise = serializedObject.FindProperty(nameof(rotate.RotateClockwiseY));
                EditorGUILayout.PropertyField(yRotateClockwise);
            }

            EditorGUILayout.EndHorizontal();

            if (rotate.RotateY) {
                SerializedProperty yCurve = serializedObject.FindProperty(nameof(rotate.CurveY));
                EditorGUILayout.PropertyField(yCurve);
            }
        }

        void ShowButtons_MovableAxis_zAxis() {
            EditorGUILayout.BeginHorizontal();

            SerializedProperty z = serializedObject.FindProperty(nameof(rotate.RotateZ));
            EditorGUILayout.PropertyField(z);

            if (rotate.RotateZ) {
                SerializedProperty zRotateClockwise = serializedObject.FindProperty(nameof(rotate.RotateClockwiseZ));
                EditorGUILayout.PropertyField(zRotateClockwise);
            }

            EditorGUILayout.EndHorizontal();
            
            if (rotate.RotateZ) {
                SerializedProperty zCurve = serializedObject.FindProperty(nameof(rotate.CurveZ));
                EditorGUILayout.PropertyField(zCurve);
            }
        }

        void ShowButtons_Speed() {
            SerializedProperty speed = serializedObject.FindProperty(nameof(rotate.speed));
            EditorGUILayout.PropertyField(speed);
        }

        void ShowButtons_Shift() {
            SerializedProperty shiftedStart = serializedObject.FindProperty(nameof(rotate.ShiftedStart));
            EditorGUILayout.PropertyField(shiftedStart);
            if (rotate.ShiftedStart != ShiftMode.non) {
                SerializedProperty shiftedAxis = serializedObject.FindProperty(nameof(rotate.ShiftedAxis));
                EditorGUILayout.PropertyField(shiftedAxis);
                SerializedProperty startShiftCurve = serializedObject.FindProperty(nameof(rotate.startShiftCurve));
                EditorGUILayout.PropertyField(startShiftCurve);
            }
        }
    }
}