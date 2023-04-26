namespace Game.Entity.Tank
{
    public class TankMovement
    {
        public float Speed { get; private set; }

        public TankMovement(float speed)
        {
            Speed = speed;
        }
    }
}