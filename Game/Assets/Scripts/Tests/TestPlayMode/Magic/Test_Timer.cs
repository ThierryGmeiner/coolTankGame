using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Magic;

namespace Tests.PlayMode.Magic
{
    public class Test_Timer
    {
        [Test]
        public void AAA_LoadNewScene() {
            TestHelper.LoadEmptyScene();
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator Update_TimerRuns()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;

            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp, "Test");
            timer.StartTimer();
            yield return null;

            Assert.Greater(startTime, timer.timeSec);
        }

        [UnityTest]
        public IEnumerator SetTimerMode_SetDestroyWhenTimeIsUp_DestroyWhenTimeIsUp()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();

            yield return null;
            timer.SetTimerMode(Timer.Modes.destroyWhenTimeIsUp);

            Assert.AreEqual(Timer.Modes.destroyWhenTimeIsUp, timer.timerMode);
        }

        [UnityTest]
        public IEnumerator SetTimerMode_SetRestartWhenTimeIsUp_RestartWhenTimeIsUp()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();

            yield return null;
            timer.SetTimerMode(Timer.Modes.restartWhenTimeIsUp);

            Assert.AreEqual(Timer.Modes.restartWhenTimeIsUp, timer.timerMode);
        }

        [UnityTest]
        public IEnumerator StartTimer_TiemrGetsActivated()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;

            timer.SetupTimer(startTime, Timer.Modes.restartWhenTimeIsUp, "Test");
            timer.StartTimer();
            yield return new WaitForFrames(2);

            Assert.AreNotEqual(startTime, timer.timeSec);
        }

        [UnityTest]
        public IEnumerator StopTimer_TimerGetsStoped()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;

            timer.SetupTimer(startTime, Timer.Modes.restartWhenTimeIsUp, "Test");
            timer.StartTimer();
            yield return null;
            timer.StopTimer();
            yield return new WaitForFrames(5);

            Assert.AreEqual(startTime, timer.timeSec, 0.1);
        }

        [UnityTest]
        public IEnumerator SetTimer_3_3()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;
            var newTime = 3;

            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp, "Test");
            yield return null;
            timer.SetTime(newTime);

            Assert.AreEqual(newTime, timer.timeSec, 0.1);
        }

        [UnityTest]
        public IEnumerator SetTimer_999999_999999()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;
            var newTime = 999999;

            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp, "Test");
            yield return null;
            timer.SetTime(newTime);

            Assert.AreEqual(newTime, timer.timeSec, 0.1);
        }

        [UnityTest]
        public IEnumerator SetTimer_negative5_negative5()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;
            var newTime = -5;

            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp, "Test");
            yield return null;
            timer.SetTime(newTime);

            Assert.AreEqual(newTime, timer.timeSec, 0.1);
        }

        [UnityTest]
        public IEnumerator ReduceTime_5minus3_2()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;
            var reducingTime = 3;

            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp, "Test");
            yield return null;
            timer.ReduceTime(reducingTime);

            Assert.AreEqual(startTime - reducingTime, timer.timeSec, 0.1);
        }

        [UnityTest]
        public IEnumerator ReduceTime_5minusNegative3_2()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;
            var reducingTime = -3;

            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp, "Test");
            yield return null;
            timer.ReduceTime(reducingTime);

            Assert.AreEqual(startTime - System.Math.Abs(reducingTime), timer.timeSec, 0.1);
        }

        [UnityTest]
        public IEnumerator ReduceTime_5minus10_negative5()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;
            var reducingTime = 10;

            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp, "Test");
            yield return null;
            timer.ReduceTime(reducingTime);

            Assert.AreEqual(startTime - reducingTime, timer.timeSec, 0.1);
        }

        [UnityTest]
        public IEnumerator IncreaseTime_5plus3_8()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;
            var increaseTime = 3;

            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp, "Test");
            yield return null;
            timer.IncreaseTime(increaseTime);

            Assert.AreEqual(startTime + increaseTime, timer.timeSec, 0.1);
        }

        [UnityTest]
        public IEnumerator IncreaseTime_5plusNegative3_2()
        {
            var obj = new GameObject();
            var timer = obj.AddComponent<PlannedTimer>();
            var startTime = 5;
            var increaseTime = -3;

            timer.SetupTimer(startTime, Timer.Modes.destroyWhenTimeIsUp, "Test");
            yield return null;
            timer.IncreaseTime(increaseTime);

            Assert.AreEqual(startTime + increaseTime, timer.timeSec, 0.1);
        }
    }
}