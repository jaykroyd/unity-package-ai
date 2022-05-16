using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.AI.GOAP
{
    public class PositionRequirement : IRequirement
    {
        private Vector3 position = default;
        private string tag = default;

        public string Tag => $"{tag} => Position: {position}";

        public PositionRequirement(string _tag, Vector3 _position)
        {
            this.tag = _tag;
            this.position = _position;
        }

        public void AddTo(IRequirementCollection _requirements)
        {
            for (int i = _requirements.Count - 1; i >= 0; i--)
            {
                if (_requirements[i] is PositionRequirement) 
                {
                    _requirements.RemoveAt(i);
                }                
            }
            _requirements.Add(Clone() as IRequirement);
        }

        public void DeductFrom(IRequirementCollection _requirements)
        {

        }

        public bool IsSatisfiedBy(IEnumerable<IRequirement> _requirements)
        {
            return _requirements.Any(x => Equals(x));
        }

        public override bool Equals(System.Object _requirement)
        {
            PositionRequirement requirement = _requirement as PositionRequirement;
            if (requirement == null) { return false; }
            return requirement.position == position;
        }

        public override int GetHashCode()
        {
            return Tag.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Tag}";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}