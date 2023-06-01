using UnityEngine;
using Game.AI;

namespace Game.Entity.Tank
{
    public class TankMovement
    {
        // data
        private readonly Tank tank;
        private readonly Transform groundCheck;
        private readonly LayerMask groundLayer;

        // pathFinding
        private Path path = new Path(new AStarNode[0], Path.Optimized.True);
        private int pathIndex = 0;
        public readonly AStar aStar;
        public readonly AStarGrid grid;

        // movement
        private float speed;
        private readonly float defaultSpeed;
        private readonly float jumpForce;
        private readonly float turboMultiplier;
        private const float AIR_MULTIPLIER = 0.65f;
        private const float BODY_ROTATION_SPEED = 6;
        private const float HEAD_ROTATION_SPEED = 5;

        public TankMovement(Tank tank, Transform groundCheck) {
            this.tank = tank;
            this.groundCheck = groundCheck;
            defaultSpeed = tank.Data.Speed;
            speed = tank.Data.Speed;
            jumpForce = tank.Data.JumpForce;
            turboMultiplier = tank.Data.TurboMultiplier;

            groundLayer = LayerMask.GetMask("Ground");
            grid = GameObject.Find("A*")?.GetComponent<AStarGrid>();
            aStar = new AStar(grid);
        }

        public float Speed { get => speed; }
        public Path Path {
            get => path;
            set { pathIndex = 0; path = value; }
        }

        public void Move(Vector3 target) {
            float movementSpeed = tank.IsGrounded ? speed * Time.deltaTime : speed * AIR_MULTIPLIER * Time.deltaTime;
            tank.transform.position = Vector3.MoveTowards(tank.transform.position, target, movementSpeed);
            if (ReachTarget()) Path = null;
            else if (ReachInterimTarget()) pathIndex++;
        }

        public void Move() {
            if (Path == null || Path.Nodes.Length == 0) return;
            // FindOptimizedPath can't bee caled in a second thread, so check manuel for it an cal it when necessary
            if (!path.IsOptimized) {
                path = aStar.FindOptimizedPath(Path);
            }
            RotateBody(path.Nodes[pathIndex].Position);
            Move(path.Nodes[pathIndex].Position);
        }

        public void Jump() {
            if (tank.IsGrounded) {
                tank.RigidBody.AddForce(new Vector3(tank.RigidBody.velocity.x, jumpForce, tank.RigidBody.velocity.z), ForceMode.Impulse);
            }
        }

        public void RotateHead(Vector3 target) {
            Rotate(tank.Head, target, HEAD_ROTATION_SPEED);
        }

        public void RotateBody(Vector3 target) {
            Rotate(tank.gameObject, target, BODY_ROTATION_SPEED);
        }

        private void Rotate(GameObject obj, Vector3 target, float rotationSpeed) {
            Vector3 direction = (target - tank.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, lookRotation, Time.deltaTime * HEAD_ROTATION_SPEED);
        }

        public Path SetPath(Vector3 startPos, Vector3 targetPos) {
            Path newPath = aStar.FindPath(startPos, targetPos);
            if (newPath.Nodes.Length > 0) {
                Path = newPath;
            } return Path;
        }

        public void EnableTurbo() => speed = defaultSpeed * turboMultiplier;

        public void DisableTurbo() => speed = defaultSpeed;

        public bool GroundCheck() => Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        private bool ReachInterimTarget() => Vector3.Distance(tank.transform.position, Path.Nodes[pathIndex].Position) < 0.1;

        private bool ReachTarget() => Vector3.Distance(tank.transform.position, Path.Nodes[Path.Nodes.Length - 1].Position) < 0.1; 
    }
}