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
        public IEnumerator Test_AStarWithEnumeratorPasses() {
            Assert.IsTrue(true);
            yield return null;
        }
    }
}