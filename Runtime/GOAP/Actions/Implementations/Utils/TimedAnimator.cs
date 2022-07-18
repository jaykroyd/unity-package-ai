using Elysium.Core.Timers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.AI.GOAP
{
    public class TimedAnimator : IAnimator
    {
        private ITimer timer = default;
        private IAnimation animation = default;

        public event UnityAction OnEnd = default;

        public TimeSpan Elapsed => timer != null && animation != null ? TimeSpan.FromSeconds(animation.Duration.TotalSeconds - timer.Elapsed.TotalSeconds) : TimeSpan.Zero;

        public TimedAnimator()
        {
            timer = Timer.CreateScaledTimer();
            timer.ID = $"timed_animator";
            timer.OnEnd.AddListener(EndAnimation);
        }

        public void Animate(IAnimation _animation)
        {
            this.animation = _animation;
            timer.Interval = animation.Duration;
            timer.Restart();
        }

        public bool HasEnded()
        {
            return timer.IsEnded;
        }

        public void Stop()
        {
            this.animation = null;
        }

        private void EndAnimation()
        {
            OnEnd?.Invoke();
            Stop();
        }

        ~TimedAnimator()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }
    }
}