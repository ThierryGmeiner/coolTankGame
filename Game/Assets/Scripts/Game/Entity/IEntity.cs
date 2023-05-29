using UnityEngine;

namespace Game.Entity
{
    public interface IEntity
    {
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
}