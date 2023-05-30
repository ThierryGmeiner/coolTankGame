using System;

namespace Game.Entity.Tank
{
    public class TankHealth
    {
        private readonly Tank tank;
        private int hitPoints;
        
        public event Action<int> OnDamaged;
        public event Action<int> OnRepaired;
        public event Action OnDestruction;

        public TankHealth(Tank tank, int maxHealth) {
            this.tank = tank;
            MaxHitPoints = maxHealth;
            hitPoints = maxHealth;
        }
        public int MaxHitPoints { get; }
        public int HitPoints { get => hitPoints; }

        public void GetDamaged(int damage) {
            hitPoints -= Math.Abs(damage);
            UnityEngine.Debug.Log($"{tank.Name} has {HitPoints} hp");

            OnDamaged?.Invoke(damage);
            if (HitPoints <= 0) OnDestruction?.Invoke();
        }

        public void GetRepaired(int healing) {
            if (HitPoints + healing > MaxHitPoints) healing = MaxHitPoints - HitPoints;
            hitPoints += Math.Abs(healing);
            OnRepaired?.Invoke(healing);
        }
    }
}