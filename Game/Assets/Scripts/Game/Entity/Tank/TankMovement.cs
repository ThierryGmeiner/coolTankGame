using UnityEngine;

namespace Game.Entity.Tank
{
    public class TankMovement
    {
        private Tank tank;
        private Transform groundCheck;
        private LayerMask groundLayer;
        private const float AIR_MULTIPLIER = 0.65f;

        public float Speed { get; private set; }

        public TankMovement(Tank tank, float speed, Transform groundCheck) {
            this.tank = tank;
            this.groundCheck = groundCheck;
            Speed = speed;
            groundLayer = LayerMask.GetMask("Ground");
        }

        public void Move(Vector3 direction) {
            direction *= Speed;
            if (!tank.IsGrounded) direction *= AIR_MULTIPLIER;

            tank.RigidBody.AddForce(direction, ForceMode.Force);
        }

        public bool GroundCheck() => Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
    }
}