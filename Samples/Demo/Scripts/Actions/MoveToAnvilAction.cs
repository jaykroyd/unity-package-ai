﻿using Elysium.AI.GOAP;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.AI.Samples.GOAP
{
    public class MoveToAnvilAction : NavigateActionBase
    {
        public override IEnumerable<IRequirement> PreConditions { get; } = new List<IRequirement>() { };
        public override IEnumerable<IRequirement> PostEffects { get; } = new List<IRequirement>() { };

        public MoveToAnvilAction(INavigator _nav, Transform _target) : base(_nav, _target)
        {
            PostEffects = new List<IRequirement>() { new PositionRequirement("next_to_anvil", _target.position) };
        }
    }
}
