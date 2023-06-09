using UnityEditor;
using UnityEngine;

namespace Game.AI
{
    [CustomEditor(typeof(TankAI))]
    public class EnemyAIEditor : Editor
    {
        TankAI ai;

        private void OnEnable() {
            ai = (TankAI)target;
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