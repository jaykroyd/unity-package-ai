using Elysium.Core.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Elysium.AI.GOAP
{
    public class RequirementCollection : IRequirementCollection
    {
        public IList<IRequirement> Requirements { get; private set; } = new List<IRequirement>();

        public int Count => Requirements.Count;

        public bool IsReadOnly => Requirements.IsReadOnly;

        public IRequirement this[int index] { get => Requirements[index]; set => Requirements[index] = value; }

        public RequirementCollection(IEnumerable<IRequirement> _requirements)
        {
            this.Requirements = _requirements.ToList();
        }

        public void Add(IEnumerable<IRequirement> _requirementsToAdd)
        {
            foreach (var requirement in _requirementsToAdd)
            {
                requirement.AddTo(this);
            }            
        }

        public void Deduct(IEnumerable<IRequirement> _requirementsToDeduct)
        {
            foreach (var requirement in _requirementsToDeduct)
            {
                requirement.DeductFrom(this);
            }
        }

        public bool IsSatisfiedBy(IEnumerable<IRequirement> _requirements)
        {
            foreach (var requirement in Requirements)
            {
                if (!requirement.IsSatisfiedBy(_requirements))
                {
                    return false;
                }
            }
            return true;
        }

        public int IndexOf(IRequirement _item)
        {
            return Requirements.IndexOf(_item);
        }

        public void Insert(int _index, IRequirement _item)
        {
            Requirements.Insert(_index, _item);
        }

        public void Add(IRequirement _item)
        {
            Requirements.Add(_item);
        }

        public bool Remove(IRequirement _item)
        {
            return Requirements.Remove(_item);
        }

        public void RemoveAt(int _index)
        {
            Requirements.RemoveAt(_index);
        }

        public void Clear()
        {
            Requirements.Clear();
        }

        public bool Contains(IRequirement _item)
        {
            return Requirements.Contains(_item);
        }

        public void CopyTo(IRequirement[] _array, int _arrayIndex)
        {
            Requirements.CopyTo(_array, _arrayIndex);
        }

        public IEnumerator<IRequirement> GetEnumerator()
        {
            return Requirements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return Requirements.Stringify();
        }
    }
}