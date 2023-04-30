using Magic.Data;
using UnityEngine;

namespace Game.Entity.Tank
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private TankData data;

        public string Name { get; private set; }
        public TankHealth Health { get; private set; }
        public TankMovement Movement { get; private set; }
        public TankArmor Armor { get; private set; }
        public Rigidbody RigidBody { get; private set; }
        public BoxCollider Collider { get; private set; }
        public TankData Data { get => data; set { data = value; InstantiateData(); } }

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<BoxCollider>();
            InstantiateData();
        }

        private void InstantiateData()
        {
            if (data == null) data = ScriptableObject.CreateInstance<TankData>();
            Name = data.Name;
            Health = new TankHealth(this, data.Health);
            Movement = new TankMovement(this, data.Speed);
            Armor = new TankArmor(this, data.ArmorProcent);
        }
    }
}