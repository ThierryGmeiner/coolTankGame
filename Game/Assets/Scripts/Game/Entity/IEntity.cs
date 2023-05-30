using UnityEngine;

namespace Game.Entity
{
    public interface IEntity
    {
        public string Name { get; }
        public Rigidbody RigidBody { get; }
        public BoxCollider Collider { get; }
        public void GetDestroyed();
    }

    public interface IDamagable
    {
        public void GetDamaged(int damage);
    }

    public interface IRepairable
    {
        public void GetRepaired(int healing);
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