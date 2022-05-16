using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elysium.AI.GOAP
{
    public class Node : INode
    {
        public INode Parent { get; } = null;
        public IAction Action { get; } = null;
        public float Cost { get; } = default;
        public IEnumerable<IRequirement> AccumulatedPostEffects { get; } = default;

        public Node(INode _parent, float _cost, IEnumerable<IRequirement> _requirements, IAction _action)
        {
            this.Parent = _parent;
            this.Cost = _cost;
            this.AccumulatedPostEffects = _requirements.Select(x => x.Clone() as IRequirement).ToList();
            this.Action = _action;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            INode currentNode = this;
            do
            {
                builder.Insert(0, currentNode.Parent != null ? $" > {currentNode.Action}" : $"{currentNode.Action}");
                currentNode = currentNode.Parent;
            }
            while (currentNode != null);

            return builder.ToString();
        }
    }
}
