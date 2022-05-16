using Elysium.AI.GOAP;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.AI.Tests.GOAP
{
    public class SampleAction : IAction
    {
        public string Name { get; } = default;        
        public float Cost { get; private set; } = 0f;
        public IEnumerable<IRequirement> PreConditions { get; private set; } = new List<IRequirement>();
        public IEnumerable<IRequirement> PostEffects { get; private set; } = new List<IRequirement>();

        public UnityAction OnRun;
        public event UnityAction OnEnd = delegate { };

        public SampleAction(string _name, float _cost, IEnumerable<IRequirement> _requires, IEnumerable<IRequirement> _delivers)
        {
            this.Name = _name;
            this.Cost = _cost;
            this.PreConditions = _requires.ToCollection();
            this.PostEffects = _delivers.ToCollection();            
        }

        public SampleAction(string _name, float _cost, IEnumerable<IRequirement> _requires, IEnumerable<IRequirement> _delivers, UnityAction _onRun)
        {
            this.Name = _name;
            this.Cost = _cost;
            this.PreConditions = _requires.ToCollection();
            this.PostEffects = _delivers.ToCollection();            
            this.OnRun = _onRun;
        }

        public bool IsUsable()
        {
            return true;
        }

        public bool Run(IGoapAgent _agent)
        {
            OnRun?.Invoke();
            OnEnd?.Invoke();
            return true;
        }

        public bool Interrupt()
        {
            return false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{{ {Name} => ");

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