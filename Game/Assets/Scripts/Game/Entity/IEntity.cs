using System;
using UnityEngine;

namespace Game.Entity
{
    public interface IEntity
    {
        public string Name { get; }
        public Rigidbody RigidBody { get; }
        public BoxCollider Collider { get; }
        public void GetDestroyed();
        public event Action OnDestruction;
    }

    public interface IDamagable
    {
        public int MaxHitPoints { get; }
        public int HitPoints { get; }

        public void GetDamaged(int damage);
        public event Action<int, int, int> OnDamaged; // maxHP, currentHP, damage
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
        public void Shoot(Vector3 direction);
    }

    public interface IDropMine
    {
        public void DropMine();
    }
}