using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Elysium.AI.GOAP;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Elysium.AI.Tests.GOAP
{
    public class Test_Agent
    {
        [SetUp]
        public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            PlayerPrefs.DeleteAll();
        }

        [Test]
        public void TestAgentEvaluate()
        {
            var actions = new Dictionary<string, (IAction, bool)>();
            void AddToDictionary(Dictionary<string, (IAction, bool)> _dictionary, string _name, List<IRequirement> _requirements, List<IRequirement> _deliveries)
            {
                _dictionary.Add(_name, (new SampleAction(_name, 0f, _requirements, _deliveries, delegate { actions[_name] = (actions[_name].Item1, true); }), false));
            }

            AddToDictionary(actions, "fetch_axe", new List<IRequirement>() { }, new List<IRequirement>() { new ResourceRequirement("axe", 1) });
            AddToDictionary(actions, "cut_tree", new List<IRequirement>() { new ResourceRequirement("axe", 1) }, new List<IRequirement>() { new ResourceRequirement("log", 1) });
            AddToDictionary(actions, "do_random_stuff", new List<IRequirement>() { }, new List<IRequirement>() { });
            AddToDictionary(actions, "climb_ladder", new List<IRequirement>() { new ResourceRequirement("ladder", 1) }, new List<IRequirement>() { new ResourceRequirement("roof", 1) });
            AddToDictionary(actions, "save_cat", new List<IRequirement>() { new ResourceRequirement("roof", 1) }, new List<IRequirement>() { new ResourceRequirement("cat", 1) });
            AddToDictionary(actions, "fetch_ladder", new List<IRequirement>() { }, new List<IRequirement>() { new ResourceRequirement("ladder", 1) });
            AddToDictionary(actions, "fetch_lawnmower", new List<IRequirement>() { }, new List<IRequirement>() { new ResourceRequirement("lawnmower", 1) });
            AddToDictionary(actions, "cut_grass", new List<IRequirement>() { new ResourceRequirement("lawnmower", 1) }, new List<IRequirement>() { new ResourceRequirement("cut_lawn", 1) });

            IGoapAgent agent = new GoapAgent(
                new List<IGoal>()
                {
                    new Goal("gather_log", 1, new List<IRequirement>() { new ResourceRequirement("log", 1) })
                },
                actions.Values.Select(x => x.Item1).ToList(),
                new List<IRequirement>() { }
            );

            var expectedActions = new string[]
            {
            "fetch_axe",
            "cut_tree",
            };

            agent.Evaluate();

            foreach (var expression in actions.Where(x => expectedActions.Contains(x.Key)).Select(x => x.Value.Item2).ToList())
            {
                Assert.True(expression);
            }
        }
    }
}