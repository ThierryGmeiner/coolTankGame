using UnityEngine;
using Magic;

namespace Game.Entity
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] protected new string name;
        [SerializeField] protected int damage;
        [SerializeField] private float shootingSpeed;
        
        public string Name { get => name; }
        public int Damage { get => damage; }
        public Rigidbody RigidBody { get; private set; }
        public BoxCollider Collider { get; private set; }

        protected virtual void Awake() {
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>();
        }

        protected virtual void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.tag == Tags.Entity || collision.gameObject.tag == Tags.Player) {
                collision.gameObject.GetComponent<IDamagable>()?.GetDamaged(damage);
                return;
            }
            Destroy(gameObject);
        }

        public virtual void Shoot(Vector3 direction) {
            Debug.Log("shoot bullet");
            RigidBody.AddForce(direction.normalized * shootingSpeed, ForceMode.Impulse);
        }

        private void OnDestroy() {
            // play sound and particles
            Debug.Log("destroy bullet");
        }
    }
}