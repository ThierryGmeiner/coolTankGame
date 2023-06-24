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
        private Vector3 movingTarget;

        [Header("Object")]
        [SerializeField] private Tank tank;

        public Vector3 MovingTarget { get => movingTarget; }
        public Tank Tank { get => tank; }

        protected override void Start() {
            base.Start();
            tank ??= GetComponent<Tank>();
            movement = tank.Movement;
            wayPointPaths = movement.aStar.CnvertWayPointsToPaths(wayPoints);

            headRotationTimer = gameObject.AddComponent<RandomTimer>();
            headRotationTimer.OnTimerEnds += SetRandomRotationTarget;
            headRotationTimer.SetupTimer(2f, 3.5f, Timer.Modes.restartWhenTimeIsUp);
            headRotationTimer.StartTimer();

            GetComponent<TankHealth>().OnDamaged += RotateTowardsDamageSource;
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
                    return StateAttackOffensive;
                }
            }
            // in searching: can go to attack or default/followPath
            else if (StateMachine == StateSearch) {
                if (canSeeTarget) {
                    return StateAttackOffensive;
                }
                if (searchTimer.timeSec <= 0) {
                    return wayPoints.Length == 0 ? StateStayAtStart : StateFollowPath;
                }
            }
            // in attack defensive: can go to attack offensive
            else if (StateMachine == StateAttackDefensive) {
                // create a score from 1 to 100
                // 50% of the scor coms from max HP the other 50% from amount of amo the tank can shoot until cooldown
                // if the score is over 30 change to to offensive
                return StateAttackOffensive;
            }
            // in attack offensive: can go to search or attack defensive
            else if (StateMachine == StateAttackOffensive) {
                if (!canSeeTarget) {
                    return StateSearch;
                }
                // if attackScore is under 30: return stateAttackDefensive
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

        public void StateAttackDefensive() {

        }

        public void StateAttackOffensive() {
            movement.HeadRotationTarget = target.transform.position;
            HandleOffensiveMovement();
            HandOffensiveleAttack();
        }

        public void HandOffensiveleAttack() {

        }

        public void HandleOffensiveMovement() {
            movingTarget = GetAttackTargetPoss();
            if (movement.grid.GetNodeFromPosition(movingTarget) != movement.aStar.TargetNode) {
                if (!Physics.Linecast(transform.position, movingTarget, obstacleLayer)) {
                    movement.SetPath(transform.position, movingTarget);
                }
            }
        }

        private Vector3 GetAttackTargetPoss() {
            float distance = Vector3.Distance(movingTarget, target.transform.position);
            if (distance > preferTargetDistanceMin && distance < preferTargetDistanceMax) return movingTarget;

            float newTargetDistance = MathM.Mid(preferTargetDistanceMin, preferTargetDistanceMax);
            return MathM.ClosestPointOfCircle(transform.position, target.transform.position, newTargetDistance);
        }

        private void RotateTowardsDamageSource(int maxHP, int hp, int damage, Vector3 direction) {
            if (StateMachine != StateAttackOffensive) {
                movement.HeadRotationTarget = new Vector3(direction.x, 0, direction.z);
                headRotationTimer.Restart();
            }
        }

        private void SetRandomRotationTarget() {
            if (StateMachine != StateAttackOffensive) {
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