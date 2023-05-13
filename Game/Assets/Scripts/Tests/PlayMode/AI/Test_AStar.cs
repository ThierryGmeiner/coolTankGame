using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.AI;

namespace Tests.PlayMode
{
    public class Test_AStar
    {
        [UnityTest]
        public IEnumerator Test_AStarSimplePasses() {
            yield return null;
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator GetAStarPath_Test1() {
            AStarGrid grid = TestHelper.CreateASTarGrid(5, 5);
            AStar aStar = new AStar(grid);

            AStarNode[] path = aStar.GetAStarPath(grid.Grid[0, 0].Position, grid.Grid[4, 4].Position);
            yield return null;

            AStarNode[] expectedPath = { grid.Grid[0, 0], grid.Grid[1, 1], grid.Grid[2, 2], grid.Grid[3, 3], grid.Grid[4, 4] };
            Assert.AreEqual(expectedPath, path);

            TestHelper.DestroyObjects(grid.gameObject);
        }
    }
}