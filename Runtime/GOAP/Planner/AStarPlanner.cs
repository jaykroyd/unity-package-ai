using Elysium.Core;
using Elysium.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Elysium.AI.GOAP
{
    public class AStarPlanner : IPlanner
    {
        private IUnityLogger logger = new UnityLogger();

        public AStarPlanner()
        {
            logger.logEnabled = false;
        }

        public bool TryCreatePlan(IGoal _goal, IEnumerable<IAction> _ownedActions, IEnumerable<IRequirement> _ownedRequirements, out Queue<IAction> _plan)
        {
            logger.Log("[Planner] Starting plan phase");

            IEnumerable<IAction> usableActions = _ownedActions.Where(x => x.IsUsable());
            logger.Log($"[Planner] Found {usableActions.Count()} usable actions");

            INode startingNode = new Node(null, 0.0f, _ownedRequirements, null);
            IList<INode> leaves = new List<INode>();            
            if (BuildGraph(startingNode, leaves, _goal, usableActions))
            {
                INode selected = GetOptimalLeaf(leaves);
                _plan = new Queue<IAction>(TranscribePlan(selected));
                return true;
            }

            logger.Log($"[Planner] No plans were found", Color.red);
            _plan = new Queue<IAction>();
            return false;            
        }

        private bool BuildGraph(INode _parent, IList<INode> _leaves, IGoal _goal, IEnumerable<IAction> _usableActions)
        {
            logger.Log($"[Planner] Building graph with parent {_parent}");            
            foreach (IAction action in _usableActions)
            {
                logger.Log($"[Planner] Evaluating action {action}");
                if (!SatisfiesPreConditions(action, _parent.AccumulatedPostEffects)) { continue; }

                IRequirementCollection accumulatedPlanPostEffects = GetAccumulatedPostEffectsMinusActionPreConditions(_parent, action);

                INode node = new Node(_parent, _parent.Cost + action.Cost, accumulatedPlanPostEffects, action);
                if (_goal.IsMetBy(accumulatedPlanPostEffects))
                {
                    logger.Log($"[Planner] Plan {node} achieves all goals (Cost: {node.Cost})", Color.green);
                    _leaves.Add(node);
                }
                else
                {
                    logger.Log($"[Planner] There are still pending goals, building next node");
                    List<IAction> subset = _usableActions.Where(x => !x.Equals(action)).ToList();
                    BuildGraph(node, _leaves, _goal, subset);
                }
            }

            return _leaves.Count > 0;
        }

        private bool SatisfiesPreConditions(IAction _action, IEnumerable<IRequirement> _accumulatedPostEffects)
        {
            bool satisfies = _action.PreConditions.ToCollection().IsSatisfiedBy(_accumulatedPostEffects);
            logger.Log($"[Planner] Do accumulated effects {_accumulatedPostEffects.Stringify()} satisfy conditions for action {_action} (conditions: {_action.PreConditions.Stringify()})? Result: {satisfies}");
            return satisfies;
        }

        private IRequirementCollection GetAccumulatedPostEffectsMinusActionPreConditions(INode _parent, IAction _action)
        {
            IRequirementCollection accumulatedEffects = _parent.AccumulatedPostEffects.ToCollection();
            logger.Log($"[Planner] Accumulated parent effects: {accumulatedEffects}");
            accumulatedEffects.Deduct(_action.PreConditions);
            logger.Log($"[Planner] Accumulated effects after action pre condition deductions: {accumulatedEffects}");
            accumulatedEffects.Add(_action.PostEffects);
            logger.Log($"[Planner] Accumulated effects after action post effects additions: {accumulatedEffects}");
            return accumulatedEffects;
        }

        private INode GetOptimalLeaf(IEnumerable<INode> _leaves)
        {
            logger.Log($"[Planner] Selecting optimal plan:", Color.yellow);
            INode optimal = null;
            foreach (var leaf in _leaves)
            {
                logger.Log($"[Planner] Plan {leaf} (Cost: {leaf.Cost}))", Color.yellow);
                if (optimal == null || leaf.Cost < optimal.Cost)
                {
                    optimal = leaf;
                }
            }

            logger.Log($"[Planner] Selected plan {optimal} (Cost: {optimal.Cost}))", Color.green);
            return optimal;
        }

        private IEnumerable<IAction> TranscribePlan(INode _node)
        {
            List<IAction> planList = new List<IAction>();
            INode node = _node;

            while (node != null)
            {
                if (node.Action != null)
                {
                    planList.Insert(0, node.Action);
                }

                node = node.Parent;
            }

            return planList;
        }        
    }
}
