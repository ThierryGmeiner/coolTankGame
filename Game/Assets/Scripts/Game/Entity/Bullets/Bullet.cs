using UnityEngine;
using Magic;
using System;

namespace Game.Entity
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(PlannedTimer))]
    public abstract class Bullet : MonoBehaviour, IEntity, IDamagable
    {
        [Header("Attack")]
        [SerializeField] protected new string name = "Bullet";
        [SerializeField] protected int damage = 10;
        [SerializeField] private float shootingSpeed = 20;

        [Space]
        [Header("Lifetime")]
        [SerializeField] private float lifeTime = 10;
        [SerializeField] private PlannedTimer timer;

        [Space]
        [Header("Components")]
        [SerializeField] private new BoxCollider collider;
        [SerializeField] private Rigidbody rigidBody;

        public event Action<int, int, int, Vector3> OnDamaged;
        public event Action OnDestruction;

        protected virtual void Awake() {
            rigidBody ??= GetComponent<Rigidbody>();
            collider ??= GetComponent<BoxCollider>();
            timer ??= GetComponent<PlannedTimer>();
        }

        private void Start() {
            InitializeTimer();
        }   

        protected virtual void OnCollisionEnter(Collision collision) {
            if (collision.gameObject == ShootingEntity) return;
            if (collision.gameObject.tag == Tags.Entity || collision.gameObject.tag == Tags.Player) {
                collision.gameObject.GetComponent<IDamagable>()?.GetDamaged(damage, transform.position);
            }
            GetDestroyed();
        }

        public virtual void Shoot(Vector3 direction) {
            RigidBody.AddForce(direction.normalized * shootingSpeed, ForceMode.Impulse);
        }

        public virtual void GetDamaged(int damage) => GetDamaged(damage, transform.position);

        public virtual void GetDamaged(int damage, Vector3 attackDirection) {
            OnDamaged?.Invoke(MaxHitPoints, HitPoints - damage, damage, attackDirection);
            GetDestroyed();
        }

        public virtual void GetDestroyed() {
            Debug.Log("destroy");
            OnDestruction?.Invoke();
            Destroy(gameObject);
        }

        private void InitializeTimer() {
            timer.OnTimerEnds += () => GetDestroyed();
            timer.SetupTimer(lifeTime, Timer.Modes.destroyWhenTimeIsUp);
            timer.StartTimer();
        }

        private void OnDestroy() {
            // play sound and particles
        }

        public string Name { get => name; }
        public GameObject ShootingEntity { get; set; } // the object that shoots the bullet
        public Rigidbody RigidBody { get => rigidBody; }
        public Collider Collider { get => collider; }
        public int MaxHitPoints { get; } = 0;
        public int HitPoints { get; } = 0;
        public float LifeTime {
            get => lifeTime;
            set {
                lifeTime = value;
                InitializeTimer();
            }
        }
    }
}