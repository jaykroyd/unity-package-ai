using Elysium.Core;
using Elysium.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Elysium.AI.GOAP
{
    public class Goal : IGoal
    {
        private IUnityLogger logger = new UnityLogger();

        public string Name { get; private set; } = default;
        public int Priority { get; private set; } = default;
        private IEnumerable<IRequirement> Requirements { get; set; } = new List<IRequirement>();

        public Goal(string _name, int _priority, IEnumerable<IRequirement> _requirements)
        {
            logger.logEnabled = false;

            this.Name = _name;
            this.Priority = _priority;
            this.Requirements = _requirements;
        }

        public virtual bool IsMetBy(IEnumerable<IRequirement> _accumulatedPostEffects)
        {
            bool satisfies = Requirements.ToCollection().IsSatisfiedBy(_accumulatedPostEffects);
            logger.Log($"[Goal] Do accumulated effects {_accumulatedPostEffects.Stringify()} satisfy conditions for goal {this} (conditions: {Requirements.Stringify()})? Result: {satisfies}");
            return satisfies;
        }

        public virtual void Achieve(IGoapAgent _agent)
        {
            
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{{ {Name} => ");

            builder.Append("Requirements: [ ");
            for (int i = 0; i < Requirements.Count(); i++)
            {
                if (i > 0) { builder.Append(", "); }
                builder.Append($"{Requirements.ElementAt(i)}");
            }
            builder.Append(" ]");

            builder.Append(" }");
            return builder.ToString();
        }
    }
}
