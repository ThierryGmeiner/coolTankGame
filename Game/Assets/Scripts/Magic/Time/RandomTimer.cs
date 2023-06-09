using System;
using UnityEngine;

namespace Magic
{
    public class RandomTimer : Timer
    {
        private float minStartingTime, maxStartingTime;
        public override event Action OnTimerEnds;

        public void SetupTimer(float minStartingTime, float maxStartingTime, Modes timerMode)
        {
            this.minStartingTime = minStartingTime;
            this.maxStartingTime = maxStartingTime;
            base.timeInSeconds = SetRandomTime();
            base.SetTimerMode(timerMode);
        }

        public float MinSartingTime { get => minStartingTime; }
        public float MaxStartingTime { get => maxStartingTime; }

        private float SetRandomTime() => base.timeInSeconds = UnityEngine.Random.Range(minStartingTime, maxStartingTime);

        protected override void RunTimer_DestroyWhenTimeIsUp()
        {
            base.timeInSeconds -= Time.deltaTime;
            if (base.TimeIsUp()) {
                OnTimerEnds?.Invoke();
                Destroy(this);
            }
        }

        protected override void RunTimer_RestartWhenTimeIsUp()
        {
            timeInSeconds -= Time.deltaTime;
            if (base.TimeIsUp()) {
                OnTimerEnds?.Invoke();
                base.timeInSeconds = SetRandomTime();
            }
        }

        protected override void RunTimer_ContinuesWhenTimeIsUp()
        {
            base.timeInSeconds -= Time.deltaTime;
            if (base.TimeIsUp())
                OnTimerEnds?.Invoke();
        }
    }
}