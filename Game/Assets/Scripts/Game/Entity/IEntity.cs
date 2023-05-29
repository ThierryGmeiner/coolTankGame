using UnityEngine;

namespace Game.Entity
{
    public interface IEntity
    {
        public Rigidbody RigidBody { get; }
        public BoxCollider Collider { get; }
        public void GetDestroyed();
        public void GetBeaten(int damage);
    }

    public interface IDamagable
    {
        public int MaxHitPoints { get; }
        public int HitPoints { get; }
        public void GetDamaged(int damage);
    }

    public interface IRepairable
    {
        public int MaxHitPoints { get; }
        public int HitPoints { get; }
        public void GetRepaired(int healing);
    }
}