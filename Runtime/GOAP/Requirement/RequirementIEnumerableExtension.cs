using System.Collections.Generic;
using System.Linq;

namespace Elysium.AI.GOAP
{
    public static class RequirementIEnumerableExtension
    {
        public static IRequirementCollection ToCollection(this IEnumerable<IRequirement> _requirements)
        {
            return new RequirementCollection(_requirements.Select(x => x.Clone() as IRequirement).ToList());
        }
    }
}