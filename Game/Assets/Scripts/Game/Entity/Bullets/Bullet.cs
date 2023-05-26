using UnityEngine;
using Magic;

namespace Game.Entity
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(PlannedTimer))]
    public abstract class Bullet : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] protected new string name;
        [SerializeField] protected int damage;
        [SerializeField] private float shootingSpeed;

        [Space]
        [Header("Lifetime")]
        [SerializeField] private float lifeTime;
        [SerializeField] private Timer.Modes timerMode;
        private PlannedTimer timer;
        
        public string Name { get => name; }
        public int Damage { get => damage; }
        public Rigidbody RigidBody { get; private set; }
        public BoxCollider Collider { get; private set; }

        protected virtual void Awake() {
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>();
            timer = GetComponent<PlannedTimer>();
        }

        private void Start() {
            timer.OnTimerEnds += () => Destroy(gameObject);
            timer.SetupTimer(lifeTime, timerMode);
            timer.StartTimer();
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