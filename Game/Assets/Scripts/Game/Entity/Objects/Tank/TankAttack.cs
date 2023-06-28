using System;
using UnityEngine;
using Magic;

namespace Game.Entity.Tank
{
    public class TankAttack : MonoBehaviour, IRangeAttack, IDropMine
    {
        private Tank tank;
        private BulletStorage bullets;
        private PlannedTimer reloadTimer;
        private PlannedTimer cooldownTimer;
        private float reloadOneBulletSeconds = 2;
        private float cooldownAfterShotSeconds = 1;

        [SerializeField] private TankData data;

        public event Action OnShoot;
        public event Action OnDropMine;
        public event Action OnReload;
        public event Action OnUpdateShotsUntilCooldown;

        public int MaxShotsUntilCooldown { get; private set; } = 5;
        public int remainingShots { get; private set; }
        public TankData Data { get => data; set => data = value; }

        private void Awake() {
            data ??= ScriptableObject.CreateInstance<TankData>();
            MaxShotsUntilCooldown = data.maxShootsUntilCooldown;
            remainingShots = MaxShotsUntilCooldown;

            reloadTimer = gameObject.AddComponent<PlannedTimer>();
            reloadTimer.SetupTimer(reloadOneBulletSeconds, Timer.Modes.restartWhenTimeIsUp);
            reloadTimer.StartTimer();

            cooldownTimer = gameObject.AddComponent<PlannedTimer>();
            cooldownTimer.SetupTimer(cooldownAfterShotSeconds, Timer.Modes.ConitinuesWhenTimeIsUp);
            cooldownTimer.StartTimer();

            reloadTimer.OnTimerEnds += Reload;
            OnShoot += () => cooldownTimer.Restart();
            OnShoot += () => remainingShots--;
            OnShoot += () => reloadTimer.Restart();
        }

        private void Start() {
            tank = GetComponent<Tank>();
            bullets = data.BulletStorage ?? ScriptableObject.CreateInstance<BulletStorage>();
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
            GameObject obj = GameObject.Instantiate(bullets.Current, tank.ShootingSpot.position, tank.ShootingSpot.rotation);
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.ShootingEntity = tank.gameObject;
            return bullet;
        }
    }
}