using System;
using UnityEngine;

namespace Game.Entity.Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(TankHealth))]
    public class Tank : MonoBehaviour, IEntity
    {
        [SerializeField] private TankData data;
        [SerializeField] private GameObject tankHead;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform shootingSpot;
        public event Action OnDestruction;

        private void Awake() {
            InstantiateData();
            groundCheck ??= CreateGroundCheck();
            shootingSpot ??= CreateShootingSpot();
            tankHead ??= new GameObject();
            data?.BulletStorage.Awake();
        }

        private void Start() {
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>();
            Health = GetComponent<TankHealth>();
            Attack = GetComponent<TankAttack>();
            Health.OnDamaged += (int maxHP, int HP, int damage) => { if (HP <= 0) GetDestroyed(); };
        }

        private void Update() { 
            IsGrounded = Movement.GroundCheck();
            Movement.HandleMovement();
        }

        public string Name { get => data.Name; }
        public Rigidbody RigidBody { get; private set; } = null;
        public Collider Collider { get; private set; } = null;
        public void GetDestroyed() {
            OnDestruction?.Invoke();
            Destroy(gameObject);
        }

        public GameObject Head { get => tankHead; }
        public TankHealth Health { get; private set; } = null;
        public TankMovement Movement { get; private set; } = null;
        public TankArmor Armor { get; private set; } = null;
        public TankAttack Attack { get; private set; } = null;
        public TankData Data { get => data; set { data = value; InstantiateData(); } }
        public Transform ShootingSpot { get => shootingSpot; }
        public bool IsGrounded { get; private set; }

        private void InstantiateData() {
            data ??= ScriptableObject.CreateInstance<TankData>();
            data.BulletStorage ??= ScriptableObject.CreateInstance<BulletStorage>();
            Movement = new TankMovement(this, groundCheck);
            Armor = new TankArmor(this, data.ArmorProcent);
        }

        private Transform CreateGroundCheck() {
            Transform obj = Instantiate(new GameObject()).transform;
            obj.position = new Vector3(transform.position.x, transform.position.y - (transform.localScale.z / 2), transform.position.z);
            obj.parent = gameObject.transform;
            return obj.transform;
        }

        private Transform CreateShootingSpot() {
            Transform obj = Instantiate(new GameObject()).transform;
            obj.position = new Vector3(transform.position.x, transform.position.y + (transform.localScale.z / 2), transform.position.z);
            obj.parent = gameObject.transform;
            return obj.transform;
        }
    }
}