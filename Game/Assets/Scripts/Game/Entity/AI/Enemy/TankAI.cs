using UnityEngine;
using Game.Entity.Tank;
using Magic;

namespace Game.AI
{
    [RequireComponent(typeof(Tank))]
    public class TankAI : EnemyAI
    {
        private TankMovement movement;

        [Header("Object")]
        [SerializeField] private Tank tank;

        protected override void Start() {
            base.Start();
            tank ??= GetComponent<Tank>();
            movement = tank.Movement;

            RandomTimer timer = gameObject.AddComponent<RandomTimer>();
            timer.OnTimerEnds += SetRandomRotationTarget;
            timer.SetupTimer(2f, 3.5f, Timer.Modes.restartWhenTimeIsUp);
            timer.StartTimer();

            if (wayPoints.Length != 0) {
                wayPointPaths = CnvertWayPointsToPaths(movement.aStar, wayPoints);
            }
        }

        private void Update() {
            StateMachine = GetState();
            StateMachine();
        }

        private System.Action GetState() {
            return wayPoints.Length == 0 ? StateStayAtStart : StateFollowPath;

            if (StateMachine == StateStayAtStart || StateMachine == StateFollowPath) {
                // can go to attack
            }
            else if (StateMachine == StateSearch) {
                // can go to attack or default/followPath
                return wayPoints.Length == 0 ? StateStayAtStart : StateFollowPath;
            }
            else if (StateMachine == StateAttack) {
                // can go to search
            }
            return StateMachine;
        }

        public void StateStayAtStart() {
            AStarNode currentPosNode = movement.grid.GetNodeFromPosition(transform.position);
            if (movement.aStar.TargetNode != startPosNode && currentPosNode != startPosNode) {
                movement.SetPath(transform.position, startPos);
            }
        }

        public void StateFollowPath() {

            movement.Path = wayPointPaths[0];



            if (Array.Contains(wayPointPaths, movement.Path)) {
                Debug.Log("contains");
            }



            //Debug.Log(wayPointPaths.Length);
            //foreach (Path path in wayPointPaths) {
            //    Debug.Log(path.Start.Position + " => " + path.Target.Position);
            //}
        }

        public void StateSearch() {

        }

        public void StateAttack() {
            movement.HeadRotationTarget = target.transform.position;

        }

        private void HandleMovement() {

            if (Vector3.Distance(transform.position, target.transform.position) < 4) {
                movement.Path = null;
                return;
            }

            if (CanSeeTarget()) {
                if (movement.Path == null || movement.Path.Nodes.Length == 0) {
                    movement.SetPath(transform.position, target.transform.position);
                }
            }
        }

        private void SetRandomRotationTarget() {
            if (StateMachine == StateStayAtStart || StateMachine == StateFollowPath) {
                movement.HeadRotationTarget = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            }
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