using System;
using UnityEngine;
using Game.AI;

namespace Game.Entity.Tank
{
    public class TankMovement
    {
        // data
        private readonly Tank tank;
        private readonly LayerMask groundLayer;

        // pathFinding
        public int pathIndex { get; private set; } = 0;
        private Path path = new Path(new AStarNode[0]);
        public readonly AStar aStar;
        public readonly AStarGrid grid;
        public event Action<Path> OnSetPath;

        // movement
        private Vector3 headRotationTarget;
        private readonly float speed = 6;
        private readonly float bodyRotationSpeed = 6;
        private readonly float headRotationSpeed = 6;

        public TankMovement(Tank tank) {
            this.tank = tank;
            
            speed = tank.Data.Movement.Speed;
            bodyRotationSpeed = tank.Data.Movement.BodyRotationSpeed;
            headRotationSpeed = tank.Data.Movement.HeadRotationSpeed;

            groundLayer = LayerMask.GetMask("Ground");
            grid = tank.SceneData.AStarGrid;
            aStar = new AStar(grid);
        }

        public float Speed { get => speed; }
        public Vector3 HeadRotationTarget { set => headRotationTarget = value; }
        public Path Path {
            get => path;
            set { pathIndex = 0; path = value; }
        }

        public void Move(Vector3 target) {
            tank.transform.position = Vector3.MoveTowards(tank.transform.position, target, speed * Time.deltaTime);

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

        public void RotateHead() {
            Rotate(tank.Head, headRotationTarget, headRotationSpeed);
        }

        public void RotateBody(Vector3 target) {
            Rotate(tank.gameObject, target, bodyRotationSpeed);
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
            if (!targetNode.IsWalkable) targetNode = grid.GetClosestWalkableNeighbor(targetNode, targetPos);

            Path newPath = aStar.FindOptimizedPath(startNode, targetNode);
            if (newPath.Nodes.Length > 0) {
                // set "Path" and not "path" that the index is overwritten
                Path = newPath;
                OnSetPath?.Invoke(Path);
            } return Path;
        }

        public Path AddPath(Vector3 targerPos) {
            if (Path != null && Path.Nodes.Length > 0) {
                // set "path" and not "Path" that the index is not overwritten
                Path additionalPath = aStar.FindOptimizedPath(Path.Target.Position, targerPos);
                path = path + additionalPath;
                OnSetPath?.Invoke(Path);
                return path;
            } 
            return SetPath(tank.transform.position, targerPos);
        }

        private bool ReachInterimTarget() => Vector3.Distance(tank.transform.position, Path.Nodes[pathIndex].Position) < 0.1;
        private bool ReachTarget() => Vector3.Distance(tank.transform.position, Path.Target.Position) < 0.1;
    }
}