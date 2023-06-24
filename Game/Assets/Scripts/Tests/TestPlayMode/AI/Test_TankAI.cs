using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game.AI;

namespace Tests.PlayMode.AI
{
    public class Test_TankAI
    {
        [Test]
        public void AAA_LoadNewScene() {
            TestHelper.LoadEmptyScene();
            Assert.IsTrue(true);
        }
     
        [UnityTest]
        public IEnumerator Test_TankAIWithEnumeratorPasses() {
            Assert.IsTrue(true);
            yield return null;
        }

        //[UnityTest]
        //public IEnumerator 
    }
}