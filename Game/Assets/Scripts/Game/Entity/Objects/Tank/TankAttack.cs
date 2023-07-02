using System;
using UnityEngine;
using Magic;
using Game.Data;

namespace Game.Entity.Tank
{
    public class TankAttack : MonoBehaviour, IRangeAttack, IDropMine
    {
        private Tank tank;

        private BulletStorage bullets;
        private PlannedTimer reloadTimer;
        private PlannedTimer cooldownTimer;
        private float reloadOneBulletSeconds = 2;
        private float cooldownAfterShotSeconds = 0.6f;

        [SerializeField] private GameObject bulletContainer;

        public event Action OnShoot;
        public event Action OnDropMine;
        public event Action OnReload;
        public event Action OnUpdateShotsUntilCooldown;

        public ObjectPooling BulletPooler { get; private set; }
        private DataTank data => tank.Data;
        public int MaxShotsUntilCooldown { get; private set; } = 5;
        public int remainingShots { get; private set; }

        private void Awake() {
            reloadTimer = gameObject.AddComponent<PlannedTimer>();
            reloadTimer.SetupTimer(reloadOneBulletSeconds, Timer.Modes.restartWhenTimeIsUp);
            reloadTimer.StartTimer();

            cooldownTimer = gameObject.AddComponent<PlannedTimer>();
            cooldownTimer.SetupTimer(cooldownAfterShotSeconds, Timer.Modes.ConitinuesWhenTimeIsUp);
            cooldownTimer.StartTimer();
            cooldownTimer.ReduceTime(cooldownAfterShotSeconds);

            reloadTimer.OnTimerEnds += Reload;
            OnShoot += () => cooldownTimer.Restart();
            OnShoot += () => remainingShots--;
            OnShoot += () => reloadTimer.Restart();
        }

        private void Start() {
            tank = GetComponent<Tank>();
            MaxShotsUntilCooldown = data.Attack.maxShootsUntilCooldown;
            remainingShots = MaxShotsUntilCooldown;

            tank = GetComponent<Tank>();
            bullets = data.Attack.BulletStorage ?? ScriptableObject.CreateInstance<BulletStorage>();

            bulletContainer ??= new GameObject();
            BulletPooler = bulletContainer.GetComponent<ObjectPooling>() ?? bulletContainer.AddComponent<ObjectPooling>();
        }

        public void ChangeBullet(GameObject bullet) {
            bullets.Current = bullet;
        }

        private void Reload() {
            if (remainingShots < MaxShotsUntilCooldown) {
                remainingShots++;
                OnReload?.Invoke();
                OnUpdateShotsUntilCooldown?.Invoke();
            }
        }

        public void DropMine() {
            OnDropMine?.Invoke();
            throw new System.NotImplementedException();
        }

        public Bullet Shoot(Vector3 direction) {
            if (remainingShots <= 0 || cooldownTimer.timeSec > 0) return null;

            Bullet bullet = InstantiateBullet();
            bullet.Shoot(direction);
            OnShoot?.Invoke();
            OnUpdateShotsUntilCooldown?.Invoke();
            return bullet;
        }

        private Bullet InstantiateBullet() {
            GameObject obj = BulletPooler.RequestObject();
            obj.transform.position = tank.ShootingSpot.position;
            obj.transform.rotation = tank.ShootingSpot.rotation;
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.ShootingEntity = tank.gameObject;
            return bullet;
        }
    }
}