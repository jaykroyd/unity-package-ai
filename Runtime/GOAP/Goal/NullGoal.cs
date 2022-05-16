using System.Collections.Generic;

namespace Elysium.AI.GOAP
{
    public class NullGoal : IGoal
    {
        public string Name { get; } = "null";
        public int Priority { get; } = 0;

        public void Achieve(IGoapAgent _agent)
        {
            
        }

        public bool IsMetBy(IEnumerable<IRequirement> _resources)
        {
            return false;
        }
    }
}
