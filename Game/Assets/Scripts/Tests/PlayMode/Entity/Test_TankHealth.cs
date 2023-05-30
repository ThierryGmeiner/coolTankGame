using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.Entity.Tank;

namespace Tests.PlayMode.Entity
{
    public class Test_TankHealth
    {
        [UnityTest]
        public IEnumerator Test_TankHealthSimplePasses() {
            yield return null;
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator TankHealth_GetHP() {
            TankHealth health = TestHelper.CreateTank<TankHealth>();
            yield return null;

            int tankHP = health.HitPoints;

            Assert.AreEqual(health.MaxHitPoints, tankHP);
        }

        [UnityTest]
        public IEnumerator TankHealth_GetMaxHP() {
            TankHealth health = TestHelper.CreateTank<TankHealth>();
            yield return null;

            int tankMaxHP = health.MaxHitPoints;

            Assert.AreEqual(health.HitPoints, tankMaxHP);
        }

        [UnityTest]
        public IEnumerator GetDamaged_LessThenHP_HPMinusDamage() {
            TankHealth health = TestHelper.CreateTank<TankHealth>();
            int damage = 10;
            yield return null;

            health.GetDamaged(damage);

            Assert.AreEqual(health.MaxHitPoints - damage, health.HitPoints);
        }

        [UnityTest]
        public IEnumerator GetDamaged_Fieres_OnDamaged() {
            TankHealth health = TestHelper.CreateTank<TankHealth>();
            int damage = 10, eventSubscribtion = 0;
            yield return null;

            health.OnDamaged += (int damage) => eventSubscribtion = damage;
            health.GetDamaged(damage);

            Assert.AreEqual(damage, eventSubscribtion);
        }

        [UnityTest]
        public IEnumerator GetDamaged_MoreThenHP_HPUnderZero() {
            TankHealth health = TestHelper.CreateTank<TankHealth>();
            int damage = 200;
            yield return null;

            health.GetDamaged(damage);

            Assert.AreEqual(health.MaxHitPoints - damage, health.HitPoints);
        }

        [UnityTest]
        public IEnumerator GetDamaged_NegativeValue_TratesAsNormal() {
            TankHealth health = TestHelper.CreateTank<TankHealth>();
            int damage = -10;
            yield return null;

            health.GetDamaged(damage);

            Assert.AreEqual(health.MaxHitPoints - Mathf.Abs(damage), health.HitPoints);
        }

        [UnityTest]
        public IEnumerator GetRepaired_RepairNotAllPoints() {
            TankHealth health = TestHelper.CreateTank<TankHealth>();
            int damage = 50, repair = 20;
            yield return null;

            health.GetDamaged(damage);
            health.GetRepaired(repair);

            Assert.AreEqual(health.MaxHitPoints - damage + repair, health.HitPoints);
        }

        [UnityTest]
        public IEnumerator GetRepaired_RepairOverMaxHealth_RepairOnlyToMaxHealth() {
            TankHealth health = TestHelper.CreateTank<TankHealth>();
            int damage = 50, repair = 70;
            int eventHasFiered = 0;
            yield return null;

            health.OnRepaired += (int repair) => eventHasFiered = repair;
            health.GetDamaged(damage);
            health.GetRepaired(repair);

            Assert.AreEqual(damage, eventHasFiered);
        }

        [UnityTest]
        public IEnumerator GetRepaired_NegativeNum_TreatesLikePositive() {
            TankHealth health = TestHelper.CreateTank<TankHealth>();
            int damage = 50, repair = -30;
            yield return null;

            health.GetDamaged(damage);
            health.GetRepaired(repair);

            Assert.AreEqual(health.MaxHitPoints - damage + System.Math.Abs(repair), health.HitPoints);
        }
    }
}