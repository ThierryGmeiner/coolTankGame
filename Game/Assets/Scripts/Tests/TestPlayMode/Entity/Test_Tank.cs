using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.Entity;
using Game.Entity.Tank;
using Game.Data;

namespace Tests.PlayMode.Entity
{
    public class Test_Tank
    {
        [Test]
        public void AAA_LoadNewScene() {
            TestHelper.LoadEmptyScene();
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator Test_TankWithEnumeratorPasses() {
            yield return null;
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator SetData_SetNewValues() {
            Tank tank = TestHelper.CreateTank<Tank>();
            string newName = "TestName";
            yield return null;

            DataTank newData = ScriptableObject.CreateInstance<DataTank>();
            newData.Entity.Name = newName;
            tank.Data = newData;

            yield return null;
            Assert.AreEqual(newName, tank.Name);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator GetName_GetsName() {
            Tank tank = TestHelper.CreateTank<Tank>();
            DataTank data = ScriptableObject.CreateInstance<DataTank>();
            yield return null;

            Assert.AreEqual(data.Entity.Name, tank.Name);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator GetSpeed_GetsSpeed() {
            Tank tank = TestHelper.CreateTank<Tank>();
            DataTank data = ScriptableObject.CreateInstance<DataTank>();
            yield return null;

            Assert.AreEqual(data.Movement.Speed, tank.Movement.Speed);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator GetHealth_GetsHealth() {
            Tank tank = TestHelper.CreateTank<Tank>();
            DataTank data = ScriptableObject.CreateInstance<DataTank>();
            yield return null;

            Assert.AreEqual(data.Health.HitPoints, tank.Health.HitPoints);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator GetRigidBody_GetsRigidBody() {
            Tank tank = TestHelper.CreateTank<Tank>();
            yield return null;

            Assert.IsNotNull(tank.RigidBody);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator GetCollider_GetsCollider() {
            Tank tank = TestHelper.CreateTank<Tank>();
            yield return null;

            Assert.IsNotNull(tank.Collider);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator OnDestruction_EventFires() {
            Tank tank = TestHelper.CreateTank<Tank>();
            bool eventHasFired = false;
            yield return null;

            tank.OnDestruction += () => eventHasFired = true;
            tank.GetDestroyed();

            Assert.IsTrue(eventHasFired);

            TestHelper.DestroyObjects(tank.gameObject);
        }
    }
}