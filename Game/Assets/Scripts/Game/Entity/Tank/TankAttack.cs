using UnityEngine;

namespace Game.Entity.Tank
{
    public class TankAttack
    {
        private readonly Tank tank;
        private BulletStorage bullets;

        public TankAttack(Tank tank, BulletStorage bullets) {
            this.tank = tank;
            this.bullets = bullets;
        }

        public void ChangeBullet(GameObject bullet) {
            bullets.Current = bullet;
        }

        public void DropMine() {
            throw new System.NotImplementedException();
        }

        public void Shoot(Vector3 direction) {
            Bullet bullet 
                = GameObject.Instantiate(bullets.Current, tank.ShootingSpot.position, tank.ShootingSpot.rotation).GetComponent<Bullet>();
            bullet.ShootingEntity = tank.gameObject;
            bullet.Shoot(direction);
        }
    }
}