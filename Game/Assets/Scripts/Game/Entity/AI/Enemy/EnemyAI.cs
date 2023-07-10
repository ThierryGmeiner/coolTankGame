using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity.Interactable;
using Game.Data;

namespace Game.AI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        [SerializeField] protected DataEnemyAI data;

        // movement
        protected AStarGrid aStarGrid;
        protected Path wayPointPaths = new Path(new AStarNode[0]);
        protected int currentPathIndex = 0;

        // other
        protected GameObject target;
        protected LayerMask targetLayer;
        protected LayerMask obstacleLayer;
        protected Vector3 startPos;
        protected AStarNode startPosNode;

        // enviorment
        protected GameObject repairBoxesContainer;
        protected EnemyAI[] otherEnemys;

        public Action StateMachine { get; protected set; }
        public DataEnemyAI Data { set => data = value; }
        public AStarNode lastVisualContact { get; set; }

        protected virtual void Awake() {
            startPos = transform.position;
            obstacleLayer = LayerMask.GetMask("Obstacle");
            targetLayer = LayerMask.GetMask("Player");
        }

        protected virtual void Start() {
            aStarGrid = GameObject.Find("A*")?.GetComponent<AStarGrid>();
            startPosNode = aStarGrid?.GetNodeFromPosition(startPos);
            target = GameObject.FindGameObjectWithTag(Magic.Tags.Player);

            repairBoxesContainer = GameObject.Find("RepairBoxes");
            otherEnemys = Magic.SceneHelper.GetObjectsFromRoot<EnemyAI>(GameObject.Find("Entitys"), this);
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
            if (!TargetIsInViewFieldd(head)) {
                return false;
            } return !Physics.Linecast(transform.position, target.transform.position, obstacleLayer);
        }

        public virtual bool TargetIsInViewFieldd(Transform head) {
            // target is in inner FOV
            if (Vector3.Distance(transform.position, target.transform.position) < data.viewRadius) return true;

            // target is in extendet FOV
            Vector3 directionToTarget = (target.transform.position - head.position).normalized;
            bool targetInAngle = Vector3.Angle(head.forward, directionToTarget) < data.viewAngle / 2;
            bool targetInExtendedSight = Vector3.Distance(transform.position, target.transform.position) < data.viewRadiusExtended;
            return targetInExtendedSight && targetInAngle;
        }

        public virtual Vector3 ViewDirection(Transform head, float angleInDegrees, bool angleIsGlobal) {
            if (!angleIsGlobal) {
                angleInDegrees += head.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}