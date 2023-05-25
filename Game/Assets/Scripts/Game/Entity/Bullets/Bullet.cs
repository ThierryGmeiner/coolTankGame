using UnityEngine;

namespace Game.Entity
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] private int damage;
        public int Damage { get => damage; }
        public Rigidbody RigidBody { get; private set; }
        public BoxCollider Collider { get; private set; }

        protected virtual void Awake() {
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>();
        }

        protected virtual void OnCollisionEnter(Collision collision) {
            string tag = collision.gameObject.tag;
            if (tag == "Entity" || tag == "Player") {
                collision.gameObject.GetComponent<IDamagable>().GetDamaged(damage);
            }
            else {
                Destroy(gameObject);
            }
        }

        private void OnDestroy() {
            // play sound and particles
            Debug.Log("destroy bullet");
        }
    }
}