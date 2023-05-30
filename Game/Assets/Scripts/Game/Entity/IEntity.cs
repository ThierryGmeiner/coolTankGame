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
        public void GetDamaged(int damage);
        public event Action<int> OnDamaged;
    }

    public interface IRepairable
    {
        public void GetRepaired(int healing);
        public event Action<int> OnRepaired;
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