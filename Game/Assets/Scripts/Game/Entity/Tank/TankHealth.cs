using System;
using UnityEngine;

namespace Game.Entity.Tank
{
    [RequireComponent(typeof(Tank))]
    public class TankHealth : MonoBehaviour, IDamagable, IRepairable
    {
        private Tank tank;
        
        public event Action<int, int, int> OnDamaged;
        public event Action<int, int, int> OnRepaired;

        private void Start() {
            tank = GetComponent<Tank>();
            MaxHitPoints = tank.Data.Health;
            HitPoints = MaxHitPoints;
        }

        public int MaxHitPoints { get; set; }
        public int HitPoints { get; private set; }

        public void GetDamaged(int damage) {
            HitPoints -= Math.Abs(damage);
            OnDamaged?.Invoke(MaxHitPoints, HitPoints, damage);
        }

        public void GetRepaired(int healing) {
            if (HitPoints + healing > MaxHitPoints) healing = MaxHitPoints - HitPoints;
            HitPoints += Math.Abs(healing);
            OnRepaired?.Invoke(MaxHitPoints, HitPoints, healing);
        }
    }
}