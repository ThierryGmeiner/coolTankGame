using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(DataTank), menuName = nameof(DataTank))]
    public class DataTank : ScriptableObject
    {
        public DataEntity Entity;
        public DataEntityHealth Health;
        public DataEntityMovement Movement;
        public DataEntityAttack Attack;

        private void Awake() {
            Entity ??= CreateInstance<DataEntity>();
            Health ??= CreateInstance<DataEntityHealth>();
            Movement ??= CreateInstance<DataEntityMovement>();
            Attack ??= CreateInstance<DataEntityAttack>();
        }
    }
}