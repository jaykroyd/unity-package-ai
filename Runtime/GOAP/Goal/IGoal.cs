using System.Collections;
using System.Collections.Generic;

namespace Elysium.AI.GOAP
{
    public interface IGoal
    {
        string Name { get; }
        int Priority { get; }

        bool IsMetBy(IEnumerable<IRequirement> resources);
        void Achieve(IGoapAgent _agent);
    }
}
