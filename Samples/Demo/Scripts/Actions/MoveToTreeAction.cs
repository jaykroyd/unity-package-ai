using Elysium.AI.GOAP;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.AI.Samples.GOAP
{
    public class MoveToTreeAction : NavigateActionBase
    {
        private IEnumerable<SampleTree> trees = default;
        private SampleTree selectedTree = default;

        public override IEnumerable<IRequirement> PreConditions { get; } = new List<IRequirement>() { };
        public override IEnumerable<IRequirement> PostEffects { get; } = new List<IRequirement>() { };

        public MoveToTreeAction(INavigator _nav, IEnumerable<SampleTree> _trees) : base(_nav, null)
        {
            this.trees = _trees;

            PostEffects = new List<IRequirement>() { new PositionRequirement("next_to_tree", Vector3.zero) };
        }

        public override bool Run(IGoapAgent _agent)
        {
            selectedTree = trees.Where(x => !x.IsEmpty).OrderBy(x => GetDistanceTo(x.transform.position)).FirstOrDefault();
            if (selectedTree is null) { return false; }
            target = selectedTree.transform;
            return base.Run(_agent);
        }

        private float GetDistanceTo(Vector3 _position)
        {
            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            if (UnityEngine.AI.NavMesh.CalculatePath(nav.transform.position, _position, -1, path))
            {
                Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
                allWayPoints[0] = nav.transform.position;
                allWayPoints[allWayPoints.Length - 1] = _position;
                for (int i = 0; i < path.corners.Length; i++)
                {
                    allWayPoints[i + 1] = path.corners[i];
                }

                float pathLength = 0;
                for (int i = 0; i < allWayPoints.Length - 1; i++)
                {
                    pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
                }

                return pathLength;
            }

            return Vector3.Distance(nav.transform.position, _position);
        }

        protected override void OnArrival(IGoapAgent _agent)
        {            
            base.OnArrival(_agent);
            selectedTree = null;
        }
    }
}
