using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.AI.GOAP
{
    public interface IPlanner
    {
        bool TryCreatePlan(IGoal _goal, IEnumerable<IAction> _actions, IEnumerable<IRequirement> _ownedRequirements, out Queue<IAction> _plan);
    }
}
