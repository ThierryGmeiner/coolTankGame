using UnityEngine;

namespace Game.Entity.Tank
{
    public class TankMovement
    {
        private Tank tank;

        public float Speed { get; private set; }

        public TankMovement(Tank tank, float speed)
        {
            this.tank = tank;
            Speed = speed;
        }

        public void Move(Vector3 direction)
        {
            if (!IsGrounded()) direction = new Vector3(direction.x * 0.65f, 0, direction.z * 0.65f);
            tank.RigidBody.AddForce(new Vector3(direction.x, 0, direction.z), ForceMode.Force);
        }

        public bool IsGrounded()
        {
            return false;
        }
    }
}