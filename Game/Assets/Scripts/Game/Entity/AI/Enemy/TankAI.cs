using UnityEngine;
using Game.Entity.Tank;
using Magic;

namespace Game.AI
{
    public class TankAI : EnemyAI
    {
        [Header("Object")]
        [SerializeField] private Tank tank;

        private Vector3 rotationTarget = Vector3.zero;

        protected override void Awake() {
            base.Awake();
            tank ??= GetComponent<Tank>();
        }

        protected override void Start() {
            base.Start();
            RandomTimer timer = gameObject.AddComponent<RandomTimer>();
            timer.OnTimerEnds += SetRandomRotationTarget;
            timer.SetupTimer(2f, 3.5f, Timer.Modes.restartWhenTimeIsUp);
            timer.StartTimer();
        }

        private void Update() {
            if (CanSeeTarget()) {

            }
            else {
                tank.Movement.RotateHead(rotationTarget);
            }
        }

        private void HandleMovement() {

            if (Vector3.Distance(transform.position, target.transform.position) < 4) {
                tank.Movement.Path = null;
                return;
            }

            if (CanSeeTarget()) {
                if (tank.Movement.Path == null || tank.Movement.Path.Nodes.Length == 0) {
                    tank.Movement.SetPath(transform.position, target.transform.position);
                }
            }
        }

        private void SetRandomRotationTarget() {
            rotationTarget = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
        }

        public override Vector3 ViewDirection(float angleInDegrees, bool angleIsGlobal) {
            if (!angleIsGlobal) {
                angleInDegrees += tank.Head.transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        protected override bool TargetIsInView() {
            Vector3 directionToTarget = (target.transform.position - tank.Head.transform.position).normalized;
            bool targetInAngle = Vector3.Angle(tank.Head.transform.forward, directionToTarget) < viewAngle / 2;
            bool targetOutsideExtendedSight = Vector3.Distance(transform.position, target.transform.position) < viewRadiusExtended;

            // target is in extendet FOV
            if (targetOutsideExtendedSight && targetInAngle) return true;

            // target is in inner FOV
            bool targetInSight = Vector3.Distance(transform.position, target.transform.position) < viewRadius;
            return targetInSight;
        }

    }
}