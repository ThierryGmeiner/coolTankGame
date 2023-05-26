using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Magic;

namespace Tests.PlayMode.Magic
{
    public class Test_PlanedTimer
    {
        [UnityTest]
        public IEnumerator SetupTimer_SetTimeTo5_TimeIs5()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;
            
            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp);
            yield return null;
            
            Assert.AreEqual(startTime, timer.time);
        }
 
        [UnityTest]
        public IEnumerator SetupTimer_SetTimeToNegative5_TimeIsNegative5()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = -5;
            
            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp);
            yield return null;
            
            Assert.AreEqual(startTime, timer.time);
        }

        [UnityTest]
        public IEnumerator DestroyWhenTimeIsUp_DestroyTimer()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = -5;
            
            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp);
            timer.StartTimer();
            yield return null;
            yield return null;

            Assert.IsNull(obj.GetComponent<PlannedTimer>());
        }

        [UnityTest]
        public IEnumerator DestroyWhenTimeIsUp_FiresEvent()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = -5;
            bool testBool = false;
            
            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp);
            timer.OnTimerEnds += () => testBool = true;
            timer.StartTimer();
            yield return null;

            Assert.IsTrue(testBool);
        }

        [UnityTest]
        public IEnumerator RestartWhenTimeIsUp_DoesentDestroyTimer()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = -5;
            
            timer.SetupTimer(startTime, Timer.Modes.restartWhenTimeIsUp);
            timer.StartTimer();
            yield return null;

            Assert.IsNotNull(obj.GetComponent<PlannedTimer>());
        }

        [UnityTest]
        public IEnumerator RestartWhenTimeIsUp_FiresEvent()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = -5;
            bool testBool = false;
            
            timer.SetupTimer(startTime, Timer.Modes.restartWhenTimeIsUp);
            timer.OnTimerEnds += () => testBool = true;
            timer.StartTimer();
            yield return null;

            Assert.IsTrue(testBool);
        }

        [UnityTest]
        public IEnumerator ContinuesWhenTimeIsUp_TimerContinuesRuning()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = -5;
            
            timer.SetupTimer(startTime, Timer.Modes.ConitinuesWhenTimeIsUp);
            timer.StartTimer();
            yield return null;

            Assert.Greater(startTime, timer.time);
        }

        [UnityTest]
        public IEnumerator ContinuesWhenTimeIsUp_FiresEvent()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = -5;
            bool testBool = false;
            
            timer.SetupTimer(startTime, Timer.Modes.ConitinuesWhenTimeIsUp);
            timer.OnTimerEnds += () => testBool = true;
            timer.StartTimer();
            yield return null;

            Assert.IsTrue(testBool);
        }
    }
}