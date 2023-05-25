using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.Entity.Tank;

namespace Tests.PlayMode.Entity
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

        [UnityTest]
        public IEnumerator Jump_IsGrounded_IncreasHeight() {
            Tank tank = TestHelper.CreateTank<Tank>();
            Transform ground = TestHelper.CreateGround<Transform>();
            tank.transform.position = new Vector3(ground.position.x, ground.position.y + (tank.transform.localScale.y / 2), ground.position.z);
            while (tank.IsGrounded == false) yield return null;

            yield return null;
            float oldHeight = tank.transform.position.y;
            tank.Movement.Jump();
            for (int i = 0; i < 20; i++) yield return null;

            Assert.Greater(tank.transform.position.y, oldHeight);

            TestHelper.DestroyObjects(tank.gameObject, ground.gameObject);
        }

        [UnityTest]
        public IEnumerator Jump_IsNotGrounded_StaySameHeight() {
            Tank tank = TestHelper.CreateTank<Tank>();
            tank.RigidBody.useGravity = false;

            yield return null;
            float oldHeight = tank.transform.position.y;
            tank.Movement.Jump();
            for (int i = 0; i < 20; i++) yield return null;

            Assert.AreEqual(oldHeight, tank.transform.position.y);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator EnableTurbo_TankWithTurboIsFaster() {
            Tank tankOne = TestHelper.CreateTank<Tank>();
            Tank tankTwo = TestHelper.CreateTank<Tank>();
            tankOne.Collider.enabled = false;
            tankTwo.Collider.enabled = false;
            tankOne.Movement.EnableTurbo();
            yield return null;

            for (int i = 0; i < 10; i++) {
                tankOne.Movement.Move(Vector2.up);
                tankTwo.Movement.Move(Vector2.up);
                yield return null;
            }

            Assert.Greater(tankOne.transform.position.z, tankTwo.transform.position.z);

            TestHelper.DestroyObjects(tankOne.gameObject, tankTwo.gameObject);
        }

        [UnityTest]
        public IEnumerator DisableTurbo_BothTanksAreEqualFast() {
            Tank tankOne = TestHelper.CreateTank<Tank>();
            Tank tankTwo = TestHelper.CreateTank<Tank>();
            tankOne.Collider.enabled = false;
            tankTwo.Collider.enabled = false;
            tankOne.Movement.EnableTurbo();
            tankOne.Movement.DisableTurbo();
            yield return null;

            for (int i = 0; i < 10; i++) {
                tankOne.Movement.Move(Vector2.up);
                tankTwo.Movement.Move(Vector2.up);
                yield return null;
            }

            Assert.AreEqual(tankOne.transform.position.z, tankTwo.transform.position.z);

            TestHelper.DestroyObjects(tankOne.gameObject, tankTwo.gameObject);
        }
    }
}