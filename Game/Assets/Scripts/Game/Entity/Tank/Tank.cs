using Magic.Data;
using UnityEngine;

namespace Game.Entity.Tank
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private TankData data;
        [SerializeField] private Transform groundCheck;

        public string Name { get; private set; }
        public TankHealth Health { get; private set; }
        public TankMovement Movement { get; private set; }
        public TankArmor Armor { get; private set; }
        public Rigidbody RigidBody { get; private set; }
        public BoxCollider Collider { get; private set; }
        public TankData Data { get => data; set { data = value; InstantiateData(); } }
        public bool IsGrounded { get; private set; }

        private void Awake() {
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>();
            if (groundCheck == null) groundCheck = CreateGroundCheck();
            InstantiateData();
        }

        private void Update() {
            IsGrounded = Movement.GroundCheck();
            Movement.Move();
        }

        private void InstantiateData() {
            if (data == null) data = ScriptableObject.CreateInstance<TankData>();
            Name = data.Name;
            Health = new TankHealth(this, data.Health);
            Movement = new TankMovement(this, groundCheck);
            Armor = new TankArmor(this, data.ArmorProcent);
        }

        private Transform CreateGroundCheck() {
            Transform obj = Instantiate(new GameObject()).transform;
            obj.position = new Vector3(transform.position.x, transform.position.y - (transform.localScale.z / 2), transform.position.z);
            obj.parent = gameObject.transform;
            return obj.transform;
        }
    }
}