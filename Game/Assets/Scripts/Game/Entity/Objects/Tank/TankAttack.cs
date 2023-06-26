using System;
using UnityEngine;
using Magic;

namespace Game.Entity.Tank
{
    public class TankAttack : MonoBehaviour, IRangeAttack, IDropMine
    {
        private Tank tank;
        private BulletStorage bullets;
        private PlannedTimer cooldownTimer;
        private float timeToReloadOndeBulletInSec = 2;

        [SerializeField] private TankData data;

        public event Action OnShoot;
        public event Action OnDropMine;
        public event Action OnReload;
        public event Action OnUpdateShotsUntilCooldown;

        public int MaxShotsUntilCooldown { get; private set; } = 5;
        public int ShotsUntilCooldown { get; private set; }
        public TankData Data { get => data; set => data = value; }

        private void Awake() {
            data ??= ScriptableObject.CreateInstance<TankData>();
            MaxShotsUntilCooldown = data.maxShootsUntilCooldown;
            ShotsUntilCooldown = MaxShotsUntilCooldown;

            cooldownTimer = gameObject.AddComponent<PlannedTimer>();
            cooldownTimer.SetupTimer(timeToReloadOndeBulletInSec, Timer.Modes.restartWhenTimeIsUp);
            cooldownTimer.OnTimerEnds += Reload;
            cooldownTimer.StartTimer();

            OnShoot += () => ShotsUntilCooldown--;
            OnShoot += () => cooldownTimer.Restart();
        }

        private void Start() {
            tank = GetComponent<Tank>();
            bullets = data.BulletStorage ?? ScriptableObject.CreateInstance<BulletStorage>();
        }

        public void ChangeBullet(GameObject bullet) {
            bullets.Current = bullet;
        }

        private void Reload() {
            if (ShotsUntilCooldown < MaxShotsUntilCooldown) {
                ShotsUntilCooldown++;
                OnReload?.Invoke();
                OnUpdateShotsUntilCooldown?.Invoke();
            }
        }

        public void DropMine() {
            OnDropMine?.Invoke();
            throw new System.NotImplementedException();
        }

        public Bullet Shoot(Vector3 direction) {
            if (ShotsUntilCooldown <= 0) return null;

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