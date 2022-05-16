using System;
using System.Collections.Generic;

namespace Elysium.AI.GOAP
{
    public interface IRequirement : ICloneable
    {
        string Tag { get; }

        bool IsSatisfiedBy(IEnumerable<IRequirement> _requirementCollection);
        void AddTo(IRequirementCollection _requirements);
        void DeductFrom(IRequirementCollection _requirements);
    }
}