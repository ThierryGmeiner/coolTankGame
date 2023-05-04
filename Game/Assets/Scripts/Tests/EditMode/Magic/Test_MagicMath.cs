using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Magic;

namespace Tests.EditMode
{
    public class Test_MagicMath
    {
        [Test]
        public void Test_MagicMathSimplePasses() {
            Assert.IsTrue(true);
        }

        [Test]
        public void Max_GetMaxValueOfTow() {
            int value1 = 2, value2 = 5;

            float maxValue = MathM.Max(value1, value2);

            Assert.AreEqual(value2, maxValue);
        }

        [Test]
        public void Max_GetMaxValueOfThree() {
            int value1 = 2, value2 = 5, value3 = 10;

            float maxValue = MathM.Max(value1, value2, value3);

            Assert.AreEqual(value3, maxValue);
        }

        [Test]
        public void Max_GetMaxValueOfFour() {
            int value1 = 2, value2 = 5, value3 = 10, value4 = 20;

            float maxValue = MathM.Max(value1, value2, value3, value4);

            Assert.AreEqual(value4, maxValue);
        }
    }
} 