using UnityEngine;

namespace Game.Entity.Tank
{
    public class TankMovement
    {
        private Tank tank;
        private Transform groundCheck;
        private LayerMask groundLayer;
        private const float AIR_MULTIPLIER = 0.65f;

        private float defaultSpeed;
        private float speed;
        private float jumpForce;

        public float Speed { get => speed; }

        public TankMovement(Tank tank, Transform groundCheck) {
            this.tank = tank;
            this.groundCheck = groundCheck;
            this.defaultSpeed = tank.Data.Speed;
            this.speed = tank.Data.Speed;
            this.jumpForce = tank.Data.JumpForce;
            groundLayer = LayerMask.GetMask("Ground");
        }

        public void Move(Vector2 direction) => Move(new Vector3(direction.x, 0, direction.y));

        public void Move(Vector3 direction) {
            direction = tank.IsGrounded ? direction * speed : direction * speed * AIR_MULTIPLIER; 
            tank.RigidBody.AddForce(direction, ForceMode.Force);
        }

        public void Jump() {
            if (tank.IsGrounded) {
                tank.RigidBody.AddForce(new Vector3(tank.RigidBody.velocity.x, jumpForce, tank.RigidBody.velocity.z), ForceMode.Impulse);
            }
        }

        public void EnableTurbo() {
            speed = defaultSpeed * 1.5f;
        }

        public void DisableTurbo() {
            speed = defaultSpeed;
        }

        public bool GroundCheck() => Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
    }
}