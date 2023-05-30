using UnityEngine;
using Magic;

namespace Game.Entity
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(PlannedTimer))]
    public abstract class Bullet : MonoBehaviour, IEntity, IDamagable
    {
        [Header("Attack")]
        [SerializeField] protected new string name;
        [SerializeField] protected int damage;
        [SerializeField] private float shootingSpeed;

        [Space]
        [Header("Lifetime")]
        [SerializeField] private float lifeTime;
        private PlannedTimer timer;

        private new BoxCollider collider;
        private Rigidbody rigidBody;
        
        public string Name { get => name; }
        public int Damage { get => damage; }
        public GameObject ShootingEntity { get; set; } // the object that shoots the bullet
        public Rigidbody RigidBody { get => rigidBody; }
        public BoxCollider Collider { get => collider; }

        protected virtual void Awake() {
            rigidBody = GetComponent<Rigidbody>();
            collider = GetComponent<BoxCollider>();
            timer = GetComponent<PlannedTimer>();
        }

        private void Start() {
            timer.OnTimerEnds += () => Destroy(gameObject);
            timer.SetupTimer(lifeTime, Timer.Modes.destroyWhenTimeIsUp);
            timer.StartTimer();
        }   

        protected virtual void OnCollisionEnter(Collision collision) {
            if (collision.gameObject == ShootingEntity) return;
            if (collision.gameObject.tag == Tags.Entity || collision.gameObject.tag == Tags.Player) {
                collision.gameObject.GetComponent<IDamagable>()?.GetDamaged(damage);
            }
            GetDestroyed();
        }

        public virtual void Shoot(Vector3 direction) {
            RigidBody.AddForce(direction.normalized * shootingSpeed, ForceMode.Impulse);
        }

        public virtual void GetDestroyed() => Destroy(gameObject);
        public virtual void GetDamaged(int damage) => GetDestroyed();

        private void OnDestroy() {
            // play sound and particles
            Debug.Log("destroy bullet");
        }
    }
}