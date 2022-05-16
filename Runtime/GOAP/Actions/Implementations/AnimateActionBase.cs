using Elysium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.AI.GOAP
{
    public abstract class AnimateActionBase : IAction
    {
        private IUnityLogger logger = new UnityLogger();
        protected IAnimator animator = default;
        protected IAnimation animation = default;
        protected IVerifier verifier = default;

        public virtual float Cost { get; protected set; } = 0.1f;
        public abstract IEnumerable<IRequirement> PreConditions { get; }
        public abstract IEnumerable<IRequirement> PostEffects { get; }

        public event UnityAction OnEnd = delegate { };

        public AnimateActionBase(IAnimator _animator, IAnimation _animation)
        {
            logger.logEnabled = false;

            this.animator = _animator;
            this.animation = _animation;
            this.verifier = new Verifier($"{GetType().Name} animation_verifier", TimeSpan.FromSeconds(0.1f));
        }

        public virtual bool IsUsable()
        {
            return true;
        }

        public virtual bool Interrupt()
        {
            verifier.Unbind();
            animator.Stop();
            return true;
        }

        public virtual bool Run(IGoapAgent _agent)
        {
            animator.Animate(animation);
            verifier.Bind(animator.HasEnded, () => OnAnimationEnd(_agent));
            return true;
        }

        protected virtual void OnAnimationEnd(IGoapAgent _agent)
        {
            logger.Log($"[Animate Action] Animation {animation.GetType()} ended");
            Interrupt();
            OnEnd?.Invoke();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{{ {GetType().Name} => ");

            builder.Append("Pre: [ ");
            for (int i = 0; i < PreConditions.Count(); i++)
            {
                if (i > 0) { builder.Append(", "); }
                builder.Append($"{PreConditions.ElementAt(i)}");
            }
            builder.Append(" ], ");

            builder.Append("Post: [ ");
            for (int i = 0; i < PostEffects.Count(); i++)
            {
                if (i > 0) { builder.Append(", "); }
                builder.Append($"{PostEffects.ElementAt(i)}");
            }
            builder.Append(" ]");

            builder.Append(" }");
            return builder.ToString();
        }
    }
}