using UnityEditor;
using UnityEngine;
using Magic;
using Game.Entity.Tank;
using Game.AI;

namespace MEditor
{
    [CustomEditor(typeof(TankAI))]
    public class TankAIEditor : Editor
    {
        private TankAI ai;
        private GameObject head;
        private float rayResolution = 0.5f;
        private LayerMask obstacleLayer;
        private LayerMask playerLayer;

        private void OnEnable() {
            ai = (TankAI)target;
            head = ai.GetComponent<Tank>().Head;
            obstacleLayer = LayerMask.GetMask("Obstacle");
            playerLayer = LayerMask.GetMask("Player");
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUILayout.Space(12);
            GUI.enabled = false;
            if (ai.Tank != null) EditorGUILayout.TextField($"See Player:  {ai.CanSeeTarget(ai.Tank.Head.transform)}");
            if (ai.Tank != null) EditorGUILayout.TextField($"Is Defensife:  {ai.IsDefensive}");

            if (ai.StateMachine == ai.StateStayAtStart) EditorGUILayout.TextField($"State:  {nameof(ai.StateStayAtStart)}");
            else if (ai.StateMachine == ai.StateFollowPath) EditorGUILayout.TextField($"State:  {nameof(ai.StateFollowPath)}");
            else if (ai.StateMachine == ai.StateSearch) EditorGUILayout.TextField($"State:  {nameof(ai.StateSearch)}");
            else if (ai.StateMachine == ai.StateAttack) EditorGUILayout.TextField($"State:  {nameof(ai.StateAttack)}");
            else if (ai.StateMachine == ai.StateTakeCover) EditorGUILayout.TextField($"State:  {nameof(ai.StateTakeCover)}");
            else EditorGUILayout.TextField($"State:  null");
            GUI.enabled = true;
        }

        private void OnSceneGUI() {
            // draw FOV-Radius
            Handles.color = Color.white;
            Handles.DrawWireArc(ai.transform.position, Vector3.up, Vector3.forward, 360, ai.ViewRadiusExtended);
            if (ai.StateMachine != ai.StateTakeCover && ai.StateMachine != ai.StateAttack) {
                Handles.DrawWireArc(ai.transform.position, Vector3.up, Vector3.forward, 360, ai.ViewRadius);
            }

            // draw FOV-Rays
            int amountOfRays = Mathf.RoundToInt(ai.ViewAngle * rayResolution);
            float currentAngle = -ai.ViewAngle / 2;
            float angle = ai.ViewAngle / amountOfRays;

            for (int i = 0; i < amountOfRays; i++) {
                DrawFOVRay(currentAngle);
                currentAngle += angle;
            }

            // draw attackpossition range
            if (ai.StateMachine == ai.StateTakeCover || ai.StateMachine == ai.StateAttack) {
                Handles.color = Color.red;
                Handles.DrawWireArc(ai.transform.position, Vector3.up, Vector3.forward, 360, ai.PreferTargetDistanceMin);
                Handles.DrawWireArc(ai.transform.position, Vector3.up, Vector3.forward, 360, ai.PreferTargetDistanceMax);
            }
        }

        private void DrawFOVRay(float angle) {
            RaycastHit rayInfo;
            Ray ray = new Ray(ai.transform.position, ai.ViewDirection(head.transform, angle, false));
            Physics.Raycast(ray, out rayInfo, ai.ViewRadiusExtended, obstacleLayer);
            float length = rayInfo.collider == null ? ai.ViewRadiusExtended : rayInfo.distance;

            Physics.Raycast(ray, out rayInfo, ai.ViewRadiusExtended);
            if (rayInfo.collider?.tag == Tags.Player) {
                length = rayInfo.distance;
                Handles.color = Color.green;
            }
            else Handles.color = Color.white;
            
            Handles.DrawLine(ai.transform.position, ai.transform.position + ai.ViewDirection(head.transform, angle, false) * length);
        }
    }
}