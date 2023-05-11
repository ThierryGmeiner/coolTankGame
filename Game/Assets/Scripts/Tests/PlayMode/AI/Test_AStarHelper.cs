using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.AI;

namespace Tests.PlayMode
{
    public class Test_AStarHelper
    {
        [UnityTest]
        public IEnumerator Test_AStarWithEnumeratorPasses() {
            Assert.IsTrue(true);
            yield return null;
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsOverNode_True() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = AStarHelper.NodeIsStraightToNeighbor(grid[1, 2], grid[1, 1]);

            Assert.IsTrue(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsUnderNode_True() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = AStarHelper.NodeIsStraightToNeighbor(grid[1, 0], grid[1, 1]);

            Assert.IsTrue(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsLeftNode_True() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = AStarHelper.NodeIsStraightToNeighbor(grid[0, 1], grid[1, 1]);

            Assert.IsTrue(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsRightNode_True() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = AStarHelper.NodeIsStraightToNeighbor(grid[2, 1], grid[1, 1]);

            Assert.IsTrue(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsUpLeftNode_False() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = AStarHelper.NodeIsStraightToNeighbor(grid[0, 2], grid[1, 1]);

            Assert.IsFalse(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsUpRightNode_False() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = AStarHelper.NodeIsStraightToNeighbor(grid[2, 2], grid[1, 1]);

            Assert.IsFalse(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsDownLeftNode_False() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = AStarHelper.NodeIsStraightToNeighbor(grid[0, 0], grid[1, 1]);

            Assert.IsFalse(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsDownRightNode_False() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = AStarHelper.NodeIsStraightToNeighbor(grid[2, 2], grid[1, 1]);

            Assert.IsFalse(isStraight);
        }
        [UnityTest]
        public IEnumerator NodeIsOutsideOfGrid_DontExistInGrid_True() {
            AStarGrid grid = TestHelper.CreateASTarGrid(3, 3);
            AStarNode node = new AStarNode(true, new Vector2(10, 10));

            bool isOutside = AStarHelper.NodeIsOutsideOfGrid(node, grid);
            yield return null;

            Assert.IsTrue(isOutside);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator NodeIsOutsideOfGrid_XtoLow_True() {
            AStarGrid grid = TestHelper.CreateASTarGrid(3, 3);
            AStarNode node = new AStarNode(true, Vector2.zero, new Vector2Int(-5, 1));

            bool isOutside = AStarHelper.NodeIsOutsideOfGrid(node, grid);
            yield return null;

            Assert.IsTrue(isOutside);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator NodeIsOutsideOfGrid_YtoLow_True() {
            AStarGrid grid = TestHelper.CreateASTarGrid(3, 3);
            AStarNode node = new AStarNode(true, Vector2.zero, new Vector2Int(1, -5));

            bool isOutside = AStarHelper.NodeIsOutsideOfGrid(node, grid);
            yield return null;

            Assert.IsTrue(isOutside);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator NodeIsOutsideOfGrid_XToHigh_True() {
            AStarGrid grid = TestHelper.CreateASTarGrid(3, 3);
            AStarNode node = new AStarNode(true, Vector2.zero, new Vector2Int(+5, 1));

            bool isOutside = AStarHelper.NodeIsOutsideOfGrid(node, grid);
            yield return null;

            Assert.IsTrue(isOutside);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator NodeIsOutsideOfGrid_YToHigh_True() {
            AStarGrid grid = TestHelper.CreateASTarGrid(3, 3);
            AStarNode node = new AStarNode(true, Vector2.zero, new Vector2Int(1, +5));

            bool isOutside = AStarHelper.NodeIsOutsideOfGrid(node, grid);
            yield return null;

            Assert.IsTrue(isOutside);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator NodeIsOutsideOfGrid_IsInside_False() {
            AStarGrid grid = TestHelper.CreateASTarGrid(3, 3);
            AStarNode node = new AStarNode(true, Vector2.zero, new Vector2Int(1, 1));

            bool isOutside = AStarHelper.NodeIsOutsideOfGrid(node, grid);
            yield return null;

            Assert.IsFalse(isOutside);

            TestHelper.DestroyObjects(grid.gameObject);
        }


        [UnityTest]
        public IEnumerator CalculateGCost_Horizontal_Add10() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            grid[1, 1].gCost = 10;
            yield return null;

            int cost = AStarHelper.CalculateGCost(grid[0, 1], grid[1, 1]);

            Assert.AreEqual(grid[1, 1].gCost + 10, cost);
        }

        [UnityTest]
        public IEnumerator CalculateGCost_Vertical_Add10() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            grid[1, 1].gCost = 10;
            yield return null;

            int cost = AStarHelper.CalculateGCost(grid[1, 0], grid[1, 1]);

            Assert.AreEqual(grid[1, 1].gCost + 10, cost);
        }

        [UnityTest]
        public IEnumerator CalculateGCost_Diagonal_Add14() {
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            grid[1, 1].gCost = 10;
            yield return null;

            int cost = AStarHelper.CalculateGCost(grid[0, 0], grid[1, 1]);

            Assert.AreEqual(grid[1, 1].gCost + 14, cost);
        }

        [UnityTest]
        public IEnumerator CalculateHCost_CostAlreadySet_ReturnOldCost() {
            AStarGrid grid = TestHelper.CreateASTarGrid(3, 3);
            AStar aStar = new AStar(grid);
            AStarNode node = grid.Grid[0, 0];
            int oldHCost = 20;
            node.hCost = oldHCost;

            yield return null;
            int newHCost = AStarHelper.CalculateHCost(node, aStar.targetNode);

            Assert.AreEqual(oldHCost, newHCost);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator CalculateHCost_xxx_xxx() {
            AStarGrid grid = TestHelper.CreateASTarGrid(10, 10);
            AStar aStar = new AStar(grid);
            AStarNode node = grid.Grid[0, 0];
            int oldHCost = 20;
            node.hCost = oldHCost;

            yield return null;
            int newHCost = AStarHelper.CalculateHCost(node, aStar.targetNode);
            
            Assert.AreEqual(oldHCost, newHCost);

            TestHelper.DestroyObjects(grid.gameObject);
        }
    }
}