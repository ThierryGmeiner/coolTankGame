using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.Entity;
using Game.Entity.Interactable;

namespace Tests.PlayMode.Entity
{
    public class Test_InteractableHealth
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
        public IEnumerator SetHitPoints_SetCorrectAmount() {
            InteractableHealth health = TestHelper.CreateRepairBox<InteractableHealth>();
            yield return null;
            
            Assert.AreEqual(3, health.MaxHitPoints);
            TestHelper.DestroyObjects(health.gameObject);
        }

        [UnityTest]
        public IEnumerator GetDamaged_ReduceHP() {
            InteractableHealth health = TestHelper.CreateRepairBox<InteractableHealth>();
            yield return null;

            health.GetDamaged(10, Health.DamageType.non);

            Assert.AreEqual(health.MaxHitPoints - 1, health.HitPoints);
            TestHelper.DestroyObjects(health.gameObject);
        }

        [UnityTest]
        public IEnumerator GetDamaged_InvokeOnDamage() {
            InteractableHealth health = TestHelper.CreateRepairBox<InteractableHealth>();
            bool isDamaged = false;
            yield return null;

            health.OnDamaged += (int x, int y, int z, Vector3 w) => isDamaged = true;
            health.GetDamaged(10, Health.DamageType.non);

            Assert.IsTrue(isDamaged);
            TestHelper.DestroyObjects(health.gameObject);
        }

        [UnityTest]
        public IEnumerator GetDamaged_threeTimes_GetDestroyed() {
            InteractableHealth health = TestHelper.CreateRepairBox<InteractableHealth>();
            bool isDestroyed = false;
            yield return null;

            health.gameObject.GetComponent<IEntity>().OnDestruction += () => isDestroyed = true;
            health.GetDamaged(10, Health.DamageType.non);
            health.GetDamaged(10, Health.DamageType.non);
            health.GetDamaged(10, Health.DamageType.non);

            Assert.IsTrue(isDestroyed);
            TestHelper.DestroyObjects(health.gameObject);
        }

        [UnityTest]
        public IEnumerator GetDamaged_TypeExplosion_GetDestroyed() {
            InteractableHealth health = TestHelper.CreateRepairBox<InteractableHealth>();
            bool isDestroyed = false;
            yield return null;

            health.gameObject.GetComponent<IEntity>().OnDestruction += () => isDestroyed = true;
            health.GetDamaged(10, Health.DamageType.Explosion);

            Assert.IsTrue(isDestroyed);
            TestHelper.DestroyObjects(health.gameObject);
        }
    }
}