using System;
using UnityEngine;
using Magic;
using Game.Data;

namespace Game.Entity.Tank
{
    public class TankAttack : MonoBehaviour, IRangeAttack, IDropMine
    {
        [SerializeField] private GameObject bulletContainer;

        private Tank tank;
        private BulletStorage bullets;
        private PlannedTimer reloadTimer;
        private PlannedTimer cooldownTimer;

        public event Action OnShoot;
        public event Action OnDropMine;
        public event Action OnReload;

        public ObjectPooling BulletPooler { get; private set; }
        private DataTank data => tank.Data;
        public int MaxShotsUntilCooldown { get; private set; } = 5;
        public int RemainingShots { get; private set; }

        private void Awake() {
            reloadTimer = gameObject.AddComponent<PlannedTimer>();
            cooldownTimer = gameObject.AddComponent<PlannedTimer>();

            reloadTimer.OnTimerEnds += Reload;

            OnShoot += () => {
                RemainingShots--;
                cooldownTimer.Restart();

                if (RemainingShots <= 0) reloadTimer.StartTimer();
                else reloadTimer.StopTimer();
            };
        }

        private void Start() {
            tank = GetComponent<Tank>();
            MaxShotsUntilCooldown = data.Attack.maxShootsUntilCooldown;
            RemainingShots = MaxShotsUntilCooldown;

            reloadTimer.SetupTimer(tank.Data.Attack.reloadOneBulletSeconds, Timer.Modes.restartWhenTimeIsUp);
            cooldownTimer.SetupTimer(tank.Data.Attack.cooldownAfterShotSeconds, Timer.Modes.ConitinuesWhenTimeIsUp);
            cooldownTimer.StartTimer();
            cooldownTimer.ReduceTime(tank.Data.Attack.cooldownAfterShotSeconds);

            bulletContainer ??= new GameObject();
            BulletPooler = bulletContainer.GetComponent<ObjectPooling>() ?? bulletContainer.AddComponent<ObjectPooling>();
        }

        public void ChangeBullet(GameObject bullet) {
            bullets.Current = bullet;
        }

        private void Reload() {
            if (RemainingShots < MaxShotsUntilCooldown) {
                RemainingShots++;
                OnReload?.Invoke();
            } 
            else {
                reloadTimer.StopTimer();
            }
        }

        public void DropMine() {
            OnDropMine?.Invoke();
            throw new System.NotImplementedException();
        }

        public Bullet Shoot(Vector3 direction) {
            if (RemainingShots <= 0 || cooldownTimer.timeSec > 0) return null;

            Bullet bullet = InstantiateBullet();
            bullet.Shoot(direction);
            OnShoot?.Invoke();
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