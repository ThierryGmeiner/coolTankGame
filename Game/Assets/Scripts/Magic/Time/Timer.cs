using System;
using UnityEngine;

namespace Magic
{
    public abstract class Timer : MonoBehaviour
    {
        protected bool timerIsActive = false;
        protected Action RunTimer;

        public string Name { get; protected set; }
        public float timeSec { get; protected set; }
        public Modes timerMode { get; protected set; }

        public abstract event Action OnTimerEnds;

        protected abstract void RunTimer_DestroyWhenTimeIsUp();
        protected abstract void RunTimer_RestartWhenTimeIsUp();
        protected abstract void RunTimer_ContinuesWhenTimeIsUp();

        public enum Modes
        {
            destroyWhenTimeIsUp,
            restartWhenTimeIsUp,
            ConitinuesWhenTimeIsUp
        }

        private void Update() {
            if (timerIsActive)
                RunTimer();
        }

        public void SetTimerMode(Modes timerMode) {
            this.timerMode = timerMode;
            switch (timerMode) {
                case Modes.destroyWhenTimeIsUp:
                RunTimer = RunTimer_DestroyWhenTimeIsUp;
                break;
                case Modes.restartWhenTimeIsUp:
                RunTimer = RunTimer_RestartWhenTimeIsUp;
                break;
                case Modes.ConitinuesWhenTimeIsUp:
                RunTimer = RunTimer_ContinuesWhenTimeIsUp;
                break;
                default:
                RunTimer = RunTimer_ContinuesWhenTimeIsUp;
                Debug.Log($"{nameof(Timer)}.{nameof(SetTimerMode)}: {nameof(timerMode)} not found.");
                Debug.Log($"{nameof(RunTimer)} was set to {nameof(RunTimer_ContinuesWhenTimeIsUp)}");
                break;
            }
        }

        public void StartTimer() => timerIsActive = true;
        public void StopTimer() => timerIsActive = false;

        public abstract void Restart();
        public void SetTime(float time) => this.timeSec = time;
        public void ReduceTime(float reducedTime) => timeSec -= Math.Abs(reducedTime);
        public void IncreaseTime(float increasedTime) => timeSec += increasedTime;

        protected bool TimeIsUp() => timeSec <= 0;
    }
}