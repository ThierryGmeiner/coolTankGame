using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.AI;

namespace Tests.PlayMode
{
    public class Test_AStarGrid
    {
        [UnityTest]
        public IEnumerator Test_AStarGridWithEnumeratorPasses() {
            yield return null;
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator CreateGrid_GetRightAmountOfNodes() {
            AStarGrid Astar = TestHelper.CreateASTarGrid();
            int expectedLength = 20; // is hardcoden in class as default

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
    }
}