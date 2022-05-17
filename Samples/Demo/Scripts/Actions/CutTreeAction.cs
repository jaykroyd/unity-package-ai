using Elysium.AI.GOAP;
using Elysium.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.AI.Samples.GOAP
{
    public class CutTreeAction : ChanneledAnimateActionBase
    {
        private SampleSlot weaponSlot = default;
        private IEnumerable<SampleTree> trees = default;
        private SampleTree selectedTree = default;
        private Transform agent = default;
        private TextPopup popup = default;
        private float breakChance = 0.3f;

        public override IEnumerable<IRequirement> PreConditions { get; } = new List<IRequirement>();
        public override IEnumerable<IRequirement> PostEffects { get; } = new List<IRequirement>();

        public CutTreeAction(IAnimator _animator, IAnimation _animation, IProgressBar _progressBar, Transform _agent, IEnumerable<SampleTree> _trees, SampleSlot _weaponSlot, TextPopup _popup) : base(_animator, _animation, _progressBar)
        {
            this.weaponSlot = _weaponSlot;
            this.trees = _trees;
            this.agent = _agent;
            this.popup = _popup;

            PreConditions = new List<IRequirement>() { new PositionRequirement("next_to_tree", Vector3.zero), new ResourceRequirement("axe", 1) };
            PostEffects = new List<IRequirement>() { new ResourceRequirement("log", 1) };
        }

        public override bool IsUsable()
        {
            return trees.Any(x => !x.IsEmpty);
        }

        public override bool Run(IGoapAgent _agent)
        {
            selectedTree = trees.Where(x => !x.IsEmpty).OrderBy(x => GetDistanceTo(x.transform.position)).FirstOrDefault();
            if (selectedTree is null) { return false; }
            return base.Run(_agent);
        }

        protected override void OnAnimationEnd(IGoapAgent _agent)
        {
            _agent.OwnedRequirements.Add(new ResourceRequirement("log", 1));

            selectedTree.Chop();
            SpawnPopup("+1 Log", Color.black);
            selectedTree = null;
            if (CheckIfAxeBroke())
            {
                _agent.OwnedRequirements.Remove(new ResourceRequirement("axe", 1));
                _agent.OwnedRequirements.Add(new ResourceRequirement("broken_axe", 1));
                weaponSlot.BreakAxe();
                SpawnPopup("Axe Broke", Color.black, 0.5f);
            }

            base.OnAnimationEnd(_agent);
        }

        private bool CheckIfAxeBroke()
        {
            return UnityEngine.Random.Range(0f, 1f) <= breakChance;
        }

        private float GetDistanceTo(Vector3 _position)
        {
            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            if (UnityEngine.AI.NavMesh.CalculatePath(agent.position, _position, -1, path))
            {
                Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
                allWayPoints[0] = agent.position;
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

            return Vector3.Distance(agent.position, _position);
        }

        protected virtual void SpawnPopup(string _text, Color _color, float _delay = 0f)
        {
            var PopupSpawnPosition = agent.position.SetY(agent.position.y + 2f);
            var PopupStyle = new TextPopup.Style
            {
                Color = _color,
                FontSize = 3,
                SortOrder = 1,
                Format = "{0}",
                Billboard = true,
                FadeInTime = 1f,
                FadeOutTime = 0.7f,
                Delay = _delay,
                Movement = new Vector3(0.2f, 1f) * 3f,
                Offset = Vector3.zero,
                Rotation = Quaternion.identity,
            };
            popup.Create(PopupSpawnPosition, _text, PopupStyle);
        }
    }
}
