using System.Collections;
using System.Collections.Generic;

namespace Elysium.AI.GOAP
{
    public interface IGoapAgent : IAgent
    {
        IList<IGoal> Goals { get; set; }
        IList<IAction> Actions { get; set; }
        IRequirementCollection OwnedRequirements { get; set; }
    }
}