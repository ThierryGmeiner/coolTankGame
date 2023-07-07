using System;
using UnityEngine;
using Game.Data;

namespace Game.Entity.Tank
{
    [RequireComponent(typeof(Tank))]
    public class TankHealth : Health, IDamagable, IRepairable
    {
        private Tank tank;

        public event Action<int, int, int, Vector3> OnDamaged;
        public event Action<int, int, int> OnRepaired;

        private void Start() {
            tank = GetComponent<Tank>();
            tank.Data ??= ScriptableObject.CreateInstance<DataTank>();
            SetupHitPoints(data.Health.HitPoints);
        }

        private DataTank data => tank.Data;

        public void GetDamaged(int damage, DamageType damageType) {
            GetDamaged(damage, damageType, transform.position);
        }

        public void GetDamaged(int damage, DamageType damageType, Vector3 attackDirection) {
            HitPoints -= Math.Abs(damage);
            OnDamaged?.Invoke(MaxHitPoints, HitPoints, damage, attackDirection);
        }

        public void GetRepaired(int healing) {
            if (HitPoints + healing > MaxHitPoints) healing = MaxHitPoints - HitPoints;
            HitPoints += Math.Abs(healing);
            OnRepaired?.Invoke(MaxHitPoints, HitPoints, healing);
        }
    }
}