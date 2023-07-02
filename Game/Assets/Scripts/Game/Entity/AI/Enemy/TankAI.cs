using UnityEngine;
using Game.Entity.Tank;
using Magic;

namespace Game.AI
{
    [RequireComponent(typeof(Tank))]
    public class TankAI : EnemyAI
    {
        private TankMovement movement;
        private TankAttack attack;
        private PlannedTimer searchTimer;
        private RandomTimer headRotationTimer;
        private Vector3 movingTarget;

        [Header("Attack")]
        [SerializeField] [Range(1, 90)] protected int aggressiveness = 80;
        [SerializeField] [Range(1, 90)] protected int anxiety = 50;
        private int hpInPrecent;
        private int attackCooldownInPrecent;

        [Header("Object")]
        [SerializeField] private Tank tank;

        public Vector3 MovingTarget { get => movingTarget; }
        public Tank Tank { get => tank; }

        protected override void Start() {
            base.Start();
            tank ??= GetComponent<Tank>();
            attack ??= GetComponent<TankAttack>();
            movement = tank.Movement;
            wayPointPaths = movement.aStar.CnvertWayPointsToPaths(wayPoints);

            SetupHeadRotationTimer();

            GetHpAttackRatio();
            attack.OnUpdateShotsUntilCooldown += GetHpAttackRatio;
            tank.Health.OnDamaged += (int a, int b, int c, Vector3 d) => GetHpAttackRatio();
            tank.Health.OnRepaired += (int a, int b, int c) => GetHpAttackRatio();
            tank.Health.OnDamaged += RotateTowardsDamageSource;
        }

        private void SetupHeadRotationTimer() {
            headRotationTimer = gameObject.AddComponent<RandomTimer>();
            headRotationTimer.OnTimerEnds += SetRandomRotationTarget;
            headRotationTimer.SetupTimer(2f, 3.5f, Timer.Modes.restartWhenTimeIsUp);
            headRotationTimer.StartTimer();
        }

        private void Update() {
            if (CanSeeTarget(tank.Head.transform)) {
                lastVisualContact = movement.aStar.Grid.GetNodeFromPosition(target.transform.position);
            }

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
                if (canSeeTarget) return StateAttackOffensive;
            }
            // in searching: can go to attack or default/followPath
            else if (StateMachine == StateSearch) {
                if (canSeeTarget) return StateAttackOffensive;
                if (searchTimer.timeSec <= 0) return wayPoints.Length == 0 ? StateStayAtStart : StateFollowPath;
            }
            // in attack defensive: can go to attack offensive
            else if (StateMachine == StateAttackDefensive) {
                if (attackCooldownInPrecent >= aggressiveness) return StateAttackOffensive;
            }
            // in attack offensive: can go to search or attack defensive
            else if (StateMachine == StateAttackOffensive) {
                if (!canSeeTarget) return StateSearch;
                if (attackCooldownInPrecent == 0) return StateAttackDefensive;
                if (attackCooldownInPrecent + hpInPrecent < anxiety * 2) return StateAttackDefensive;
            }
            return StateMachine;
        }

        private void GetHpAttackRatio() {
            hpInPrecent = 100 * tank.Health.HitPoints / tank.Health.MaxHitPoints;
            attackCooldownInPrecent = 100 * attack.remainingShots / attack.MaxShotsUntilCooldown;
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
                searchTimer.SetupTimer(Random.Range(8, 12), Timer.Modes.destroyWhenTimeIsUp);
                searchTimer.StartTimer();
            }

            if (movement.aStar.TargetNode != lastVisualContact) {
                movement.SetPath(transform.position, lastVisualContact.Position);
            }
        }

        public void StateAttackDefensive() {
            HandleDefensiveMovement();
            HandleDefensiveAttack();
        }

        private void HandleDefensiveAttack() {
            if (TargetIsInScope(tank.Head.transform, 0.2f)) {
                attack.Shoot(MathM.ConvertToVector3(tank.Head.transform.rotation.eulerAngles.y));
            }
        }

        private void HandleDefensiveMovement() {
            if (!Physics.Linecast(transform.position, target.transform.position, obstacleLayer)) {
                movement.HeadRotationTarget = target.transform.position;
                GetCover();
            }
        }

        private void GetCover() {
            AStarNode cover = movement.grid.GetCoveredNode(transform.position, target, 10);
            if (movement.aStar.Grid.GetNodeFromPosition(transform.position) != cover && movement.aStar.TargetNode != cover) {
                movement.SetPath(transform.position, cover.Position);
            }
        }

        public void StateAttackOffensive() {
            movement.HeadRotationTarget = target.transform.position;
            HandleOffensiveMovement();
            HandleOffensiveAttack();
        }

        private void HandleOffensiveAttack() {
            if (TargetIsInScope(tank.Head.transform, 0.5f)) {
                attack.Shoot(MathM.ConvertToVector3(tank.Head.transform.rotation.eulerAngles.y));
            }
        }

        private void HandleOffensiveMovement() {
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

        protected override void RotateTowardsDamageSource(int maxHP, int hp, int damage, Vector3 direction) {
            if (StateMachine != StateAttackOffensive) {
                movement.HeadRotationTarget = new Vector3(direction.x, 0, direction.z);
                headRotationTimer.Restart();
                headRotationTimer.IncreaseTime(5);
            }
        }

        private void SetRandomRotationTarget() {
            if (StateMachine != StateAttackOffensive && StateMachine != StateAttackDefensive) {
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