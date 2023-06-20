using UnityEngine;
using Game.Entity.Tank;
using Magic;

namespace Game.AI
{
    [RequireComponent(typeof(Tank))]
    public class TankAI : EnemyAI
    {
        private TankMovement movement;
        private PlannedTimer searchTimer;
        private RandomTimer headRotationTimer;
        protected Vector3 targetPos;

        [Header("Object")]
        [SerializeField] private Tank tank;
        [SerializeField] GameObject obj;

        protected override void Start() {
            base.Start();
            tank ??= GetComponent<Tank>();
            movement = tank.Movement;
            wayPointPaths = movement.aStar.CnvertWayPointsToPaths(wayPoints);

            headRotationTimer = gameObject.AddComponent<RandomTimer>();
            headRotationTimer.OnTimerEnds += SetRandomRotationTarget;
            headRotationTimer.SetupTimer(2f, 3.5f, Timer.Modes.restartWhenTimeIsUp);
            headRotationTimer.StartTimer();
        }

        private void Update() {
            StateMachine = GetState();
            StateMachine();
        }

        private System.Action GetState() {
            bool canSeeTarget = CanSeeTarget(head: tank.Head.transform);

            if (StateMachine == null) {
                return wayPoints.Length == 0 ? StateStayAtStart : StateFollowPath;
            }
            // in default: can go to attack
            else if (StateMachine == StateStayAtStart || StateMachine == StateFollowPath) {
                if (canSeeTarget) {
                    return StateAttack;
                }
            }
            // in searching: can go to attack or default/followPath
            else if (StateMachine == StateSearch) {
                if (canSeeTarget) {
                    return StateAttack;
                }
                if (searchTimer.timeInSeconds <= 0) {
                    return wayPoints.Length == 0 ? StateStayAtStart : StateFollowPath;
                }
            }
            // in attack: can go to search
            else if (StateMachine == StateAttack) {
                if (!canSeeTarget) {
                    return StateSearch;
                }
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
            if (movement.Path == null || movement.Path.Nodes.Length == 0) {
                movement.Path = wayPointPaths;
            }
        }

        public void StateSearch() {
            if (searchTimer == null) {
                searchTimer = gameObject.AddComponent<PlannedTimer>();
                searchTimer.SetupTimer(Random.Range(6, 15), Timer.Modes.destroyWhenTimeIsUp);
                searchTimer.StartTimer();
            }
        }

        public void StateAttack() {
            movement.HeadRotationTarget = target.transform.position;
            StateAttack_HandleMovement();
            StateAttack_HandleAttack();
        }

        public void StateAttack_HandleAttack() {

        }

        public void StateAttack_HandleMovement() {
            targetPos = GetAttackTargetPoss();
            if (movement.grid.GetNodeFromPosition(targetPos) != movement.aStar.TargetNode) {
                movement.SetPath(transform.position, targetPos);
            }
            if (obj != null) obj.transform.position = movement.aStar.TargetNode.Position;
        }

        private Vector3 GetAttackTargetPoss() {
            Vector3 newTargetPos = MathM.ClosestPointOfCircle(transform.position, target.transform.position, preferTargetDistance);
            return Physics.Linecast(transform.position, targetPos, obstacleLayer) ? targetPos : newTargetPos;
        }

        private void SetRandomRotationTarget() {
            if (StateMachine != StateAttack) {
                movement.HeadRotationTarget = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            }
        }

        public override Vector3 ViewDirection(float angleInDegrees, bool angleIsGlobal) {
            if (!angleIsGlobal) {
                angleInDegrees += tank.Head.transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}