using System;
using UnityEngine;

namespace Game.Entity.Interactable
{
    public class InteractableHealth : Health, IDamagable
    {
        [SerializeField] private int requiredHits = 3;
        private IEntity entity;

        public event Action<int, int, int, Vector3> OnDamaged;

        private void Awake() {
            SetupHitPoints(requiredHits);
        }

        private void Start() {
            entity = GetComponent<IEntity>();
        }

        public void GetDamaged(int damage, DamageType damageType) {
            if (damageType == DamageType.Explosion) {
                entity.GetDestroyed();
            } 
            else {
                HitPoints--;
                if (HitPoints <= 0) entity.GetDestroyed();
            } 
            OnDamaged?.Invoke(MaxHitPoints, HitPoints, damage, Vector3.zero);
        }

        public void GetDamaged(int damage, DamageType damageType, Vector3 attackDirection) => GetDamaged(damage, damageType);
    }
}