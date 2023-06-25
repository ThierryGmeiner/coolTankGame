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

        public void GetDamaged(int damage);
        public void GetDamaged(int damage, Vector3 attackDirection);
        public event Action<int, int, int, Vector3> OnDamaged; // maxHP, currentHP, damage, damageDirection
    }

    public interface IRepairable
    {
        public int MaxHitPoints { get; }
        public int HitPoints { get; }

        public void GetRepaired(int healing);
        public event Action<int, int, int> OnRepaired; // maxHP, currentHP, healing
    }

    public interface IRangeAttack
    {
        public int MaxShotsUntilCooldown { get; }
        public int ShotsUntilCooldown { get; }

        public Bullet Shoot(Quaternion direction);
        public event Action OnShoot;
        public event Action OnReload;
        public event Action OnUpdateShotsUntilCooldown;
    }

    public interface IDropMine
    {
        public void DropMine();
        public event Action OnDropMine;
    }
}