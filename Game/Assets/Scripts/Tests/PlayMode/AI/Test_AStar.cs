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
        public IEnumerator UpdateNeighbors_UpdateAllNeighbors() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            AStar aStar = new AStar(grid);
            yield return null;
            aStar.TargetNode = grid.Grid[0, 0];

            aStar.UpdateNeighbors(grid.Grid[5, 5]);

            var g = grid.Grid;
            bool allNeighborsAreUpdated = g[4, 4].gCost > 0 && g[5, 4].gCost > 0 && g[6, 4].gCost > 0 && g[4, 5].gCost > 0
                                        && g[6, 5].gCost > 0 && g[4, 6].gCost > 0 && g[5, 6].gCost > 0 && g[6, 6].gCost > 0;
            Assert.IsTrue(allNeighborsAreUpdated);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator GetAStarPath_Test1() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStar aStar = new AStar(grid);
            aStar.StartNode = grid.Grid[0, 0];
            aStar.TargetNode = grid.Grid[4, 4];

            AStarNode[] path = aStar.GetAStarPath();

            AStarNode[] expectedPath = { grid.Grid[0, 0], grid.Grid[1, 1], grid.Grid[2, 2], grid.Grid[3, 3], grid.Grid[4, 4] };
            Assert.AreEqual(expectedPath, path);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator GetAStarPath_Test2() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStar aStar = new AStar(grid);
            aStar.StartNode = grid.Grid[4, 2];
            aStar.TargetNode = grid.Grid[0, 0];

            AStarNode[] path = aStar.GetAStarPath();

            AStarNode[] expectedPath = { grid.Grid[4, 2], grid.Grid[3, 1], grid.Grid[2, 0], grid.Grid[1, 0], grid.Grid[0, 0] };
            Assert.AreEqual(expectedPath, path);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator GetAStarPath_Test3() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStar aStar = new AStar(grid);
            aStar.StartNode = grid.Grid[0, 0];
            aStar.TargetNode = grid.Grid[3, 0];
            grid.Grid[2, 0].IsWalkable = false;
            grid.Grid[2, 1].IsWalkable = false;
            grid.Grid[2, 2].IsWalkable = false;

            AStarNode[] path = aStar.GetAStarPath();

            AStarNode[] expectedPath = { grid.Grid[0, 0], grid.Grid[1, 1], grid.Grid[1, 2], grid.Grid[2, 3], grid.Grid[3, 2],
                                        grid.Grid[3, 1], grid.Grid[3, 0] };
            Assert.AreEqual(expectedPath, path);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator GetAStarPath_Test4() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStar aStar = new AStar(grid);
            aStar.StartNode = grid.Grid[0, 0];
            aStar.TargetNode = grid.Grid[3, 0];
            grid.Grid[2, 0].IsWalkable = false;
            grid.Grid[2, 1].IsWalkable = false;
            grid.Grid[2, 2].IsWalkable = false;
            grid.Grid[3, 2].IsWalkable = false;
            grid.Grid[4, 2].IsWalkable = false;
            grid.Grid[5, 2].IsWalkable = false;

            AStarNode[] path = aStar.GetAStarPath();
            
            AStarNode[] expectedPath = { grid.Grid[0, 0], grid.Grid[1, 1], grid.Grid[1, 2], grid.Grid[2, 3], grid.Grid[3, 3],
                                        grid.Grid[4, 3], grid.Grid[5, 3], grid.Grid[6, 2], grid.Grid[5, 1], grid.Grid[4, 0], grid.Grid[3, 0] };
            Assert.AreEqual(expectedPath, path);

            TestHelper.DestroyObjects(grid.gameObject);
        }
    }
}