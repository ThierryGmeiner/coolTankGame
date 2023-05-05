using UnityEngine.InputSystem;
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

        public float Speed { get => speed; }

        public TankMovement(Tank tank, float speed, Transform groundCheck) {
            this.tank = tank;
            this.groundCheck = groundCheck;
            this.defaultSpeed = speed;
            this.speed = speed;
            groundLayer = LayerMask.GetMask("Ground");
        }

        public void Move(Vector2 direction) => Move(new Vector3(direction.x, 0, direction.y));

        public void Move(Vector3 direction) {
            direction *= speed;
            if (!tank.IsGrounded) direction *= AIR_MULTIPLIER;

            tank.RigidBody.AddForce(direction, ForceMode.Force);
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