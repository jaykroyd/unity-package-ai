using System;

namespace Elysium.AI.GOAP
{
    public interface IAnimator
    {
        TimeSpan Elapsed { get; }

        bool HasEnded();
        void Animate(IAnimation _animation);
        void Stop();
    }
}