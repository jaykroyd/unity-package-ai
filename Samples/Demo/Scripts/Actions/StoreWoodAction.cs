using Elysium.AI.GOAP;
using Elysium.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.AI.Samples.GOAP
{
    public class StoreWoodAction : ChanneledAnimateActionBase
    {
        private Transform agent = default;
        private TextPopup popup = default;

        public override IEnumerable<IRequirement> PreConditions { get; } = new List<IRequirement>();
        public override IEnumerable<IRequirement> PostEffects { get; } = new List<IRequirement>();

        public StoreWoodAction(IAnimator _animator, IAnimation _animation, IProgressBar _progressBar, Transform _agent, Transform _chest, TextPopup _popup) : base(_animator, _animation, _progressBar)
        {
            this.agent = _agent;
            this.popup = _popup;

            PreConditions = new List<IRequirement>() { new PositionRequirement("next_to_chest", _chest.position), new ResourceRequirement("log", 1) };
            PostEffects = new List<IRequirement>() { new ResourceRequirement("wood", 1) };
        }

        protected override void OnAnimationEnd(IGoapAgent _agent)
        {
            _agent.OwnedRequirements.Remove(new ResourceRequirement("log", 1));

            SpawnPopup("+1 Wood", Color.black);
            SpawnPopup("-1 Log", Color.black, 0.5f);
            base.OnAnimationEnd(_agent);
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
