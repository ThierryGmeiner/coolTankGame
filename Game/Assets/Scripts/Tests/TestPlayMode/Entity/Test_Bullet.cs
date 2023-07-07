using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.Entity;
using Game.Entity.Tank;

namespace Tests.PlayMode.Entity
{
    public class Test_Bullet
    {
        [Test]
        public void AAA_LoadNewScene() {
            TestHelper.LoadEmptyScene();
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator Test_BulletWithEnumeratorPasses() {
            Assert.IsTrue(true);
            yield return null;
        }

        [UnityTest]
        public IEnumerator OnDamaged_FiresEvent() {
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            bool eventHasFired = false;
            yield return null;

            bullet.OnDamaged += (int maxHP, int hp, int damage, Vector3 direction) => eventHasFired = true;
            bullet.GetDamaged(10, Health.DamageType.non);

            Assert.IsTrue(eventHasFired);

            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator OnDamaged_FiresEvent_RightDamage() {
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            int eventFiredDamage = 0;
            int bulletDamage = 10;
            yield return null;

            bullet.OnDamaged += (int maxHP, int hp, int damage, Vector3 direction) => eventFiredDamage = damage;
            bullet.GetDamaged(bulletDamage, Health.DamageType.non);

            Assert.AreEqual(bulletDamage, eventFiredDamage);

            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator OnDamaged_FiresEvent_RightHP() {
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            int eventFiredHP = int.MaxValue;
            int bulletHP = bullet.HitPoints;
            int bulletDamage = 10;
            yield return null;

            bullet.OnDamaged += (int maxHP, int hp, int damage, Vector3 direction) => eventFiredHP = hp;
            bullet.GetDamaged(bulletDamage, Health.DamageType.non);

            Assert.AreEqual(bulletHP - bulletDamage, eventFiredHP);

            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator OnDamaged_FiresEvent_RightMaxHP() {
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            int eventFiredHP = int.MaxValue;
            int bulletDamage = 10;
            yield return null;

            bullet.OnDamaged += (int maxHP, int hp, int damage, Vector3 direction) => eventFiredHP = maxHP;
            bullet.GetDamaged(bulletDamage, Health.DamageType.non);

            Assert.AreEqual(bullet.MaxHitPoints, eventFiredHP);

            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator OnDestruction_FiresEvent() {
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            bool eventHasFired = false;
            yield return null;

            bullet.OnDestruction += () => eventHasFired = true;
            bullet.GetDestroyed();

            Assert.IsTrue(eventHasFired);

            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator OnCollisionEnter_Collides_DestroyBullet() {
            Tank tank = TestHelper.CreateTank<Tank>();
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            bool bulletIsDestroyed = false;
            bullet.OnDestruction += () => bulletIsDestroyed = true;

            bullet.transform.position = tank.transform.position;
            yield return new WaitForFixedUpdate();

            Assert.IsTrue(bulletIsDestroyed);   

            TestHelper.DestroyObjects(tank.gameObject);
            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator OnCollisionEnter_Collides_DamageEntity() {
            Tank tank = TestHelper.CreateTank<Tank>();
            Bullet bullet = TestHelper.CreateBullet<Bullet>();

            bullet.transform.position = tank.transform.position;
            yield return new WaitForFixedUpdate();

            Assert.Greater(tank.Health.MaxHitPoints, tank.Health.HitPoints);

            TestHelper.DestroyObjects(tank.gameObject);
            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator OnCollisionEnter_IsShootingEntity_DontDamageEntity() {
            Tank tank = TestHelper.CreateTank<Tank>();
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            bullet.ShootingEntity = tank.gameObject;

            bullet.transform.position = tank.transform.position;
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(tank.Health.MaxHitPoints, tank.Health.HitPoints);

            TestHelper.DestroyObjects(tank.gameObject);
            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator Shoot_AddVelocity() {
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            bullet.transform.position = TestHelper.GetEmtySpace(bullet.transform.localScale);
            bullet.GetComponent<Collider>().enabled = false;
            yield return null;

            bullet.Shoot(Vector3.forward);
            yield return new WaitForFixedUpdate();

            Assert.AreNotEqual(0, bullet.RigidBody.velocity.z);

            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator GetDamaged_FireEvent() {
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            bool eventHasFired = false;
            yield return null;

            bullet.OnDamaged += (int x, int y, int z, Vector3 w) => eventHasFired = true;
            bullet.GetDamaged(10, Health.DamageType.non);

            Assert.IsTrue(eventHasFired);

            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator GetDestroyed_FireEvent() {
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            bool eventHasFired = false;
            yield return null;

            bullet.OnDestruction += () => eventHasFired = true;
            bullet.GetDestroyed();

            Assert.IsTrue(eventHasFired);

            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }

        [UnityTest]
        public IEnumerator Lifetime_IsUp_GetDestroyed() {
            Bullet bullet = TestHelper.CreateBullet<Bullet>();
            yield return null;
            bullet.LifeTime = 0.001f;
            bool isDestroyed = false;

            bullet.OnDestruction += () => isDestroyed = true;
            yield return new WaitForFrames(5);

            Assert.IsTrue(isDestroyed);

            if (bullet != null) TestHelper.DestroyObjects(bullet.gameObject);
        }
    }
}