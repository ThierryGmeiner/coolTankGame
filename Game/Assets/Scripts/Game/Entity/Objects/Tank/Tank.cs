using System;
using UnityEngine;
using Game.Data;

namespace Game.Entity.Tank
{
    public class Tank : MonoBehaviour, IEntity
    {
        [SerializeField] private DataTank data;
        [Space]
        [SerializeField] private GameObject tankHead;
        [SerializeField] private Transform shootingSpot;

        public event Action OnDestruction;

        private void Awake() {
            shootingSpot ??= CreateShootingSpot();
            tankHead ??= new GameObject();

            Health = GetComponent<TankHealth>() ?? gameObject.AddComponent<TankHealth>();
            Attack = GetComponent<TankAttack>() ?? gameObject.AddComponent<TankAttack>();
            RigidBody = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>() ?? gameObject.AddComponent<BoxCollider>();
            InstantiateData();
        }

        private void Start() {
            Health.OnDamaged += (int maxHP, int HP, int damage, Vector3 direction) => { if (HP <= 0) GetDestroyed(); };
        }

        private void Update() { 
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

        private void InstantiateData() {
            data ??= ScriptableObject.CreateInstance<DataTank>();
            Movement = new TankMovement(this);
        }

        private Transform CreateShootingSpot() {
            Transform obj = Instantiate(new GameObject()).transform;
            obj.position = new Vector3(transform.position.x, transform.position.y + (transform.localScale.z / 2), transform.position.z);
            obj.parent = gameObject.transform;
            return obj.transform;
        }
    }
}