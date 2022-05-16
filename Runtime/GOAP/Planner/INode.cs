using System.Collections.Generic;

namespace Elysium.AI.GOAP
{
    public interface INode
    {
        INode Parent { get; }
        IAction Action { get; }
        float Cost { get; }
        IEnumerable<IRequirement> AccumulatedPostEffects { get; }
    }
}
