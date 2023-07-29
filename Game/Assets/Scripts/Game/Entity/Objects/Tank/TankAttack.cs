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
        private PlannedTimer attackCooldown;

        public event Action OnShoot;
        public event Action OnDropMine;
        public event Action OnReload;

        public ObjectPooling BulletPooler { get; private set; }
        private DataTank data => tank.Data;
        public int MaxShotsUntilCooldown { get; private set; } = 5;
        public int RemainingShots { get; private set; }
        public bool IsReloading { get; private set; } = false;

        private void Awake() {
            reloadTimer = gameObject.AddComponent<PlannedTimer>();
            attackCooldown = gameObject.AddComponent<PlannedTimer>();
            reloadTimer.OnTimerEnds += Reload;

            OnShoot += () => {
                RemainingShots--;
                attackCooldown.Restart();
                IsReloading = false;

                if (RemainingShots <= 0) {
                    reloadTimer.StartTimer();
                    IsReloading = true;
                }
                else reloadTimer.StopTimer();
            };
        }

        private void Start() {
            tank = GetComponent<Tank>();
            MaxShotsUntilCooldown = data.Attack.maxShootsUntilCooldown;
            RemainingShots = MaxShotsUntilCooldown;

            reloadTimer.SetupTimer(tank.Data.Attack.reloadOneBulletSeconds, Timer.Modes.restartWhenTimeIsUp, "ReloadTimer");
            attackCooldown.SetupTimer(tank.Data.Attack.cooldownAfterShotSeconds, Timer.Modes.ConitinuesWhenTimeIsUp, "AttackCooldown");
            attackCooldown.StartTimer();
            attackCooldown.ReduceTime(tank.Data.Attack.cooldownAfterShotSeconds);

            BulletPooler =
                tank.SceneData.BulletCotainer?.GetComponent<ObjectPooling>()
                ?? tank.SceneData.BulletCotainer?.AddComponent<ObjectPooling>() 
                ?? new GameObject().AddComponent<ObjectPooling>();
        }

        public void ChangeBullet(GameObject bullet) {
            bullets.Current = bullet;
        }

        private void Reload() {
            if (RemainingShots < MaxShotsUntilCooldown) {
                RemainingShots++;
                OnReload?.Invoke();
            } else {
                reloadTimer.StopTimer();
            }
        }

        public void DropMine() {
            OnDropMine?.Invoke();
            throw new System.NotImplementedException();
        }

        public Bullet Shoot(Vector3 direction) {
            if (RemainingShots <= 0 || attackCooldown.timeSec > 0) { return null; }

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