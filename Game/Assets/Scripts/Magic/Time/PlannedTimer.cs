using System;
using System.Threading;
using UnityEngine;

namespace Magic
{
    public class PlannedTimer : Timer
    {
        private float startTimeInSeconds;
        public override event Action OnTimerEnds;

        public void SetupTimer(float timeInSeconds, Modes timerMode)
        {
            this.startTimeInSeconds = timeInSeconds;
            base.timeInSeconds = timeInSeconds;
            SetTimerMode(timerMode);
        }

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
            if (base.TimeIsUp())
            {
                OnTimerEnds?.Invoke();
                timeInSeconds = startTimeInSeconds;
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