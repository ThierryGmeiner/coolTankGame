using System;
using UnityEngine;
using Game.Data;
using Magic;

namespace Game.Entity.Interactable 
{
    public class RepaiBox : MonoBehaviour, IEntity
    {
        public event Action OnDestruction;

        [SerializeField] private DataInteractable data;

        private void Awake() {
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
            Health = GetComponent<InteractableHealth>();
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag(Tags.Player) || other.CompareTag(Tags.Entity)) {
                IDamaging damaging = other.GetComponent<IDamaging>();
                if (damaging!= null) {
                    Health.GetDamaged(1, damaging.DamageType);
                    other.GetComponent<IEntity>()?.GetDestroyed();
                }

                IRepairable repairable = other.GetComponent<IRepairable>();
                if (repairable == null || repairable.HasFullHP) { return; }
                repairable.GetRepaired(data.healing);
                GetDestroyed();
            }
        }

        public string Name => data.Entity.Name;
        public DataInteractable Data { get => data; set => data = value; }
        public InteractableHealth Health { get; private set; }
        public Rigidbody RigidBody { get; private set; }
        public Collider Collider { get; private set; }

        public void GetDestroyed() {
            OnDestruction?.Invoke();
            Destroy(gameObject);
        }
    }
}