using System;
using UnityEngine;
using Game.Data;

namespace Game.AI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        [SerializeField] protected DataEnemyAI data;

        // movement
        protected Path wayPointPaths = new Path(new AStarNode[0]);
        protected int currentPathIndex = 0;

        // other
        protected GameObject target;
        protected LayerMask targetLayer;
        protected LayerMask obstacleLayer;
        protected Vector3 startPos;
        protected AStarNode startPosNode;
        protected SceneData sceneData;

        public Action StateMachine { get; protected set; }
        public DataEnemyAI Data { set => data = value; }
        public AStarNode lastVisualContact { get; set; }

        protected virtual void Awake() {
            startPos = transform.position;
            obstacleLayer = LayerMask.GetMask("Obstacle");
            targetLayer = LayerMask.GetMask("Player");
            sceneData = GameObject.Find("SceneData").GetComponent<SceneData>();
        }

        protected virtual void Start() {
            startPosNode = sceneData.AStarGrid?.GetNodeFromPosition(startPos);
            target = sceneData.Player ?? new GameObject();
        }

        public float ViewAngle { get => data.viewAngle; }
        public float ViewRadius { get => data.viewRadius; }
        public float ViewRadiusExtended { get => data.viewRadiusExtended; }
        public float PreferTargetDistanceMin { get => data.preferTargetDistanceMin; }
        public float PreferTargetDistanceMax { get => data.preferTargetDistanceMax; }

        protected abstract void RotateTowardsDamageSource(int maxHP, int hp, int damage, Vector3 direction);

        public virtual bool TargetIsInScope(Transform head, float scopeRadius) {
            Ray ray = new Ray(head.position, Magic.MathM.ConvertToVector3(head.rotation.eulerAngles.y));

            // do this for optimization: firtst check if target is in scope, then if no obstacle is in betwean
            if (Physics.SphereCast(ray, scopeRadius, data.viewRadiusExtended, targetLayer)) {
                return !Physics.SphereCast(ray, scopeRadius, data.viewRadiusExtended, obstacleLayer);
            } return false;
        }

        public virtual bool CanSeeTarget(Transform head) {
            if (!TargetIsInViewField(head)) {
                return false;
            } return !Physics.Linecast(transform.position, target.transform.position, obstacleLayer);
        }

        protected virtual bool TargetIsInViewField(Transform head) {
            var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget > data.viewRadiusExtended) { return false; }
            if (distanceToTarget < data.viewRadius) { return true; }

            Vector3 directionToTarget = (target.transform.position - head.position).normalized;
            bool targetInAngle = Vector3.Angle(head.forward, directionToTarget) < data.viewAngle / 2;
            return targetInAngle;
        }

        public virtual Vector3 ViewDirection(Transform head, float angleInDegrees, bool angleIsGlobal) {
            if (!angleIsGlobal) {
                angleInDegrees += head.eulerAngles.y;
            } return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}