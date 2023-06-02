using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Entity
{
    public class Test_Bullet
    {
        [UnityTest]
        public IEnumerator Test_BulletWithEnumeratorPasses() {
            Assert.IsTrue(true);
            yield return null;
        }
    }
}