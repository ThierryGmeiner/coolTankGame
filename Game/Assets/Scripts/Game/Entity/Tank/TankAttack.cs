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

        public void ChangeAttack(int index) {
            bullets.SetCurrentBullet(index);
        }

        public void Attack(Vector2 direction) {

        }
    }
}