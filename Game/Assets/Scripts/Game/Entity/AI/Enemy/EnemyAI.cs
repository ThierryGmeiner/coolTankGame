using System;
using UnityEngine;

namespace Game.AI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] protected float viewRadius = 6;
        [SerializeField] protected float viewRadiusExtended = 18;
        [SerializeField] [Range(0, 360)] protected float viewAngle = 90;
        protected AStarNode lastVisualContact;

        [Header("Movement")]
        [SerializeField] protected Transform[] wayPoints = new Transform[0];
        [SerializeField] protected float preferTargetDistanceMin = 5;
        [SerializeField] protected float preferTargetDistanceMax = 14;
        protected AStarGrid aStarGrid;
        protected Path wayPointPaths = new Path(new AStarNode[0], true);
        protected int currentPathIndex = 0;

        public Action StateMachine { get; protected set; }
        protected GameObject target;
        protected LayerMask targetLayer;
        protected LayerMask obstacleLayer;
        protected Vector3 startPos;
        protected AStarNode startPosNode;


        protected virtual void Awake() {
            startPos = transform.position;
            obstacleLayer = LayerMask.GetMask("Obstacle");
            targetLayer = LayerMask.GetMask("Player");
        }

        protected virtual void Start() {
            aStarGrid = GameObject.Find("A*")?.GetComponent<AStarGrid>();
            startPosNode = aStarGrid?.GetNodeFromPosition(startPos);
            target = GameObject.FindGameObjectWithTag(Magic.Tags.Player).transform.root.gameObject;
        }

        public float ViewAngle { get => viewAngle; }
        public float ViewRadius { get => viewRadius; }
        public float ViewRadiusExtended { get => viewRadiusExtended; }
        public float PreferTargetDistanceMin { get => preferTargetDistanceMin; }
        public float PreferTargetDistanceMax { get => preferTargetDistanceMax; }

        public virtual bool TargetIsInScope(Transform head, float scopeRadius) {
            Ray ray = new Ray(head.position, Magic.MathM.ConvertToVector3(head.rotation.eulerAngles.y));

            // do this for optimization: firtst check if target is in scope, then if no obstacle is in betwean
            if (Physics.SphereCast(ray, scopeRadius, viewRadiusExtended, targetLayer)) {
                return !Physics.SphereCast(ray, scopeRadius, viewRadiusExtended, obstacleLayer);
            } return false;
        }

        public virtual bool CanSeeTarget(Transform head) {
            if (!TargetIsInViewFieldd(head)) {
                return false;
            } return !Physics.Linecast(transform.position, target.transform.position, obstacleLayer);
        }

        public virtual bool TargetIsInViewFieldd(Transform head) {
            // target is in inner FOV
            if (Vector3.Distance(transform.position, target.transform.position) < viewRadius) return true;

            // target is in extendet FOV
            Vector3 directionToTarget = (target.transform.position - head.position).normalized;
            bool targetInAngle = Vector3.Angle(head.forward, directionToTarget) < viewAngle / 2;
            bool targetInExtendedSight = Vector3.Distance(transform.position, target.transform.position) < viewRadiusExtended;
            return targetInExtendedSight && targetInAngle;
        }

        public virtual Vector3 ViewDirection(float angleInDegrees, bool angleIsGlobal) {
            if (!angleIsGlobal) {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}