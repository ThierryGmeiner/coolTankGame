using System;
using UnityEngine;

namespace Magic
{
    public class PlannedTimer : Timer
    {
        private float startTimeSec;
        public override event Action OnTimerEnds;

        public float StartTimeSec { get => startTimeSec; }

        public override void Restart() => timeSec = startTimeSec;

        public void SetupTimer(float timeInSeconds, Modes timerMode, string name) {
            this.startTimeSec = timeInSeconds;
            base.timeSec = timeInSeconds;
            base.Name = name;
            SetTimerMode(timerMode);
        }

        protected override void RunTimer_DestroyWhenTimeIsUp() {
            base.timeSec -= Time.deltaTime;
            if (base.TimeIsUp()) {
                OnTimerEnds?.Invoke();
                Destroy(this);
            }
        }

        protected override void RunTimer_RestartWhenTimeIsUp()
        {
            timeSec -= Time.deltaTime;
            if (base.TimeIsUp())
            {
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