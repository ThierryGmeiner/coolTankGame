using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using Game.Entity.Tank;
using Game.AI;
using Magic;

namespace Tests.PlayMode.Entity
{
    public class Test_TankMovement
    {
        [Test]
        public void AAA_LoadNewScene() {
            TestHelper.LoadEmptyScene();
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses() {
            yield return null;
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator Move_VectorForward_ChangePosition() {
            Tank tank = TestHelper.CreateTank<Tank>();
            yield return null;
            tank.Movement.SetPath(tank.transform.position, new Vector3(15, 0, 15));
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
        public IEnumerator SetPath_SetNewPath() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            Tank tank = TestHelper.CreateTank<Tank>();
            yield return null;

            tank.Movement.SetPath(tank.transform.position, grid.Grid[0, 0].Position);

            Assert.IsNotNull(tank.Movement.Path);

            TestHelper.DestroyObjects(tank.gameObject, grid.gameObject);
        }

        [UnityTest]
        public IEnumerator SetPath_PathLengthIsZero_SetNoPath() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            Tank tank = TestHelper.CreateTank<Tank>();
            yield return null;
            grid.Grid[0, 0].IsWalkable = false; // findPath return emty path when target/start isnt walkable

            Path oldPath = tank.Movement.SetPath(grid.Grid[2, 2].Position, grid.Grid[1, 1].Position);
            Path newPath = tank.Movement.SetPath(grid.Grid[0, 0].Position, grid.Grid[1, 1].Position);

            Assert.AreEqual(oldPath.Nodes.Length, newPath.Nodes.Length);

            TestHelper.DestroyObjects(tank.gameObject, grid.gameObject);
        }

        [UnityTest]
        public IEnumerator Move_PathIsNull_DoNothing() {
            Tank tank = TestHelper.CreateTank<Tank>();
            Vector3 oldPos = tank.transform.position;
            yield return null;

            tank.Movement.Path = null;
            tank.Movement.HandleMovement();

            Assert.AreEqual(oldPos.x, tank.transform.position.x, 0.02f);
            Assert.AreEqual(oldPos.z, tank.transform.position.z, 0.02f);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator Move_PathLengthIsZero_DoNothing() {
            Tank tank = TestHelper.CreateTank<Tank>();
            Vector3 oldPos = tank.transform.position;
            yield return null;

            tank.Movement.Path = new Path(new AStarNode[0]);
            tank.Movement.HandleMovement();

            Assert.AreEqual(oldPos.x, tank.transform.position.x, 0.05f);

            TestHelper.DestroyObjects(tank.gameObject);
        }

        [UnityTest]
        public IEnumerator Move_PathIsOptimized_PathStaySame() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            Tank tank = TestHelper.CreateTank<Tank>();
            yield return null;

            Path unoptimizedPath = tank.Movement.SetPath(grid.Grid[0, 0].Position, grid.Grid[5, 5].Position);
            Path optimizedPath = tank.Movement.aStar.FindOptimizedPath(unoptimizedPath);
            tank.Movement.HandleMovement();

            Assert.AreEqual(optimizedPath.Nodes.Length, tank.Movement.Path.Nodes.Length);

            TestHelper.DestroyObjects(tank.gameObject, grid.gameObject);
        }
    }
}