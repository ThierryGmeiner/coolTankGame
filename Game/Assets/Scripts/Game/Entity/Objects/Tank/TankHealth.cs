using System;
using UnityEngine;
using Game.Data;

namespace Game.Entity.Tank
{
    [RequireComponent(typeof(Tank))]
    public class TankHealth : MonoBehaviour, IDamagable, IRepairable
    {
        private Tank tank;

        public event Action<int, int, int, Vector3> OnDamaged;
        public event Action<int, int, int> OnRepaired;

        private void Start() {
            tank = GetComponent<Tank>();
            tank.Data ??= ScriptableObject.CreateInstance<DataTank>();
            MaxHitPoints = data.Health.HitPoints;
            HitPoints = MaxHitPoints;
        }

        private DataTank data => tank.Data;
        public int MaxHitPoints { get; set; }
        public int HitPoints { get; private set; }

        public void GetDamaged(int damage) {
            GetDamaged(damage, transform.position);
        }

        public void GetDamaged(int damage, Vector3 attackDirection) {
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