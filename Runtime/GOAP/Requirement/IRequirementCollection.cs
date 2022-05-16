using System.Collections.Generic;

namespace Elysium.AI.GOAP
{
    public interface IRequirementCollection : IList<IRequirement>
    {
        IList<IRequirement> Requirements { get; }

        void Add(IEnumerable<IRequirement> _requirementsToDeduct);
        void Deduct(IEnumerable<IRequirement> _requirementsToDeduct);
        bool IsSatisfiedBy(IEnumerable<IRequirement> _requirements);
    }
}