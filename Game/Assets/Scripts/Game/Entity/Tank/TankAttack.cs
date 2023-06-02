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
            //throw new System.NotImplementedException();
        }

        public void DropMine() {
            throw new System.NotImplementedException();
        }

        public Bullet Shoot(Quaternion direction) {
            return Shoot(GetShootingVector(direction));
        }

        public Bullet Shoot(Vector3 direction) {
            GameObject obj = GameObject.Instantiate(bullets.Current, tank.ShootingSpot.position, tank.ShootingSpot.rotation);
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.ShootingEntity = tank.gameObject;
            bullet.Shoot(direction);
            return bullet;
        }

        private Vector3 GetShootingVector(Quaternion rotation) {
            // convert the y rotation into an eulerAngle and convert it to an vector3
            // https://answers.unity.com/questions/54495/how-do-i-convert-angle-to-vector3.html
            float angle = rotation.eulerAngles.y;
            return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
        }
    }
}