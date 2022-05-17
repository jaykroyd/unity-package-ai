using Elysium.AI.GOAP;
using Elysium.Core.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.AI.Samples.GOAP
{
    public class PickupAxeAction : AnimateActionBase
    {
        private SampleSlot weaponSlot = default;
        private TextPopup popup = default;
        private GameObject axe = default;
        private Transform agent = default;

        public override IEnumerable<IRequirement> PreConditions { get; } = new List<IRequirement>();
        public override IEnumerable<IRequirement> PostEffects { get; } = new List<IRequirement>();

        public PickupAxeAction(IAnimator _animator, IAnimation _animation, Transform _agent, GameObject _axe, SampleSlot _weaponSlot, TextPopup _popup) : base(_animator, _animation)
        {
            this.weaponSlot = _weaponSlot;
            this.popup = _popup;
            this.axe = _axe;
            this.agent = _agent;

            PreConditions = new List<IRequirement>() { new PositionRequirement("next_to_axe", _axe.transform.position) };
            PostEffects = new List<IRequirement>() { new ResourceRequirement("axe", 1) };
        }

        protected override void OnAnimationEnd(IGoapAgent _agent)
        {
            _agent.OwnedRequirements.Add(new ResourceRequirement("axe", 1));

            axe.SetActive(false);
            weaponSlot.PickUpAxe();            
            SpawnPopup("+1 Axe", Color.black);
            _agent.Actions.Remove(this);
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
