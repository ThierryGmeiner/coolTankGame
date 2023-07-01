using UnityEngine;
using Game.AI;

namespace Game.Entity.Tank
{
    public class TankMovement
    {
        //------------------------------------------------------------------------------//
        //------------------------------------------------------------------------------//
        // pfad mit mehreren positionen erstellen                                       //
        // min shift punkt setzen (diesen mit shift auch wieder löschen)                //
        // pfad mit zb partickel anzeigen                                               //
        //------------------------------------------------------------------------------//
        //------------------------------------------------------------------------------//
        
        // data
        private readonly Tank tank;
        private readonly Transform groundCheck;
        private readonly LayerMask groundLayer;

        // pathFinding
        private Path path = new Path(new AStarNode[0], true);
        private int pathIndex = 0;
        public readonly AStar aStar;
        public readonly AStarGrid grid;

        // movement
        private Vector3 headRotationTarget;
        private readonly float speed;
        private readonly float jumpForce;
        private const float AIR_MULTIPLIER = 0.65f;
        private const float BODY_ROTATION_SPEED = 6;
        private const float HEAD_ROTATION_SPEED = 6;

        public TankMovement(Tank tank, Transform groundCheck) {
            this.tank = tank;
            this.groundCheck = groundCheck;
            speed = tank.Data.Speed;
            jumpForce = tank.Data.JumpForce;

            groundLayer = LayerMask.GetMask("Ground");
            grid = GameObject.Find("A*")?.GetComponent<AStarGrid>();
            aStar = new AStar(grid);
        }

        public float Speed { get => speed; }
        public Vector3 HeadRotationTarget { set => headRotationTarget = value; }
        public Path Path {
            get => path;
            set { pathIndex = 0; path = value; }
        }

        public void Move(Vector3 target) {
            float moveSpeed = tank.IsGrounded ? speed * Time.deltaTime : speed * AIR_MULTIPLIER * Time.deltaTime;
            tank.transform.position = Vector3.MoveTowards(tank.transform.position, target, moveSpeed);

            if (ReachTarget()) {
                aStar.StartNode = new AStarNode(true, Vector3.zero);
                aStar.TargetNode = new AStarNode(true, Vector3.zero);
                Path = null;
            }
            else if (ReachInterimTarget()) {
                pathIndex++;
            }
        }

        public void HandleMovement() {
            RotateHead();
            if (Path == null || Path.Nodes.Length == 0) return;

            RotateBody(path.Nodes[pathIndex].Position);
            Move(path.Nodes[pathIndex].Position);
        }

        public void Jump() {
            if (tank.IsGrounded) {
                tank.RigidBody.AddForce(new Vector3(tank.RigidBody.velocity.x, jumpForce, tank.RigidBody.velocity.z), ForceMode.Impulse);
            }
        }

        public void RotateHead() {
            Rotate(tank.Head, headRotationTarget, HEAD_ROTATION_SPEED);
        }

        public void RotateBody(Vector3 target) {
            Rotate(tank.gameObject, target, BODY_ROTATION_SPEED);
        }

        private void Rotate(GameObject obj, Vector3 target, float rotationSpeed) {
            // rotate object
            Vector3 direction = (target - tank.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // to ensure only the y angle rotates
            obj.transform.rotation = new Quaternion(0, obj.transform.rotation.y, 0, obj.transform.rotation.w);
        }

        public Path SetPath(Vector3 startPos, Vector3 targetPos) {
            AStarNode startNode = grid.GetNodeFromPosition(startPos);
            AStarNode targetNode = grid.GetNodeFromPosition(targetPos);
            if (!targetNode.IsWalkable) targetNode = aStar.GetClosestWalkableNeighbor(targetNode, targetPos);

            Path newPath = aStar.FindOptimizedPath(startNode, targetNode);
            if (newPath.Nodes.Length > 0) {
                // set "Path" and not "path" that the index is overwritten
                Path = newPath;
            } return Path;
        }

        public Path AddPath(Vector3 targerPos) {
            if (Path != null && Path.Nodes.Length > 0) {
                // set "path" and not "Path" that the index is not overwritten
                Path additionalPath = aStar.FindOptimizedPath(Path.Target.Position, targerPos);
                path = path + additionalPath;
                return path;
            } 
            return SetPath(tank.transform.position, targerPos);
        }

        public bool GroundCheck() => Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
        private bool ReachInterimTarget() => Vector3.Distance(tank.transform.position, Path.Nodes[pathIndex].Position) < 0.1;
        private bool ReachTarget() => Vector3.Distance(tank.transform.position, Path.Target.Position) < 0.1;
    }
}