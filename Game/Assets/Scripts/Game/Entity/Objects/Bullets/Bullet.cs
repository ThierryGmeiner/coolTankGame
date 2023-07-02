using UnityEngine;
using Magic;
using System;

namespace Game.Entity
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(PlannedTimer))]
    public abstract class Bullet : MonoBehaviour, IEntity, IDamagable, IPoolable
    {
        [Header("Attack")]
        [SerializeField] protected new string name = "Bullet";
        [SerializeField] protected int damage = 10;
        [SerializeField] private float shootingSpeed = 20;

        [Space]
        [Header("Lifetime")]
        [SerializeField] private float lifeTime = 8;
        private PlannedTimer timer;

        public event Action<int, int, int, Vector3> OnDamaged;
        public event Action OnDestruction;

        protected virtual void Awake() {
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>();
            timer = gameObject.AddComponent<PlannedTimer>();
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
            OnDestruction?.Invoke();
            SetInactive();
        }

        private void InitializeTimer() {
            timer.OnTimerEnds += () => GetDestroyed();
            timer.SetupTimer(lifeTime, Timer.Modes.destroyWhenTimeIsUp);
            timer.StartTimer();
        }

        public void SetActive() {
            Collider.enabled = true;
            RigidBody.isKinematic = false;
        }

        public void SetInactive() {
            Collider.enabled = false;
            RigidBody.velocity = Vector3.zero;
            RigidBody.isKinematic = true;
            transform.position = new Vector3(-210, -210, -210);
        }

        private void OnDestroy() {
            // play sound and particles
        }

        public string Name { get => name; }
        public GameObject GameObject { get => gameObject; }
        public ObjectPooling Container { get; set; }
        public GameObject ShootingEntity { get; set; } // the object that shoots the bullet
        public Rigidbody RigidBody { get; set; }
        public Collider Collider { get; set; }
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