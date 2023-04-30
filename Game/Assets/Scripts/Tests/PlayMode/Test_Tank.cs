using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.Entity.Tank;
using Magic.Data;

namespace Tests.PlayMode
{
    public class Test_Tank
    {
        [UnityTest]
        public IEnumerator Test_TankWithEnumeratorPasses()
        {
            yield return null;
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator SetData_SetNewValues()
        {
            Tank tank = TestHelper.CreateTank<Tank>();
            string newName = "TestName";
            yield return null;

            TankData newData = ScriptableObject.CreateInstance<TankData>();
            newData.Name = newName;
            tank.Data = newData;

            yield return null;
            Assert.AreEqual(newName, tank.Name);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator GetName_GetsName()
        {
            Tank tank = TestHelper.CreateTank<Tank>();
            TankData data = ScriptableObject.CreateInstance<TankData>();
            yield return null;

            Assert.AreEqual(data.Name, tank.Name);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator GetSpeed_GetsSpeed()
        {
            Tank tank = TestHelper.CreateTank<Tank>();
            TankData data = ScriptableObject.CreateInstance<TankData>();
            yield return null;

            Assert.AreEqual(data.Speed, tank.Movement.Speed);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator GetHealth_GetsHealth()
        {
            Tank tank = TestHelper.CreateTank<Tank>();
            TankData data = ScriptableObject.CreateInstance<TankData>();
            yield return null;

            Assert.AreEqual(data.Health, tank.Health.HP);

            TestHelper.DestroyObjects(tank.gameObject);
        }
    
        [UnityTest]
        public IEnumerator GetArmor_GetsArmor()
        {
            Tank tank = TestHelper.CreateTank<Tank>();
            TankData data = ScriptableObject.CreateInstance<TankData>();
            yield return null;

            Assert.AreEqual(data.ArmorProcent, tank.Armor.ArmorProcent);

            TestHelper.DestroyObjects(tank.gameObject);
        }
    
        [UnityTest]
        public IEnumerator GetRigidBody_GetsRigidBody()
        {
            Tank tank = TestHelper.CreateTank<Tank>();
            yield return null;

            Assert.IsNotNull(tank.RigidBody);

            TestHelper.DestroyObjects(tank.gameObject);
        }
    
        [UnityTest]
        public IEnumerator GetCollider_GetsCollider()
        {
            Tank tank = TestHelper.CreateTank<Tank>();
            yield return null;

            Assert.IsNotNull(tank.Collider);

            TestHelper.DestroyObjects(tank.gameObject);
        }
    }
}