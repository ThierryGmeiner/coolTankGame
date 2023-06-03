using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.AI;

namespace Tests.PlayMode.AI
{
    public class Test_AStarGrid
    {
        [Test]
        public void AAA_LoadNewScene() {
            TestHelper.LoadEmptyScene();
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator CreateGrid_GetRightAmountOfNodes() {
            AStarGrid Astar = TestHelper.CreateASTarGrid();
            int expectedLength = 10; // is hardcoden in class as default

            yield return null;

            Assert.AreEqual(expectedLength, Astar.Grid.GetLength(0));

            TestHelper.DestroyObjects(Astar.gameObject);
        }

        [UnityTest]
        public IEnumerator CreateGrid_HasNoObstacle_AllNotesAreWalkable() {
            AStarGrid Astar = TestHelper.CreateASTarGrid();
            bool allNodesAreWalkable = true;
            yield return null;

            foreach (AStarNode node in Astar.Grid) {
                if (!node.IsWalkable) {
                    allNodesAreWalkable = false;
                    break;
                }
            }

            Assert.IsTrue(allNodesAreWalkable);

            TestHelper.DestroyObjects(Astar.gameObject);
        }

        //[UnityTest]
        //public IEnumerator CreateGrid_HasObstacle_NotAllNotesAreWalkable() {
        //    GameObject obstacle = TestHelper.CreateObstacle(new Vector3(5, 5, 5));
        //    AStarGrid Astar = TestHelper.CreateASTarGrid();
        //    bool allNodesAreWalkable = true;
        //    yield return null; 

        //    foreach (AStarNode node in Astar.Grid) {
        //        if (!node.IsWalkable) {
        //            allNodesAreWalkable = false;
        //            break;
        //        }
        //    }

        //    Assert.IsFalse(allNodesAreWalkable);

        //    TestHelper.DestroyObjects(Astar.gameObject, obstacle);
        //}

        [UnityTest]
        public IEnumerator GetNodeFromPosition_BottomLeft() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;

            Vector3 searchPos = grid.Grid[0, 0].Position;
            AStarNode node = grid.GetNodeFromPosition(searchPos);

            Assert.AreEqual(searchPos, node.Position);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator GetNodeFromPosition_BottomRight() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;

            Vector3 searchPos = grid.Grid[2, 0].Position;
            AStarNode node = grid.GetNodeFromPosition(searchPos);

            Assert.AreEqual(searchPos, node.Position);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator GetNodeFromPosition_TopLeft() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;

            Vector3 searchPos = grid.Grid[0, 2].Position;
            AStarNode node = grid.GetNodeFromPosition(searchPos);

            Assert.AreEqual(searchPos, node.Position);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator GetNodeFromPosition_TopRight() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;

            Vector3 searchPos = grid.Grid[2, 2].Position;
            AStarNode node = grid.GetNodeFromPosition(searchPos);

            Assert.AreEqual(searchPos, node.Position);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator GetNodeWithLowestFCost_OneLowest_GetLowest() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            grid.Grid[0, 0].hCost = 100;
            grid.Grid[0, 1].hCost = 10;
            grid.Grid[1, 0].hCost = 101;
            grid.Grid[1, 1].hCost = 0;

            AStarNode cheapestNode = grid.GetCheapestNode();

            Assert.AreEqual(grid.Grid[0, 1], cheapestNode);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator GetNodeWithLowestFCost_TowLowest_GetLowestHCost() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            grid.Grid[0, 0].hCost = 20;
            grid.Grid[0, 1].hCost = 10;
            grid.Grid[0, 0].gCost = 10;
            grid.Grid[0, 1].gCost = 20;
            grid.Grid[1, 0].hCost = 100;
            grid.Grid[1, 1].hCost = 0;

            AStarNode cheapestNode = grid.GetCheapestNode();

            Assert.AreEqual(grid.Grid[0, 1], cheapestNode);

            TestHelper.DestroyObjects(grid.gameObject);
        }
    }
}