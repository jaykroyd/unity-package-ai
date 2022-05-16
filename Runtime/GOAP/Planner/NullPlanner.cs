using System.Collections.Generic;

namespace Elysium.AI.GOAP
{
    public class NullPlanner : IPlanner 
    {
        public bool TryCreatePlan(IGoal _goal, IEnumerable<IAction> _actions, IEnumerable<IRequirement> _ownedRequirements, out Queue<IAction> _plan)
        {
            _plan = new Queue<IAction>();
            return false;
        }
    }
}
