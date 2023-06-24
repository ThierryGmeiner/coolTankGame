using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.Entity;
using Game.Entity.Tank;

namespace Tests.PlayMode.Entity
{
    public class Test_TankAttack
    {
        [Test]
        public void AAA_LoadNewScene() {
            TestHelper.LoadEmptyScene();
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator Test_TankAttackWithEnumeratorPasses() {
            Assert.IsTrue(true);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Shoot_CreateBullet() {
            TankAttack tank = TestHelper.CreateTank<TankAttack>();
            yield return null;
            GameObject bullet = TestHelper.CreateBullet();

            tank.ChangeBullet(bullet);
            Bullet newBullet = tank.Shoot(new Vector3(10, 10, 10));

            Assert.IsNotNull(newBullet);

            TestHelper.DestroyObjects(tank.gameObject, bullet, newBullet.gameObject);
        }

        [UnityTest]
        public IEnumerator Shoot_SetShootingEntity() {
            TankAttack tank = TestHelper.CreateTank<TankAttack>();
            yield return null;
            GameObject bullet = TestHelper.CreateBullet();

            tank.ChangeBullet(bullet);
            Bullet newBullet = tank.Shoot(new Vector3(10, 10, 10));

            Assert.AreEqual(tank.gameObject, newBullet.ShootingEntity);

            TestHelper.DestroyObjects(tank.gameObject, bullet, newBullet.gameObject);
        }

        [UnityTest]
        public IEnumerator Shoot_OneShotUntilCooldownLess() {
            TankAttack tank = TestHelper.CreateTank<TankAttack>();
            yield return null;
            GameObject obj = TestHelper.CreateBullet();
            tank.ChangeBullet(obj);

            Bullet bullet = tank.Shoot(new Vector3(10, 10, 10));

            Assert.AreEqual(tank.MaxShotsUntilCooldown - 1, tank.ShotsUntilCooldown);

            TestHelper.DestroyObjects(tank.gameObject, obj.gameObject, bullet.gameObject); ;
        }
    }
}