using System;
using UnityEngine;

namespace Game.Entity
{
    public interface IEntity
    {
        public string Name { get; }
        public Rigidbody RigidBody { get; }
        public Collider Collider { get; }
        public void GetDestroyed();
        public event Action OnDestruction;
    }

    public interface IDamagable
    {
        public int MaxHitPoints { get; }
        public int HitPoints { get; }
        public bool HasFullHP { get; }
        public void GetDamaged(int damage, Health.DamageType damageType);
        public void GetDamaged(int damage, Health.DamageType damageType, Vector3 attackDirection);

        public event Action<int, int, int, Vector3> OnDamaged; // maxHP, currentHP, damage, damageDirection
    }

    public interface IRepairable
    {
        public int MaxHitPoints { get; }
        public int HitPoints { get; }
        public bool HasFullHP { get; }

        public void GetRepaired(int healing);
        public event Action<int, int, int> OnRepaired; // maxHP, currentHP, healing
    }

    public interface IDamaging
    {
        public Health.DamageType DamageType { get; }
    }

    public interface IRangeAttack
    {
        public int MaxShotsUntilCooldown { get; }
        public int RemainingShots { get; }

        public Bullet Shoot(Vector3 direction);
        public event Action OnShoot;
        public event Action OnReload;
    }

    public interface IDropMine
    {
        public void DropMine();
        public event Action OnDropMine;
    }
}