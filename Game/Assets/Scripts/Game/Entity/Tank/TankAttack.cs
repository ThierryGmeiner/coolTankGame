using UnityEngine;

namespace Game.Entity.Tank
{
    public class TankAttack : MonoBehaviour, IRangeAttack, IDropMine
    {
        private Tank tank;
        private BulletStorage bullets;

        private void Start() {
            tank = GetComponent<Tank>();
            bullets = tank.Data.BulletStorage;
        }

        public void ChangeBullet(GameObject bullet) {
            bullets.Current = bullet;
        }

        public void DropMine() {
            throw new System.NotImplementedException();
        }

        public void Shoot(Quaternion direction) {
            Shoot(GetShootingVector(direction));
        }

        public void Shoot(Vector3 direction) {
            GameObject obj = GameObject.Instantiate(bullets.Current, tank.ShootingSpot.position, tank.ShootingSpot.rotation);
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.ShootingEntity = tank.gameObject;
            bullet.Shoot(direction);
        }

        private Vector3 GetShootingVector(Quaternion rotation) {
            // https://answers.unity.com/questions/54495/how-do-i-convert-angle-to-vector3.html
            // convert the y rotation into an eulerAngle and convert it to an vector3

            float angle = rotation.eulerAngles.y;
            return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
        }
    }
}