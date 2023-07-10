using UnityEngine;
using Game.Entity.Tank;
using Game.Data;
using Magic;

namespace Game.AI
{
    [RequireComponent(typeof(Tank))]
    public class TankAI : EnemyAI
    {
        private Tank tank;
        private TankMovement movement;
        private TankAttack attack;
        private PlannedTimer searchTimer;
        private RandomTimer headRotationTimer;
        private Vector3 movingTarget;

        public Tank Tank { get => tank; }

        protected override void Start() {
            base.Start();
            tank = GetComponent<Tank>();
            attack = tank.Attack;
            movement = tank.Movement;
            wayPointPaths = movement.aStar.CnvertWayPointsToPaths(data.wayPoints);

            SetupHeadRotationTimer();
            tank.Health.OnDamaged += RotateTowardsDamageSource;
        }

        private void SetupHeadRotationTimer() {
            headRotationTimer = gameObject.AddComponent<RandomTimer>();
            headRotationTimer.OnTimerEnds += SetRandomRotationTarget;
            headRotationTimer.SetupTimer(2f, 3.5f, Timer.Modes.restartWhenTimeIsUp, "RandomHeadRotation");
            headRotationTimer.StartTimer();
        }

        private void Update() {
            if (CanSeeTarget(tank.Head.transform)) {
                AStarNode playerNode = movement.aStar.Grid.GetNodeFromPosition(target.transform.position);
                foreach (EnemyAI others in otherEnemys) { others.lastVisualContact = playerNode; }
                lastVisualContact = playerNode;
            }
            StateMachine = GetState();
            StateMachine();
        }

        private System.Action GetState() {
            bool canSeeTarget = CanSeeTarget(head: tank.Head.transform);

            if (StateMachine == null) {
                return data.wayPoints.Length == 0 ? StateStayAtStart : StateFollowPath;
            }
            // in default: can go to attack
            else if (StateMachine == StateStayAtStart || StateMachine == StateFollowPath) {
                if (canSeeTarget) return StateAttack;
            }
            // in searching: can go to attack or default/followPath
            else if (StateMachine == StateSearch) {
                if (canSeeTarget) return StateAttack;
                if (searchTimer.timeSec <= 0) return data.wayPoints.Length == 0 ? StateStayAtStart : StateFollowPath;
            }
            // in take cover: can go to attack
            else if (StateMachine == StateTakeCover) {
                if (!attack.IsReloading) return StateAttack;
                if (IsDefensive) {
                    if (leftShotsInPrecent >= 100) return StateAttack;
                }
                else {
                    if (leftShotsInPrecent >= 100 - data.aggressiveness) return StateAttack;
                }
            }
            // in attack: can go to search or take cover
            else if (StateMachine == StateAttack) {
                if (!canSeeTarget) return StateSearch;
                if (attack.RemainingShots <= 0) return StateTakeCover;
            }
            return StateMachine;
        }

        public bool IsDefensive => hpInPrecent < data.changeToDefenseMode;
        private int hpInPrecent => 100 * tank.Health.HitPoints / tank.Health.MaxHitPoints;
        private int leftShotsInPrecent => 100 * attack.RemainingShots / attack.MaxShotsUntilCooldown;

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
                searchTimer.SetupTimer(Random.Range(8, 12), Timer.Modes.destroyWhenTimeIsUp, "SearchTimer");
                searchTimer.StartTimer();
            }

            if (movement.aStar.TargetNode != lastVisualContact) {
                movement.SetPath(transform.position, lastVisualContact.Position);
            }
        }

        public void StateTakeCover() {
            HandleTakeCover_Movement();
            HandleTakeCover_Attack();
        }

        private void HandleTakeCover_Attack() {
            if (TargetIsInScope(tank.Head.transform, 1)) {
                attack.Shoot(MathM.ConvertToVector3(tank.Head.transform.rotation.eulerAngles.y));
            }
        }

        private void HandleTakeCover_Movement() {
            if (hpInPrecent < data.changeToDefenseMode * 1.7f) {
                Transform closestRepairBox = GetClosestRepairbox();
                if (InteracableIsCloseEnough(closestRepairBox)) {
                    movement.SetPath(transform.position, closestRepairBox.position);
                    return;
                }
            }
            if (!Physics.Linecast(transform.position, target.transform.position, obstacleLayer)) {
                movement.HeadRotationTarget = target.transform.position;
                GetCover();
            }
        }

        private bool InteracableIsCloseEnough(Transform interactable) {
            if (interactable == null) { return false; }
            
            float range = IsDefensive ? ViewRadiusExtended * 0.75f : ViewRadiusExtended / 2;
            return Vector3.Distance(transform.position, interactable.position) < range;
        }

        private Transform GetClosestRepairbox() {
            float distance = float.MaxValue;
            Transform closest = null;

            for (int i = 0; i < repairBoxesContainer.transform.childCount; i++) {
                Transform current = repairBoxesContainer.transform.GetChild(i);
                float currentDistance = Vector3.Distance(transform.position, current.position);
                
                if (currentDistance < distance && !Physics.Linecast(transform.position, current.position, obstacleLayer)) {
                    closest = current;
                    distance = currentDistance;
                }
            } 
            return closest;
        }

        private bool IsInReach(Vector3 pos, float maxDistance)
            => Vector3.Distance(transform.position, pos) < maxDistance / 2 && !Physics.Linecast(transform.position, pos, obstacleLayer);

        private void GetCover() {
            AStarNode cover = movement.grid.GetCoveredNode(transform.position, target, 10);
            if (movement.aStar.Grid.GetNodeFromPosition(transform.position) != cover && movement.aStar.TargetNode != cover) {
                movement.SetPath(transform.position, cover.Position);
            }
        }

        public void StateAttack() {
            movement.HeadRotationTarget = target.transform.position;
            HandleAttack_Movement();
            HandleAttack_Attack();
        }

        private void HandleAttack_Attack() {
            if (TargetIsInScope(tank.Head.transform, 1)) {
                attack.Shoot(MathM.ConvertToVector3(tank.Head.transform.rotation.eulerAngles.y));
            }
        }

        private void HandleAttack_Movement() {
            movingTarget = GetAttackTargetPoss();
            if (movement.grid.GetNodeFromPosition(movingTarget) != movement.aStar.TargetNode) {
                if (!Physics.Linecast(transform.position, movingTarget, obstacleLayer)) {
                    movement.SetPath(transform.position, movingTarget);
                }
            }
        }

        private Vector3 GetAttackTargetPoss() {
            float distance = Vector3.Distance(movingTarget, target.transform.position);
            if (distance > data.preferTargetDistanceMin && distance < data.preferTargetDistanceMax) return movingTarget;

            float newTargetDistance = MathM.Mid(data.preferTargetDistanceMin, data.preferTargetDistanceMax);
            return MathM.ClosestPointOfCircle(transform.position, target.transform.position, newTargetDistance);
        }

        protected override void RotateTowardsDamageSource(int maxHP, int hp, int damage, Vector3 direction) {
            if (StateMachine != StateAttack) {
                movement.HeadRotationTarget = new Vector3(direction.x, 0, direction.z);
                headRotationTimer.Restart();
                headRotationTimer.IncreaseTime(5);
            }
        }

        private void SetRandomRotationTarget() {
            if (StateMachine != StateAttack && StateMachine != StateTakeCover) {
                movement.HeadRotationTarget = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            }
        }
    }
}