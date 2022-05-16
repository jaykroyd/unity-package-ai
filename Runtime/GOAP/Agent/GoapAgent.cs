using Elysium.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.AI.GOAP
{
    public class GoapAgent : IGoapAgent
    {
        protected IUnityLogger logger = new UnityLogger();
        protected IPlanner planner = new AStarPlanner();
        protected IGoal currentGoal = null;
        protected Queue<IAction> currentPlan = new Queue<IAction>();
        protected IAction currentAction = null;

        public IList<IGoal> Goals { get; set; } = new List<IGoal>();
        public IList<IAction> Actions { get; set; } = new List<IAction>();
        public IRequirementCollection OwnedRequirements { get; set; } = null;
        protected bool HasPlan => currentGoal != null;        

        public GoapAgent(IList<IGoal> _goals, IList<IAction> _actions, IList<IRequirement> _ownedRequirements) 
        {
            logger.logEnabled = false;

            this.Goals = _goals;
            this.Actions = _actions;
            this.OwnedRequirements = _ownedRequirements.ToCollection();
        }

        public virtual void Evaluate()
        {
            if (HasPlan)
            {
                // logger.Log($"[Agent] Already has an active plan");
                return; 
            }

            CreateNewPlan();
        }

        public virtual void Recalculate()
        {
            currentAction.Interrupt();
            CreateNewPlan();
        }

        protected virtual void CreateNewPlan()
        {
            IOrderedEnumerable<IGoal> sortedGoals = Goals.OrderBy(x => x.Priority);
            foreach (var goal in sortedGoals)
            {
                if (planner.TryCreatePlan(goal, Actions, OwnedRequirements, out Queue<IAction> _plan))
                {
                    logger.Log($"[Agent] Successfully created plan for goal: {goal}");
                    logger.Log($"[Agent] Plan: {FormatPlan(_plan)}");
                    this.currentGoal = goal;
                    this.currentPlan = _plan;
                    this.currentAction = null;
                    LoadFirstAction();
                    return;
                }
            }

            logger.Log($"[Agent] Failed to create a plan");
        }

        protected virtual void LoadFirstAction()
        {
            LoadAction();
        }

        protected virtual void LoadNextAction()
        {
            currentAction.OnEnd -= LoadNextAction;
            LoadAction();
        }

        protected virtual void LoadAction()
        {
            if (currentPlan.Count == 0)
            {
                logger.Log($"[Agent] No more actions left to run");
                this.currentGoal.Achieve(this);
                this.currentGoal = null;
                this.currentAction = null;
                return;
            }

            currentAction = currentPlan.Dequeue();
            currentAction.OnEnd += LoadNextAction;
            logger.Log($"[Agent] Running next action: {currentAction}");

            if (!currentAction.Run(this))
            {
                logger.Log($"[Agent] Failed to run action: {currentAction}");
                currentAction.OnEnd -= LoadNextAction;
            }
        }        

        private string FormatPlan(Queue<IAction> _plan)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            for (int i = 0; i < _plan.Count(); i++)
            {
                if (i > 0) { builder.Append(" => "); }
                builder.Append($"{_plan.ElementAt(i)}");
            }
            builder.Append(" }");
            return builder.ToString();
        }
    }
}