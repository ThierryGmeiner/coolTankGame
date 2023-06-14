using UnityEditor;
using UnityEngine;

namespace Game.AI
{
    [CustomEditor(typeof(TankAI))]
    public class TankAIEditor
        : Editor
    {
        TankAI ai;

        private void OnEnable() {
            ai = (TankAI)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUILayout.Space(12);
            GUI.enabled = false;
                if (ai.StateMachine == ai.StateStayAtStart) EditorGUILayout.TextField($"State:  {nameof(ai.StateStayAtStart)}");
                else if (ai.StateMachine == ai.StateFollowPath) EditorGUILayout.TextField($"State:  {nameof(ai.StateFollowPath)}");
                else if (ai.StateMachine == ai.StateSearch) EditorGUILayout.TextField($"State:  {nameof(ai.StateSearch)}");
                else if (ai.StateMachine == ai.StateAttack) EditorGUILayout.TextField($"State:  {nameof(ai.StateAttack)}");
                else EditorGUILayout.TextField($"State:  null");
            GUI.enabled = true;
        }

        private void OnSceneGUI() {
            Handles.color = Color.white;
            Handles.DrawWireArc(ai.transform.position, Vector3.up, Vector3.forward, 360, ai.ViewRadius);
            Handles.DrawWireArc(ai.transform.position, Vector3.up, Vector3.forward, 360, ai.ViewRadiusExtended);

            Vector3 viewAngleA = ai.ViewDirection(-ai.ViewAngle / 2, false);
            Vector3 viewAngleB = ai.ViewDirection(ai.ViewAngle / 2, false);

            Handles.DrawLine(ai.transform.position, ai.transform.position + viewAngleA * ai.ViewRadiusExtended);
            Handles.DrawLine(ai.transform.position, ai.transform.position + viewAngleB * ai.ViewRadiusExtended);
        }
    }
}