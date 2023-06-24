using System;
using UnityEngine;

namespace Game.Entity.Tank
{
    [RequireComponent(typeof(Tank))]
    public class TankHealth : MonoBehaviour, IDamagable, IRepairable
    {        
        public event Action<int, int, int, Vector3> OnDamaged;
        public event Action<int, int, int> OnRepaired;

        [SerializeField] private TankData data;

        private void Awake() {
            data ??= ScriptableObject.CreateInstance<TankData>();
            MaxHitPoints = data.Health;
            HitPoints = MaxHitPoints;
        }

        public int MaxHitPoints { get; set; }
        public int HitPoints { get; private set; }
        public TankData Data {
            get { return data; }
            set { data = value; }
        }


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