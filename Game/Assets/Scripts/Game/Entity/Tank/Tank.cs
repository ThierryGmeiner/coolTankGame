using UnityEngine;

namespace Game.Entity.Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class Tank : MonoBehaviour, IEntity, IDamagable, IRepairable
    {
        [SerializeField] private TankData data;
        [SerializeField] private GameObject tankHead;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform shootingSpot;

        private void Awake() {
            data?.BulletStorage.ManualAwake();
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>();
            groundCheck ??= CreateGroundCheck();
            tankHead ??= new GameObject();
            InstantiateData();
            Health.OnDestruction += GetDestroyed;
        }

        private void Update() { 
            IsGrounded = Movement.GroundCheck();
            Movement.Move();
        }

        public void GetDestroyed() => Destroy(gameObject);
        public void GetDamaged(int damage) => Health.GetDamaged(damage);
        public void GetRepaired(int healing) => Health.GetRepaired(healing);

        private void InstantiateData() {
            data ??= ScriptableObject.CreateInstance<TankData>();
            Name = data.Name; 
            Health = new TankHealth(this, data.Health);
            Movement = new TankMovement(this, groundCheck);
            Armor = new TankArmor(this, data.ArmorProcent);
            Attack = new TankAttack(this, data.BulletStorage);
        }

        private Transform CreateGroundCheck() {
            Transform obj = Instantiate(new GameObject()).transform;
            obj.position = new Vector3(transform.position.x, transform.position.y - (transform.localScale.z / 2), transform.position.z);
            obj.parent = gameObject.transform;
            return obj.transform;
        }

        public string Name { get; private set; }
        public GameObject TankHead { get => tankHead; }
        public TankHealth Health { get; private set; } = null;
        public TankMovement Movement { get; private set; } = null;
        public TankArmor Armor { get; private set; } = null;
        public TankAttack Attack { get; private set; } = null;
        public Rigidbody RigidBody { get; private set; } = null;
        public BoxCollider Collider { get; private set; } = null;
        public TankData Data { get => data; set { data = value; InstantiateData(); } }
        public Transform ShootingSpot { get => shootingSpot; }
        public bool IsGrounded { get; private set; }
    }
}