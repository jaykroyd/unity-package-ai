using System;

namespace Elysium.AI.GOAP
{
    public class TimedAnimation : IAnimation
    {
        public TimeSpan Duration { get; private set; }

        public TimedAnimation(TimeSpan _duration)
        {
            this.Duration = _duration;
        }
    }
}