using System;

namespace Game.Entity.Tank
{
    public class TankHealth : IDamagable, IRepairable
    {
        private readonly Tank tank;
        public event Action<int> OnDamaged;
        public event Action<int> OnRepaired;
        public event Action OnDestruction;

        public TankHealth(Tank tank, int maxHealth) {
            this.tank = tank;
            MaxHitPoints = maxHealth;
            HitPoints = maxHealth;
        }
        public int MaxHitPoints { get; }
        public int HitPoints { get; private set; }

        public void GetDamaged(int damage) {
            HitPoints -= Math.Abs(damage);
            UnityEngine.Debug.Log($"{tank.Name} has {HitPoints} hp");

            OnDamaged?.Invoke(damage);
            if (HitPoints <= 0) OnDestruction?.Invoke();
        }

        public void GetRepaired(int healing) {
            if (HitPoints + healing > MaxHitPoints) healing = MaxHitPoints - HitPoints;
            HitPoints += Math.Abs(healing);
            OnRepaired?.Invoke(healing);
        }
    }
}