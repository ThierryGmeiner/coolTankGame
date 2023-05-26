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
            base.time = SetRandomTime();
            base.SetTimerMode(timerMode);
        }

        private float SetRandomTime() => base.time = UnityEngine.Random.Range(minStartingTime, maxStartingTime);

        protected override void RunTimer_DestroyWhenTimeIsUp()
        {
            base.time -= Time.deltaTime;
            if (base.TimeIsUp()) {
                OnTimerEnds?.Invoke();
                Destroy(this);
            }
        }

        protected override void RunTimer_RestartWhenTimeIsUp()
        {
            time -= Time.deltaTime;
            if (base.TimeIsUp()) {
                OnTimerEnds?.Invoke();
                base.time = SetRandomTime();
            }
        }

        protected override void RunTimer_ContinuesWhenTimeIsUp()
        {
            base.time -= Time.deltaTime;
            if (base.TimeIsUp())
                OnTimerEnds?.Invoke();
        }
    }
}