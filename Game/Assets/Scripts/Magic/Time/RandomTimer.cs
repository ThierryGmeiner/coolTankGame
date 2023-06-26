using System;
using UnityEngine;

namespace Magic
{
    public class RandomTimer : Timer
    {
        private float minStartingTimeSec, maxStartingTimeSec;
        public override event Action OnTimerEnds;

        public void SetupTimer(float minStartingTime, float maxStartingTime, Modes timerMode)
        {
            this.minStartingTimeSec = minStartingTime;
            this.maxStartingTimeSec = maxStartingTime;
            base.timeSec = SetRandomTime();
            base.SetTimerMode(timerMode);
        }

        public float MinSartingTime { get => minStartingTimeSec; }
        public float MaxStartingTime { get => maxStartingTimeSec; }

        private float SetRandomTime() => base.timeSec = UnityEngine.Random.Range(minStartingTimeSec, maxStartingTimeSec);

        public override void Restart() => SetRandomTime();

        protected override void RunTimer_DestroyWhenTimeIsUp()
        {
            base.timeSec -= Time.deltaTime;
            if (base.TimeIsUp()) {
                OnTimerEnds?.Invoke();
                Destroy(this);
            }
        }

        protected override void RunTimer_RestartWhenTimeIsUp()
        {
            timeSec -= Time.deltaTime;
            if (base.TimeIsUp()) {
                OnTimerEnds?.Invoke();
                Restart();
            }
        }

        protected override void RunTimer_ContinuesWhenTimeIsUp()
        {
            base.timeSec -= Time.deltaTime;
            if (base.TimeIsUp())
                OnTimerEnds?.Invoke();
        }
    }
}