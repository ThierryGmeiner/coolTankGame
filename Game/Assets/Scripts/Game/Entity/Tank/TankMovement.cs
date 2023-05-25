using UnityEngine;
using Game.AI;

namespace Game.Entity.Tank
{
    public class TankMovement
    {
        private readonly Tank tank;
        private readonly Transform groundCheck;
        private readonly LayerMask groundLayer;
        private readonly float defaultSpeed;
        private readonly float jumpForce;

        private float speed;
        private AStarNode[] path = null;
        private int pathIndex = 0;

        private const float AIR_MULTIPLIER = 0.65f;
        private const float ROTATION_SPEED = 6;

        public TankMovement(Tank tank, Transform groundCheck) {
            this.tank = tank;
            this.groundCheck = groundCheck;
            defaultSpeed = tank.Data.Speed;
            speed = tank.Data.Speed;
            jumpForce = tank.Data.JumpForce;
            groundLayer = LayerMask.GetMask("Ground");
        }

        public float Speed { get => speed; }
        public AStarNode[] Path {
            get => path;
            set { pathIndex = 0; path = value; }
        }

        public void Move(Vector2 direction) => Move(new Vector3(direction.x, 0, direction.y));

        public void Move(Vector3 target) {
            float movementSpeed = tank.IsGrounded ? speed * Time.deltaTime : speed * AIR_MULTIPLIER * Time.deltaTime;
            tank.transform.position = Vector3.MoveTowards(tank.transform.position, target, movementSpeed);
        }

        public void Move() {
            if (Path == null) return;
            
            Rotate(path[pathIndex].Position);
            Move(path[pathIndex].Position);

            if (ReachTarget()) Path = null;
            else if (ReachInterimTarget()) pathIndex++;
        }

        public void Jump() {
            if (tank.IsGrounded) {
                tank.RigidBody.AddForce(new Vector3(tank.RigidBody.velocity.x, jumpForce, tank.RigidBody.velocity.z), ForceMode.Impulse);
            }
        }

        public void Rotate(Vector3 target) {
            Vector3 _direction = (target - tank.transform.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);
            tank.transform.rotation = Quaternion.Slerp(tank.transform.rotation, _lookRotation, Time.deltaTime * ROTATION_SPEED);
        }

        public void EnableTurbo() => speed = defaultSpeed * 1.5f;

        public void DisableTurbo() => speed = defaultSpeed;

        public bool GroundCheck() => Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        private bool ReachInterimTarget() => Vector3.Distance(tank.transform.position, Path[pathIndex].Position) < 0.1;

        private bool ReachTarget() => Vector3.Distance(tank.transform.position, Path[Path.Length - 1].Position) < 0.1;
    }
}