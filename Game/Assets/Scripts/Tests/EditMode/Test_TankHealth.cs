using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.Entity.Tank;

namespace Tests.EditMode
{
    public class Test_TankHealth
    {
        [Test]
        public void Test_TankHealthSimplePasses() {
            Assert.IsTrue(true);
        }

        [Test]
        public void TankHealth_GetHP() {
            TankHealth health;
            Tank tank = TestHelper.CreateTank<Tank>();
            int hp = 100;

            health = new TankHealth(tank, hp);
            int tankHP = health.HP;

            Assert.AreEqual(hp, tankHP);
        }

        [Test]
        public void TankHealth_GetMaxHP() {
            TankHealth health;
            Tank tank = TestHelper.CreateTank<Tank>();
            int hp = 100;

            health = new TankHealth(tank, hp);
            int tankMaxHP = health.MaxHP;

            Assert.AreEqual(hp, tankMaxHP);
        }

        [Test]
        public void GetDamaged_LessThenHP_HPMinusDamage() {
            TankHealth health;
            Tank tank = TestHelper.CreateTank<Tank>();
            int hp = 100, damage = 10;
            health = new TankHealth(tank, hp);

            health.GetDamaged(damage);

            Assert.AreEqual(hp - damage, health.HP);
        }

        [Test]
        public void GetDamaged_Fieres_OnDamaged() {
            TankHealth health;
            Tank tank = TestHelper.CreateTank<Tank>();
            int hp = 100, damage = 10, eventSubscribtion = 0;
            health = new TankHealth(tank, hp);

            health.OnDamaged += (int damage) => eventSubscribtion = damage;
            health.GetDamaged(damage);

            Assert.AreEqual(damage, eventSubscribtion);
        }

        [Test]
        public void GetDamaged_MoreThenHP_HPUnderZero() {
            TankHealth health;
            Tank tank = TestHelper.CreateTank<Tank>();
            int hp = 100, damage = 200;
            health = new TankHealth(tank, hp);

            health.GetDamaged(damage);

            Assert.AreEqual(hp - damage, health.HP);
        }

        [Test]
        public void GetDamaged_NegativeValue_TratesAsNormal() {
            TankHealth health;
            Tank tank = TestHelper.CreateTank<Tank>();
            int hp = 100, damage = -10;
            health = new TankHealth(tank, hp);

            health.GetDamaged(damage);

            Assert.AreEqual(90, health.HP);
        }

        [Test]
        public void GetDamaged_MoreThenHP_FieresOnDestruction() {
            TankHealth health;
            Tank tank = TestHelper.CreateTank<Tank>();
            int hp = 100, damage = 200;
            health = new TankHealth(tank, hp);
            bool eventHasFiered = false;

            health.OnDestruction += () => eventHasFiered = true;
            health.GetDamaged(damage);

            Assert.IsTrue(eventHasFiered);
        }

        [Test]
        public void GetRepaired_RepairNotAllPoints() {
            TankHealth health;
            Tank tank = TestHelper.CreateTank<Tank>();
            int hp = 100, damage = 50, repair = 20;
            health = new TankHealth(tank, hp);

            health.GetDamaged(damage);
            health.GetRepaired(repair);

            Assert.AreEqual(hp - damage + repair, health.HP);
        }

        [Test]
        public void GetRepaired_RepairOverMaxHealth_RepairOnlyToMaxHealth() {
            TankHealth health;
            Tank tank = TestHelper.CreateTank<Tank>();
            int hp = 100, damage = 50, repair = 70;
            health = new TankHealth(tank, hp);
            int eventHasFiered = 0;

            health.OnRepaired += (int repair) => eventHasFiered = repair;
            health.GetDamaged(damage);
            health.GetRepaired(repair);

            Assert.AreEqual(damage, eventHasFiered);
        }

        [Test]
        public void GetRepaired_NegativeNum_TreatesLikePositive() {
            TankHealth health;
            Tank tank = TestHelper.CreateTank<Tank>();
            int hp = 100, damage = 50, repair = -30;
            health = new TankHealth(tank, hp);

            health.GetDamaged(damage);
            health.GetRepaired(repair);

            Assert.AreEqual(hp - damage + System.Math.Abs(repair), health.HP);
        }
   }
}