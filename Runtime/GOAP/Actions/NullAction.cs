using System.Collections.Generic;
using UnityEngine.Events;

namespace Elysium.AI.GOAP
{
    public class NullAction : IAction
    {        
        public float Cost { get; } = 0f;
        public IEnumerable<IRequirement> PreConditions { get; private set; } = new List<IRequirement>();
        public IEnumerable<IRequirement> PostEffects { get; } = new List<IRequirement>();

        public event UnityAction OnEnd = delegate { };

        public bool IsUsable()
        {
            return false;
        }

        public bool Run(IGoapAgent _agent)
        {
            return false;
        }

        public bool Interrupt()
        {
            return true;
        }
    }
}