using System;
using UnityEngine;

namespace Game.AI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] protected float viewRadius = 7;
        [SerializeField] protected float viewRadiusExtended = 12;
        [SerializeField] [Range(0, 360)] protected float viewAngle = 80;

        protected GameObject target;
        protected LayerMask obstacleLayer;
        protected Action StateMachine;
        protected Vector3 startPos;

        protected virtual void Awake() {
            startPos = transform.position;
            obstacleLayer = LayerMask.GetMask("Obstacle");
        }

        protected virtual void Start() {
            target = GameObject.FindGameObjectWithTag(Magic.Tags.Player).transform.root.gameObject;
        }

        public float ViewAngle { get => viewAngle; }
        public float ViewRadius { get => viewRadius; }
        public float ViewRadiusExtended { get => viewRadiusExtended; }

        protected virtual bool CanSeeTarget() {
            if (!TargetIsInView()) {
                return false;
            }
            return !Physics.Linecast(transform.position, target.transform.position, obstacleLayer);

        }

        protected virtual bool TargetIsInView() {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            bool targetInAngle = Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2;
            bool targetOutsideExtendedSight = Vector3.Distance(transform.position, target.transform.position) < viewRadiusExtended;

            // target is in extendet FOV
            if (targetOutsideExtendedSight && targetInAngle) return true;

            // target is in inner FOV
            bool targetInSight = Vector3.Distance(transform.position, target.transform.position) < viewRadius;
            return targetInSight;
        }

        public virtual Vector3 ViewDirection(float angleInDegrees, bool angleIsGlobal) {
            if (!angleIsGlobal) {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

    }
}