using System;
using UnityEngine;

namespace Game.Entity.Interactable 
{
    public class RepaiBox : MonoBehaviour, IEntity
    {
        public string Name => throw new NotImplementedException();

        public Rigidbody RigidBody => throw new NotImplementedException();

        public Collider Collider => throw new NotImplementedException();

        public event Action OnDestruction;

        public void GetDestroyed() {
            OnDestruction?.Invoke();
            Destroy(gameObject);
        }
    }
}