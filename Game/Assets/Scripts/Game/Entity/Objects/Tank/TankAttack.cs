using System;
using UnityEngine;
using Magic;
using Game.Data;

namespace Game.Entity.Tank
{
    public class TankAttack : MonoBehaviour, IRangeAttack, IDropMine
    {
        private Tank tank;
        private PlannedTimer reloadTimer;
        private PlannedTimer attackCooldown;
        private UI.ScrolingWheel inventory;

        public event Action OnShoot;
        public event Action OnDropMine;
        public event Action OnReload;

        // nullReference by enemy because they dont have a scrolingwhele
        // i have to overwork this system
        // i think the bulletstorage isnt a god idea

        public ObjectPooling ActiveBulletPooler { get; private set; }
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

            inventory = gameObject.GetComponentInChildren<UI.ScrolingWheel>(includeInactive: true);
            inventory.OnSelectItem += (Item item) => ChangeBullet(item);

            reloadTimer.SetupTimer(tank.Data.Attack.reloadOneBulletSeconds, Timer.Modes.restartWhenTimeIsUp, "ReloadTimer");
            attackCooldown.SetupTimer(tank.Data.Attack.cooldownAfterShotSeconds, Timer.Modes.ConitinuesWhenTimeIsUp, "AttackCooldown");
            attackCooldown.StartTimer();
            attackCooldown.ReduceTime(tank.Data.Attack.cooldownAfterShotSeconds);

            ActiveBulletPooler =
                tank.SceneData.BulletCotainer?.GetComponent<ObjectPooling>()
                ?? tank.SceneData.BulletCotainer?.AddComponent<ObjectPooling>() 
                ?? new GameObject().AddComponent<ObjectPooling>();
        }

        public void ChangeBullet(Item item) {
            //ActiveBulletPooler = item.objectPool;
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

            Bullet bullet = ActiveBulletPooler.RequestObject().GetComponent<Bullet>();
            bullet.Shoot(tank.gameObject, tank.ShootingSpot.transform, direction);
            OnShoot?.Invoke();
            return bullet;
        }
    }
}