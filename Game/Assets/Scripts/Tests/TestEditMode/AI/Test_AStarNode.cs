using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.AI;

namespace Tests.EditMode.AI
{
    public class Test_AStarNode
    {
        [Test]
        public void Test_AStarNodeSimplePasses() {
            Assert.IsTrue(true);
        }

        [Test]
        public void Clear_ClearFCost_0() {
            AStarNode node = new AStarNode(true, Vector2.zero);
            node.gCost = 200;
            node.hCost = 200;

            node.Clear();

            Assert.AreEqual(0, node.fCost);
        }

        [Test]
        public void Clear_LastNodeInPath_Null() {
            AStarNode node = new AStarNode(true, Vector2.zero);
            node.LastNodeInPath = new AStarNode(true, Vector2.zero);

            node.Clear();

            Assert.IsNull(node.LastNodeInPath);
        }

        [Test]
        public void Clear_AllNeighborsAreDiscovered_False() {
            AStarNode node = new AStarNode(true, Vector2.zero);
            node.AllNeighborsAreDiscovered = true;

            node.Clear();

            Assert.IsFalse(node.AllNeighborsAreDiscovered);
        }
    }
}