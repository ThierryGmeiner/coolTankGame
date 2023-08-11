using UnityEngine;
using Magic;
using System;

namespace Game.Entity
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(PlannedTimer))]
    public abstract class Bullet : MonoBehaviour, IEntity, IDamagable, IPoolable, IDamaging
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
            InitializeLifeTime();
        }

        protected virtual void OnCollisionEnter(Collision collision) {
            if (collision.gameObject == ShootingEntity) { return; }

            if (collision.gameObject.tag == Tags.Entity || collision.gameObject.tag == Tags.Player) {
                collision.gameObject.GetComponent<IDamagable>()?.GetDamaged(damage, Health.DamageType.Shot, transform.position);
            }
            GetDestroyed();
        }

        public virtual void Shoot(GameObject shootingEntity, Transform shootingSpot, Vector3 direction) {
            transform.position = shootingSpot.position;
            transform.rotation = shootingSpot.rotation;
            ShootingEntity = shootingEntity;
            RigidBody.AddForce(direction.normalized * shootingSpeed, ForceMode.Impulse);
        }

        public virtual void GetDamaged(int damage, Health.DamageType damageType) => GetDamaged(damage, damageType, transform.position);

        public virtual void GetDamaged(int damage, Health.DamageType damageType, Vector3 attackDirection) {
            OnDamaged?.Invoke(MaxHitPoints, HitPoints - damage, damage, attackDirection);
            GetDestroyed();
        }

        public virtual void GetDestroyed() {
            OnDestruction?.Invoke();
            SetInactive();
        }

        private void InitializeLifeTime() {
            timer.OnTimerEnds += GetDestroyed;
            timer.SetupTimer(lifeTime, Timer.Modes.destroyWhenTimeIsUp, "LifeTime");
            timer.StartTimer();
        }

        public void SetActive() {
            Collider.enabled = true;
            RigidBody.isKinematic = false;
            InitializeLifeTime();
        }

        public void SetInactive() {
            Collider.enabled = false;
            RigidBody.velocity = Vector3.zero;
            RigidBody.isKinematic = true;
            transform.position = new Vector3(-210, -210, -210);
        }

        public string Name { get => name; }
        public GameObject GameObject { get => gameObject; }
        public ObjectPooling ObjectPooler { get; set; }
        public GameObject ShootingEntity { get; protected set; } // the object that shoots the bullet
        public Rigidbody RigidBody { get; set; }
        public Collider Collider { get; set; }
        public Health.DamageType DamageType { get => Health.DamageType.Shot; }
        public int MaxHitPoints { get; } = 0;
        public int HitPoints { get; } = 0;
        public bool HasFullHP => HitPoints >= MaxHitPoints;
        public float LifeTime {
            get => lifeTime;
            set {
                lifeTime = value;
                InitializeLifeTime();
            }
        }
    }
}