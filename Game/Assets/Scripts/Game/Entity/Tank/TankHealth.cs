namespace Game.Entity.Tank
{
    public class TankHealth : IDamagable
    {
        public int MaxHP { get; private set; }
        public int HP { get; private set; }

        public TankHealth(int maxHealth)
        {
            MaxHP = maxHealth;
            HP = maxHealth;
        }

        public void GetDamaged(int damage)
        {
            

        }
    }
}