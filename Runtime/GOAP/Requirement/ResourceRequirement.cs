using Elysium.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.AI.GOAP
{
    public class ResourceRequirement : IRequirement
    {
        private string resourceId = default;
        private int quantity = default;

        public string Tag => resourceId;
        public string ResourceId => resourceId;
        public int Quantity => quantity;
        
        public ResourceRequirement(string _resourceId, int _quantity)
        {
            this.resourceId = _resourceId;
            this.quantity = _quantity;
        }

        public bool IsSatisfiedBy(IEnumerable<IRequirement> _requirements)
        {
            return quantity < 1 || _requirements.Any(x => Equals(x) && (x as ResourceRequirement).quantity >= quantity);
        }

        public void AddTo(IRequirementCollection _requirements)
        {
            IRequirement existing = _requirements.FirstOrDefault(x => x.Equals(this));
            if (existing is null)
            {
                _requirements.Add(Clone() as IRequirement);
            }
            else
            {
                ResourceRequirement resRequirement = existing as ResourceRequirement;
                resRequirement.quantity += quantity;
            }
        }

        public void DeductFrom(IRequirementCollection _requirements)
        {
            for (int i = _requirements.Count - 1; i >= 0; i--)
            {
                ResourceRequirement resRequirement = _requirements[i] as ResourceRequirement;
                if (resRequirement is null || !resRequirement.Equals(this)) { continue; }                
                resRequirement.quantity -= quantity;
                if (resRequirement.quantity <= 0)
                {
                    _requirements.RemoveAt(i);
                }
            }
        }

        public override bool Equals(System.Object _requirement)
        {
            ResourceRequirement requirement = _requirement as ResourceRequirement;
            return requirement != null && requirement.resourceId == resourceId;
        }

        public override int GetHashCode()
        {
            return resourceId.GetHashCode();
        }

        public override string ToString()
        {
            return $"{resourceId} x{quantity}";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}