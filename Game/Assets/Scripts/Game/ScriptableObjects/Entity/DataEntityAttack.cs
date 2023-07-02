using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(DataEntityAttack), menuName = nameof(DataEntityAttack))]
    public class DataEntityAttack : ScriptableObject
    {
        public BulletStorage BulletStorage;
        public int maxShootsUntilCooldown = 5;
        public float reloadOneBulletSeconds = 2;
        public float cooldownAfterShotSeconds = 0.6f;

        private void Awake() {
            BulletStorage ??= CreateInstance<BulletStorage>();
        }
    }
}