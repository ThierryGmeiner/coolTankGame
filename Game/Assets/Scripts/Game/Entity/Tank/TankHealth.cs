using System;

namespace Game.Entity.Tank
{
    public class TankHealth : IDamagable, IRepairable
    {
        private Tank tank;
        public event Action<int> OnDamaged;
        public event Action<int> OnRepaired;
        public event Action OnDestruction;

        public int MaxHP { get; private set; }
        public int HP { get; private set; }

        public TankHealth(Tank tank, int maxHealth) {
            this.tank = tank;
            MaxHP = maxHealth;
            HP = maxHealth;
        }

        public void GetDamaged(int damage) {
            HP -= Math.Abs(damage);
            OnDamaged?.Invoke(damage);
            if (HP <= 0) OnDestruction?.Invoke();
        }

        public void GetRepaired(int healing) {
            if (HP + healing > MaxHP) healing = MaxHP - HP;
            HP += Math.Abs(healing);
            OnRepaired?.Invoke(healing);
        }
    }
}