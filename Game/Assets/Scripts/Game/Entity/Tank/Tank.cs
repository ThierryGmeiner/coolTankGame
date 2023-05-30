using UnityEngine;

namespace Game.Entity.Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class Tank : MonoBehaviour, IEntity, IDamagable, IRepairable, IRangeAttack, IDropMine
    {
        [SerializeField] private TankData data;
        [SerializeField] private GameObject tankHead;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform shootingSpot;

        private void Awake() {
            data?.BulletStorage.ManualAwake();
            groundCheck ??= CreateGroundCheck();
            tankHead ??= new GameObject();

            InstantiateData();
            Health.OnDestruction += GetDestroyed;
        }

        private void Update() { 
            IsGrounded = Movement.GroundCheck();
            Movement.Move();
        }

        // implementation IEntity
        public string Name { get => data.Name; }
        public Rigidbody RigidBody { get; private set; } = null;
        public BoxCollider Collider { get; private set; } = null;

        // implementation IDamagable, IRepairable, IRangeAttack, IDropMine
        public void GetDestroyed() => Destroy(gameObject);
        public void GetDamaged(int damage) => Health.GetDamaged(damage);
        public void GetRepaired(int healing) => Health.GetRepaired(healing);
        public void Shoot(Vector3 direction) => Attack.Shoot(direction);
        public void DropMine() => Attack.DropMine();

        // implementation intern variables
        public GameObject TankHead { get => tankHead; }
        public TankHealth Health { get; private set; } = null;
        public TankMovement Movement { get; private set; } = null;
        public TankArmor Armor { get; private set; } = null;
        public TankAttack Attack { get; private set; } = null;
        public TankData Data { get => data; set { data = value; InstantiateData(); } }
        public Transform ShootingSpot { get => shootingSpot; }
        public bool IsGrounded { get; private set; }

        private void InstantiateData() {
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>();
            data ??= ScriptableObject.CreateInstance<TankData>();
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
    }
}