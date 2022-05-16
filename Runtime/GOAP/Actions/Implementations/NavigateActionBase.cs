using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.AI.GOAP
{
    public abstract class NavigateActionBase : IAction
    {
        protected INavigator nav = default;
        protected IVerifier verifier = default;
        protected Transform target = default;

        public virtual float Cost { get; protected set; } = 0.1f;
        public abstract IEnumerable<IRequirement> PreConditions { get; }
        public abstract IEnumerable<IRequirement> PostEffects { get; }

        public event UnityAction OnEnd = delegate { };

        public NavigateActionBase(INavigator _nav, Transform _target)
        {
            this.nav = _nav;
            this.target = _target;
            this.verifier = new Verifier($"{GetType().Name} navigation_verifier", TimeSpan.FromSeconds(0.1f));
        }

        public virtual bool IsUsable()
        {
            return true;
        }

        public virtual bool Interrupt()
        {
            verifier.Unbind();
            nav.Stop();
            return true;
        }        

        public virtual bool Run(IGoapAgent _agent)
        {
            if (nav.SetDestination(target.position))
            {
                verifier.Bind(nav.HasArrived, () => OnArrival(_agent));
                return true;
            }

            return false;
        }

        protected virtual void OnArrival(IGoapAgent _agent)
        {
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