using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.Entity.Tank;

namespace Tests.PlayMode
{
    public class Test_TankMovement
    {

        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses() {
            yield return null;
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator Move_VectorZero_Stay() {
            Tank tank = TestHelper.CreateTank<Tank>();
            tank.RigidBody.useGravity = false;
            Vector3 startPos = tank.transform.position;

            for (int i = 0; i < 10; i++) {
                yield return null;
                tank.Movement.Move(Vector3.zero);
            }

            Assert.AreEqual(startPos, tank.transform.position);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator Move_VectorForward_ChangePosition() {
            Tank tank = TestHelper.CreateTank<Tank>();
            tank.RigidBody.useGravity = false;
            Vector3 startPos = tank.transform.position;

            for (int i = 0; i < 10; i++) {
                yield return null;
                tank.Movement.Move(new Vector3(10000, 0, 10000));
            }

            Assert.AreNotEqual(startPos, tank.transform.position);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest] 
        public IEnumerator GroundCheck_IsNotGrounded_False() {
            Tank tank = TestHelper.CreateTank<Tank>();
            yield return null;

            bool isGrounded = tank.Movement.GroundCheck();

            Assert.IsFalse(isGrounded);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator GroundCheck_IsGrounded_True() {
            Tank tank = TestHelper.CreateTank<Tank>();
            Transform ground = TestHelper.CreateGround<Transform>();
            tank.transform.position = new Vector3(ground.position.x, ground.position.y + (tank.transform.localScale.y / 2), ground.position.z);

            for (int i = 0; i < 5; i++) yield return null;
            bool isGrounded = tank.Movement.GroundCheck();

            Assert.IsTrue(isGrounded);

            TestHelper.DestroyObjects(tank.gameObject, ground.gameObject);
        }
    }
}