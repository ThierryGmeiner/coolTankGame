using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(DataEntityAttack), menuName = nameof(DataEntityAttack))]
    public class DataEntityAttack : ScriptableObject
    {
        public BulletStorage BulletStorage;
        public int maxShootsUntilCooldown = 5;

        private void Awake() {
            BulletStorage ??= CreateInstance<BulletStorage>();
        }
    }
}