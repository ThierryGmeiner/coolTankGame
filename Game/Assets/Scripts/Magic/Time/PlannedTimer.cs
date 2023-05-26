using System;
using UnityEngine;

namespace Magic
{
    public class PlannedTimer : Timer
    {
        private float startingTime;
        public override event Action OnTimerEnds;

        public void SetupTimer(float startingTime, Modes timerMode)
        {
            this.startingTime = startingTime;
            base.time = startingTime;
            base.SetTimerMode(timerMode);
        }

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
            if (base.TimeIsUp())
            {
                OnTimerEnds?.Invoke();
                time = startingTime;
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