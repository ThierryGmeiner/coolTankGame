using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Magic;

namespace Tests.PlayMode.Magic
{
    public class Test_RandomTimer
    {
        [Test]
        public void AAA_LoadNewScene() {
            TestHelper.LoadEmptyScene();
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator SetupTimer_SetTimeMin5_Max6_IsInBetween()
        {
            GameObject obj = new GameObject();
            RandomTimer timer = obj.AddComponent<RandomTimer>();
            float startTimeMin = 5;
            float startTimeMax = 6;

            timer.SetupTimer(startTimeMin, startTimeMax, Timer.Modes.destroyWhenTimeIsUp);
            yield return null;

            bool TimeIsInBetween = timer.timeInSeconds > startTimeMin && timer.timeInSeconds < startTimeMax;
            Assert.IsTrue(TimeIsInBetween);
        }
        
        [UnityTest]
        public IEnumerator SetupTimer_SetTimeNegaticeMin5_Max6_IsInBetween()
        {
            GameObject obj = new GameObject();
            RandomTimer timer = obj.AddComponent<RandomTimer>();
            float startTimeMin = -6;
            float startTimeMax = -5;

            timer.SetupTimer(startTimeMin, startTimeMax, Timer.Modes.destroyWhenTimeIsUp);
            yield return null;

            bool TimeIsInBetween = timer.timeInSeconds > startTimeMin && timer.timeInSeconds < startTimeMax;
            Assert.IsTrue(TimeIsInBetween);
        }


        [UnityTest]
        public IEnumerator DestroyWhenTimeIsUp_DestroyTimer()
        {
            GameObject obj = new GameObject();
            RandomTimer timer = obj.AddComponent<RandomTimer>();
            float startTimeMin = -6;
            float startTimeMax = -5;

            timer.SetupTimer(startTimeMin, startTimeMax, Timer.Modes.destroyWhenTimeIsUp);
            timer.StartTimer();
            yield return new WaitForFrames(2);

            Assert.IsNull(obj.GetComponent<RandomTimer>());
        }

        [UnityTest]
        public IEnumerator DestroyWhenTimeIsUp_FiresEvent()
        {
            GameObject obj = new GameObject();
            RandomTimer timer = obj.AddComponent<RandomTimer>();
            float startTimeMin = -6;
            float startTimeMax = -5;
            bool testBool = false;

            timer.SetupTimer(startTimeMin, startTimeMax,Timer.Modes.destroyWhenTimeIsUp);
            timer.OnTimerEnds += () => testBool = true;
            timer.StartTimer();
            yield return null;

            Assert.IsTrue(testBool);
        }

        [UnityTest]
        public IEnumerator RestartWhenTimeIsUp_DoesentDestroyTimer()
        {
            GameObject obj = new GameObject();
            RandomTimer timer = obj.AddComponent<RandomTimer>();
            float startTimeMin = -6;
            float startTimeMax = -5;

            timer.SetupTimer(startTimeMin, startTimeMax, Timer.Modes.restartWhenTimeIsUp); 
            timer.StartTimer();
            yield return null;

            Assert.IsNotNull(obj.GetComponent<RandomTimer>());
        }

        [UnityTest]
        public IEnumerator RestartWhenTimeIsUp_FiresEvent()
        {
            GameObject obj = new GameObject();
            RandomTimer timer = obj.AddComponent<RandomTimer>();
            float startTimeMin = -6;
            float startTimeMax = -5;
            bool testBool = false;

            timer.SetupTimer(startTimeMin, startTimeMax, Timer.Modes.restartWhenTimeIsUp);
            timer.OnTimerEnds += () => testBool = true;
            timer.StartTimer();
            yield return null;

            Assert.IsTrue(testBool);
        }

        [UnityTest]
        public IEnumerator ContinuesWhenTimeIsUp_TimerContinuesRuning()
        {
            GameObject obj = new GameObject();
            RandomTimer timer = obj.AddComponent<RandomTimer>();
            float startTimeMin = -6;
            float startTimeMax = -6;

            timer.SetupTimer(startTimeMin, startTimeMax, Timer.Modes.destroyWhenTimeIsUp);
            timer.StartTimer();
            yield return null;

            Assert.Greater(startTimeMin, timer.timeInSeconds);
        }

        [UnityTest]
        public IEnumerator ContinuesWhenTimeIsUp_FiresEvent()
        {
            GameObject obj = new GameObject();
            RandomTimer timer = obj.AddComponent<RandomTimer>();
            float startTimeMin = -6;
            float startTimeMax = -5;
            bool testBool = false;

            timer.SetupTimer(startTimeMin, startTimeMax, Timer.Modes.destroyWhenTimeIsUp);
            timer.OnTimerEnds += () => testBool = true;
            timer.StartTimer();
            yield return null;

            Assert.IsTrue(testBool);
        }
    }
}