using Elysium.AI.GOAP;
using Elysium.Core;
using Elysium.Core.Utils;
using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Elysium.AI.Samples.GOAP
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SampleAgent : MonoBehaviour, IUpdater
    {
        [SerializeField] private SampleProgressBar progressBar = default;
        [SerializeField] private SampleSlot weaponSlot = default;
        [SerializeField] private TextPopup textPopup = default;
        [SerializeField] private Transform axe = default;       
        [SerializeField] private Transform chest = default;
        [SerializeField] private Transform anvil = default;
        [SerializeField] private SampleTree[] trees = new SampleTree[0];

        private IGoapAgent goapAgent = default;
        private NavMeshAgent navAgent = default;
        private bool updating = true;
        private float evaluateInterval = 2f;
        private float evaluateTimer = 0f;


        public event UnityAction<float> OnUpdate = delegate { };

        private void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.stoppingDistance = 2f;
        }

        private void Start()
        {
            INavigator navMeshNavigator = new NavMeshAgentNavigator(navAgent);
            IAnimator animator = new TimedAnimator();
            IAnimation pickupAxeAnimation = new TimedAnimation(TimeSpan.FromSeconds(0.1f));
            IAnimation cutTreeAnimation = new TimedAnimation(TimeSpan.FromSeconds(1));
            IAnimation repairAxeAnimation = new TimedAnimation(TimeSpan.FromSeconds(3));
            IAnimation storeWoodAnimation = new TimedAnimation(TimeSpan.FromSeconds(1));

            var goals = new List<IGoal>()
            {
                new Goal("Collect Wood", 1, new List<IRequirement>(){ new ResourceRequirement("wood", 1) }),
            };

            var actions = new List<IAction>()
            {
                new MoveToAxeAction(navMeshNavigator, axe),
                new PickupAxeAction(animator, pickupAxeAnimation, transform, axe.gameObject, weaponSlot, textPopup),
                new MoveToTreeAction(navMeshNavigator, trees),
                new CutTreeAction(animator, cutTreeAnimation, progressBar, transform, trees, weaponSlot, textPopup),
                new MoveToChestAction(navMeshNavigator, chest),
                new StoreWoodAction(animator, storeWoodAnimation, progressBar, transform, chest, textPopup),
                new MoveToAnvilAction(navMeshNavigator, anvil),
                new RepairAxeAction(animator, repairAxeAnimation, progressBar, transform, anvil, weaponSlot, textPopup),
            };

            var resources = new List<IRequirement>();
            goapAgent = new GoapAgent(goals, actions, resources);
        }

        void IUpdater.Start()
        {
            updating = true;
        }

        void IUpdater.Stop()
        {
            updating = false;
        }

        void Update()
        {
            if (updating) { OnUpdate?.Invoke(Time.unscaledDeltaTime); }

            evaluateTimer += Time.unscaledDeltaTime;
            if (evaluateTimer >= evaluateInterval)
            {
                evaluateTimer = 0;
                goapAgent.Evaluate();
            }            
        }
    }
}
