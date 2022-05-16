using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.AI.GOAP
{
    public interface IAction
    {
        float Cost { get; }
        IEnumerable<IRequirement> PreConditions { get; }
        IEnumerable<IRequirement> PostEffects { get; }

        event UnityAction OnEnd;

        bool IsUsable();
        bool Run(IGoapAgent _agent);
        bool Interrupt();
    }
}