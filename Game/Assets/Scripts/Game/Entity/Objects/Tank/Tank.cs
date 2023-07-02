using System;
using UnityEngine;
using Game.Data;

namespace Game.Entity.Tank
{
    public class Tank : MonoBehaviour, IEntity
    {
        [SerializeField] private DataTank data;

        [SerializeField] private GameObject tankHead;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform shootingSpot;

        public event Action OnDestruction;

        private void Awake() {
            InstantiateData();
            groundCheck ??= CreateGroundCheck();
            shootingSpot ??= CreateShootingSpot();
            tankHead ??= new GameObject();
            data?.Attack?.BulletStorage.Awake();

            Health = GetComponent<TankHealth>() ?? gameObject.AddComponent<TankHealth>();
            Attack = GetComponent<TankAttack>() ?? gameObject.AddComponent<TankAttack>();
            RigidBody = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>() ?? gameObject.AddComponent<BoxCollider>();
        }

        private void Start() {
            Health.OnDamaged += (int maxHP, int HP, int damage, Vector3 direction) => { if (HP <= 0) GetDestroyed(); };
        }

        private void Update() { 
            IsGrounded = Movement.GroundCheck();
            Movement.HandleMovement();
        }

        public string Name { get => Data.Entity.Name; }
        public Rigidbody RigidBody { get; private set; }
        public Collider Collider { get; private set; }
        
        public void GetDestroyed() {
            OnDestruction?.Invoke();
            Destroy(gameObject);
        }

        public GameObject Head { get => tankHead; }
        public TankHealth Health { get; private set; } = null;
        public TankMovement Movement { get; private set; } = null;
        public TankAttack Attack { get; private set; } = null;
        public DataTank Data { get => data; set { data = value; InstantiateData(); } }
        public Transform ShootingSpot { get => shootingSpot; }
        public bool IsGrounded { get; private set; }

        private void InstantiateData() {
            data ??= ScriptableObject.CreateInstance<DataTank>();
            Movement = new TankMovement(this, groundCheck);
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