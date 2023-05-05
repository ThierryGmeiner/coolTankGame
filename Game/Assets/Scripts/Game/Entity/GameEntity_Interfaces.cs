namespace Game.Entity
{
    public interface IDamagable
    {
        public void GetDamaged(int damage);
    }

    public interface IRepairable
    {
        public void GetRepaired(int healing);
    }
}