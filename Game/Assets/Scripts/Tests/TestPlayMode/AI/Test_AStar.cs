using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.AI;

namespace Tests.PlayMode.AI
{
    public class Test_AStar
    {
        [Test]
        public void AAA_LoadNewScene() {
            TestHelper.LoadEmptyScene();
            Assert.IsTrue(true);
        }

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
        public IEnumerator FindPath_Test1() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStar aStar = new AStar(grid);

            Path path = aStar.FindPath(grid.Grid[0, 0].Position, grid.Grid[4, 4].Position);

            AStarNode[] expectedPath = { grid.Grid[0, 0], grid.Grid[1, 1], grid.Grid[2, 2], grid.Grid[3, 3], grid.Grid[4, 4] };
            Assert.AreEqual(expectedPath, path.Nodes);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator FindPath_Test2() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStar aStar = new AStar(grid);

            Path path = aStar.FindPath(grid.Grid[4, 2].Position, grid.Grid[0, 0].Position);

            AStarNode[] expectedPath = { grid.Grid[4, 2], grid.Grid[3, 1], grid.Grid[2, 0], grid.Grid[1, 0], grid.Grid[0, 0] };
            Assert.AreEqual(expectedPath, path.Nodes);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator FindPath_Test3() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStar aStar = new AStar(grid);
            grid.Grid[2, 0].IsWalkable = false;
            grid.Grid[2, 1].IsWalkable = false;
            grid.Grid[2, 2].IsWalkable = false;

            Path path = aStar.FindPath(grid.Grid[0, 0].Position, grid.Grid[3, 0].Position);

            AStarNode[] expectedPath = { grid.Grid[0, 0], grid.Grid[1, 1], grid.Grid[1, 2], grid.Grid[2, 3], grid.Grid[3, 2],
                                        grid.Grid[3, 1], grid.Grid[3, 0] };
            Assert.AreEqual(expectedPath, path.Nodes);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator FindPath_Test4() {
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStar aStar = new AStar(grid);
            grid.Grid[2, 0].IsWalkable = false;
            grid.Grid[2, 1].IsWalkable = false;
            grid.Grid[2, 2].IsWalkable = false;
            grid.Grid[3, 2].IsWalkable = false;
            grid.Grid[4, 2].IsWalkable = false;
            grid.Grid[5, 2].IsWalkable = false;

            Path path = aStar.FindPath(grid.Grid[0, 0].Position, grid.Grid[3, 0].Position);

            AStarNode[] expectedPath = { grid.Grid[0, 0], grid.Grid[1, 1], grid.Grid[1, 2], grid.Grid[2, 3], grid.Grid[3, 3],
                                        grid.Grid[4, 3], grid.Grid[5, 3], grid.Grid[6, 2], grid.Grid[5, 1], grid.Grid[4, 0], grid.Grid[3, 0] };
            Assert.AreEqual(expectedPath, path.Nodes);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator Test_AStarWithEnumeratorPasses() {
            Assert.IsTrue(true);
            yield return null;
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsOverNode_True() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = aStar.NodeIsStraightToNeighbor(grid[1, 2], grid[1, 1]);

            Assert.IsTrue(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsUnderNode_True() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = aStar.NodeIsStraightToNeighbor(grid[1, 0], grid[1, 1]);

            Assert.IsTrue(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsLeftNode_True() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = aStar.NodeIsStraightToNeighbor(grid[0, 1], grid[1, 1]);

            Assert.IsTrue(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsRightNode_True() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = aStar.NodeIsStraightToNeighbor(grid[2, 1], grid[1, 1]);

            Assert.IsTrue(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsUpLeftNode_False() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = aStar.NodeIsStraightToNeighbor(grid[0, 2], grid[1, 1]);

            Assert.IsFalse(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsUpRightNode_False() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = aStar.NodeIsStraightToNeighbor(grid[2, 2], grid[1, 1]);

            Assert.IsFalse(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsDownLeftNode_False() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = aStar.NodeIsStraightToNeighbor(grid[0, 0], grid[1, 1]);

            Assert.IsFalse(isStraight);
        }

        [UnityTest]
        public IEnumerator NodeIsStraightToNeighbor_IsDownRightNode_False() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            yield return null;

            bool isStraight = aStar.NodeIsStraightToNeighbor(grid[2, 2], grid[1, 1]);

            Assert.IsFalse(isStraight);
        }

        [UnityTest]
        public IEnumerator CalculateGCost_Horizontal_Add10() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            grid[1, 1].gCost = 10;
            yield return null;

            float cost = aStar.CalculateGCost(grid[0, 1], grid[1, 1]);

            Assert.AreEqual(grid[1, 1].gCost + 10, cost);
        }

        [UnityTest]
        public IEnumerator CalculateGCost_Vertical_Add10() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            grid[1, 1].gCost = 10;
            yield return null;

            float cost = aStar.CalculateGCost(grid[1, 0], grid[1, 1]);

            Assert.AreEqual(grid[1, 1].gCost + AStar.STRAIGHT_MOVE_COST, cost);
        }

        [UnityTest]
        public IEnumerator CalculateGCost_Diagonal_Add14() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode[,] grid = TestHelper.CreateAStarNodeArray(3, 3);
            grid[1, 1].gCost = 10;
            yield return null;

            float cost = aStar.CalculateGCost(grid[0, 0], grid[1, 1]);

            Assert.AreEqual(grid[1, 1].gCost + AStar.DIAGOANAL_MOVE_COST, cost);
        }

        [UnityTest]
        public IEnumerator CalculateHCost_CostAlreadySet_ReturnOldCost() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStarNode node = grid.Grid[0, 0];
            int oldHCost = 20;
            node.hCost = oldHCost;

            float newHCost = aStar.CalculateHCost(node, aStar.TargetNode);

            Assert.AreEqual(oldHCost, newHCost);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator CalculateHCost_StraightMoves() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStarNode startNode = grid.Grid[9, 0];
            AStarNode targetNode = grid.Grid[0, 0];

            float hCost = aStar.CalculateHCost(startNode, targetNode);

            float requiredHCost = AStar.STRAIGHT_MOVE_COST * 9; // 9 are the required amount of steps
            Assert.AreEqual(requiredHCost, hCost);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator CalculateHCost_DiagonaleMoves() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStarNode startNode = grid.Grid[0, 0];
            AStarNode targetNode = grid.Grid[9, 9];

            float hCost = aStar.CalculateHCost(startNode, targetNode);

            float requiredHCost = AStar.DIAGOANAL_MOVE_COST * 9; // 9 are the required amount of steps
            Assert.AreEqual(requiredHCost, hCost);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator CalculateHCost_DiagonalAndStraight_Test1() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStarNode startNode = grid.Grid[0, 0];
            AStarNode targetNode = grid.Grid[2, 1];

            float hCost = aStar.CalculateHCost(startNode, targetNode);

            float requiredHCost = AStar.DIAGOANAL_MOVE_COST * 1 + AStar.STRAIGHT_MOVE_COST * 1;
            Assert.AreEqual(requiredHCost, hCost);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator CalculateHCost_DiagonalAndStraight_Test2() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStarNode startNode = grid.Grid[0, 0];
            AStarNode targetNode = grid.Grid[2, 5];

            float hCost = aStar.CalculateHCost(startNode, targetNode);

            float requiredHCost = AStar.DIAGOANAL_MOVE_COST * 2 + AStar.STRAIGHT_MOVE_COST * 3;
            Assert.AreEqual(requiredHCost, hCost);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator CalculateHCost_DiagonalAndStraight_Test3() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStarNode startNode = grid.Grid[6, 7];
            AStarNode targetNode = grid.Grid[1, 4];

            float hCost = aStar.CalculateHCost(startNode, targetNode);

            float requiredHCost = AStar.DIAGOANAL_MOVE_COST * 3 + AStar.STRAIGHT_MOVE_COST * 2;
            Assert.AreEqual(requiredHCost, hCost);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator CalculateHCost_DiagonalAndStraight_Test4() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStarNode startNode = grid.Grid[6, 4];
            AStarNode targetNode = grid.Grid[1, 4];

            float hCost = aStar.CalculateHCost(startNode, targetNode);

            float requiredHCost = AStar.DIAGOANAL_MOVE_COST * 0 + AStar.STRAIGHT_MOVE_COST * 5;
            Assert.AreEqual(requiredHCost, hCost);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator CalculateHCost_DiagonalAndStraight_Test5() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarGrid grid = TestHelper.CreateASTarGrid();
            yield return null;
            AStarNode startNode = grid.Grid[6, 0];
            AStarNode targetNode = grid.Grid[1, 7];

            float hCost = aStar.CalculateHCost(startNode, targetNode);

            float requiredHCost = AStar.DIAGOANAL_MOVE_COST * 5 + AStar.STRAIGHT_MOVE_COST * 2;
            Assert.AreEqual(requiredHCost, hCost);

            TestHelper.DestroyObjects(grid.gameObject);
        }

        [UnityTest]
        public IEnumerator GetPathViaBacktracking_Test1() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode node1 = new AStarNode(true, Vector2.zero);
            AStarNode node2 = new AStarNode(true, Vector2.one);
            AStarNode node3 = new AStarNode(true, Vector2.left);
            node3.LastNodeInPath = node2;
            node2.LastNodeInPath = node1;
            yield return null;

            List<AStarNode> path = aStar.GetPath(node1, node3);

            List<AStarNode> expectedPath = new List<AStarNode>();
            expectedPath.Add(node1);
            expectedPath.Add(node2);
            expectedPath.Add(node3);
            Assert.AreEqual(expectedPath, path);
        }

        [UnityTest]
        public IEnumerator GetPathViaBacktracking_Test2() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode node1 = new AStarNode(true, Vector2.zero);
            AStarNode node2 = new AStarNode(true, Vector2.one);
            AStarNode node3 = new AStarNode(true, Vector2.left);
            AStarNode node4 = new AStarNode(true, Vector2.right);
            AStarNode node5 = new AStarNode(true, Vector2.up);
            node5.LastNodeInPath = node4;
            node4.LastNodeInPath = node3;
            node3.LastNodeInPath = node2;
            node2.LastNodeInPath = node1;
            yield return null;

            List<AStarNode> path = aStar.GetPath(node1, node5);

            List<AStarNode> expectedPath = new List<AStarNode>();
            expectedPath.Add(node1);
            expectedPath.Add(node2);
            expectedPath.Add(node3);
            expectedPath.Add(node4);
            expectedPath.Add(node5);
            Assert.AreEqual(expectedPath, path);
        }

        [UnityTest]
        public IEnumerator UpdateNode_UpdateLastNodeInPath() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode node = new AStarNode(true, Vector3.zero, Vector2Int.zero);
            AStarNode updatingNode = new AStarNode(true, Vector3.zero, Vector2Int.up);
            AStarNode targetNode = new AStarNode(true, Vector3.zero, Vector2Int.down);
            updatingNode.gCost = 10;
            yield return null;

            aStar.UpdateNode(node, updatingNode, targetNode);

            Assert.AreEqual(updatingNode, node.LastNodeInPath);
        }

        [UnityTest]
        public IEnumerator UpdateNode_UpdateGCost() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode node = new AStarNode(true, Vector3.zero, Vector2Int.zero);
            AStarNode updatingNode = new AStarNode(true, Vector3.zero, Vector2Int.up);
            AStarNode targetNode = new AStarNode(true, Vector3.zero, Vector2Int.down);
            float oldGCost = node.gCost;
            updatingNode.gCost = 10;
            yield return null;

            aStar.UpdateNode(node, updatingNode, targetNode);

            Assert.AreNotEqual(oldGCost, node.gCost);
        }

        [UnityTest]
        public IEnumerator UpdateNode_UpdateHCost() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode node = new AStarNode(true, Vector3.zero, Vector2Int.zero);
            AStarNode updatingNode = new AStarNode(true, Vector3.back, Vector2Int.down);
            AStarNode targetNode = new AStarNode(true, Vector3.forward, Vector2Int.up);
            float oldHCost = node.hCost;
            updatingNode.gCost = 10;

            aStar.UpdateNode(node, updatingNode, targetNode);
            yield return null;

            Assert.AreNotEqual(oldHCost, node.hCost);
        }

        [UnityTest]
        public IEnumerator UpdateNode_GCostIsUndefinde_Update() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode node = new AStarNode(true, Vector3.zero, Vector2Int.zero);
            AStarNode updatingNode = new AStarNode(true, Vector3.zero, Vector2Int.up);
            AStarNode targetNode = new AStarNode(true, Vector3.zero, Vector2Int.down);
            updatingNode.gCost = 10;
            yield return null;

            aStar.UpdateNode(node, updatingNode, targetNode);

            Assert.AreEqual(updatingNode, node.LastNodeInPath);
        }

        [UnityTest]
        public IEnumerator UpdateNode_GCostIsLower_Update() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode node = new AStarNode(true, Vector3.zero, Vector2Int.zero);
            AStarNode updatingNode = new AStarNode(true, Vector3.zero, Vector2Int.up);
            AStarNode targetNode = new AStarNode(true, Vector3.zero, Vector2Int.down);
            updatingNode.gCost = 10;
            node.gCost = 100;
            yield return null;

            aStar.UpdateNode(node, updatingNode, targetNode);

            Assert.AreEqual(updatingNode, node.LastNodeInPath);
        }

        [UnityTest]
        public IEnumerator UpdateNode_GCostIsHigher_DontUpdate() {
            AStar aStar = new AStar(TestHelper.CreateASTarGrid());
            AStarNode node = new AStarNode(true, Vector3.zero, Vector2Int.zero);
            AStarNode updatingNode = new AStarNode(true, Vector3.zero, Vector2Int.up);
            AStarNode targetNode = new AStarNode(true, Vector3.zero, Vector2Int.down);
            updatingNode.gCost = 10;
            node.gCost = 8;
            yield return null;

            aStar.UpdateNode(node, updatingNode, targetNode);

            Assert.AreEqual(null, node.LastNodeInPath);
        }
    }
}