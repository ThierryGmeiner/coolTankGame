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

        [Header("Object")]
        [SerializeField] private Tank tank;

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
            if (StateMachine == null) {
                return wayPoints.Length == 0 ? StateStayAtStart : StateFollowPath;
            }
            else if (StateMachine == StateStayAtStart || StateMachine == StateFollowPath) {
                // can go to attack
                if (CanSeeTarget(tank.Head.transform)) {
                    return StateAttack;
                }
            }
            else if (StateMachine == StateSearch) {
                // can go to attack or default/followPath
                if (CanSeeTarget(tank.Head.transform)) {
                    return StateAttack;
                }
                if (searchTimer.timeInSeconds <= 0) {
                    return wayPoints.Length == 0 ? StateStayAtStart : StateFollowPath;
                }
            }
            else if (StateMachine == StateAttack) {
                // can go to search
                if (!CanSeeTarget(tank.Head.transform)) {
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


            Vector3 targetPos;

            if (Vector3.Distance(transform.position, target.transform.position) > preferTargetDistance) {
                Debug.Log("to faraway");
                targetPos = MathM.ClosestPointOfCircle(transform.position, target.transform.position, preferTargetDistance);
            }
            else {
                // watch if nothing is in between the player and enemy
                // else it is possible the tank gos behind a wall
                Debug.Log("to close");
                targetPos = MathM.ClosestPointOfCircle(target.transform.position, transform.position, preferTargetDistance);
            }

            if (movement.grid.GetNodeFromPosition(targetPos) != movement.aStar.TargetNode) {
                Debug.Log("set path");
                Vector3 currentPos= transform.position;
                System.Threading.Thread t = new System.Threading.Thread( () 
                    => movement.SetPath(currentPos, targetPos));
                t.Start();
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
    }
}