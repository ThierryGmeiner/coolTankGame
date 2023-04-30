namespace Game.Entity.Tank
{
    public class TankHealth : IDamagable
    {
        private Tank tank;

        public int MaxHP { get; private set; }
        public int HP { get; private set; }

        public TankHealth(Tank tank, int maxHealth)
        {
            this.tank = tank;
            MaxHP = maxHealth;
            HP = maxHealth;
        }

        public void GetDamaged(int damage)
        {
            

        }
    }
}