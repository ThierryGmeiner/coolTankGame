using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.AI;

namespace Tests.PlayMode
{
    public class Test_EnemyAI
    {
        [Test]
        public void AAA_LoadNewScene() {
            TestHelper.LoadEmptyScene();
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator Test_EnemyAIWithEnumeratorPasses() {
            Assert.IsTrue(true);
            yield return null;
        }

        [UnityTest]
        public IEnumerator CanSeeTarget_InRange_True() {
            TankAI ai = TestHelper.CreateEnemyTank<TankAI>();
            GameObject player = TestHelper.CreatePlayerTank();
            yield return null;

            ai.transform.position = Vector3.zero;
            player.transform.position = new Vector3(0, 0, ai.ViewRadius / 2);
            bool canSeePlayer = ai.CanSeeTarget(ai.Tank.transform);

            Assert.IsTrue(canSeePlayer);
            TestHelper.DestroyObjects(ai.gameObject, player.gameObject);
        }

        [UnityTest]
        public IEnumerator CanSeeTarget_InExtendedRange_True() {
            TankAI ai = TestHelper.CreateEnemyTank<TankAI>();
            GameObject player = TestHelper.CreatePlayerTank();
            yield return null;

            ai.transform.position = Vector3.zero;
            player.transform.position = new Vector3(0, 0, ai.ViewRadiusExtended - 1);
            bool canSeePlayer = ai.CanSeeTarget(ai.Tank.transform);

            Assert.IsTrue(canSeePlayer);
            TestHelper.DestroyObjects(ai.gameObject, player.gameObject);
        }

        [UnityTest]
        public IEnumerator CanSeeTarget_InExtendedRangeButBehinde_False() {
            TankAI ai = TestHelper.CreateEnemyTank<TankAI>();
            GameObject player = TestHelper.CreatePlayerTank();
            yield return null;

            ai.transform.position = Vector3.zero;
            player.transform.position = new Vector3(0, 0, -ai.ViewRadiusExtended + 1);
            bool canSeePlayer = ai.CanSeeTarget(ai.Tank.transform);

            Assert.IsFalse(canSeePlayer);
            TestHelper.DestroyObjects(ai.gameObject, player.gameObject);
        }

        [UnityTest]
        public IEnumerator CanSeeTarget_OutOfRange_False() {
            TankAI ai = TestHelper.CreateEnemyTank<TankAI>();
            GameObject player = TestHelper.CreatePlayerTank();
            yield return null;

            ai.transform.position = Vector3.zero;
            player.transform.position = Vector3.forward * 100;
            bool canSeePlayer = ai.CanSeeTarget(ai.Tank.transform);

            Assert.IsFalse(canSeePlayer);
            TestHelper.DestroyObjects(ai.gameObject, player.gameObject);
        }
    }
}