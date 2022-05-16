using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using Elysium.AI.GOAP;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace Elysium.AI.Tests.GOAP
{
    public class Test_Planner
    {
        [SetUp]
        public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            PlayerPrefs.DeleteAll();
        }

        [Test]
        public void TestTryCreatePlanEmptyParameters()
        {
            IGoal goal = new Goal("generic", 1, new List<IRequirement>() { new ResourceRequirement("achievement00", 1) });

            IPlanner[] planners = new IPlanner[]
            {
                new AStarPlanner(),
            };

            foreach (var planner in planners)
            {
                bool success = planner.TryCreatePlan(goal, new IAction[0], new IRequirement[0], out Queue<IAction> _plan);
                Assert.False(success);
                Assert.Zero(_plan.Count());
            }
        }

        [Test]
        public void TestTryCreatePlanSuccess()
        {
            IPlanner[] planners = new IPlanner[]
            {
                new AStarPlanner(),
            };

            var cases = new (IGoal, IAction[])[]
            {
                (
                    new Goal("generic", 1, new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }),
                    new IAction[]
                    {
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { },
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }
                        ),
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { },
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }
                        ),
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { },
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }
                        ),
                    }
                ),
                (
                    new Goal("generic", 1, new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }),
                    new IAction[]
                    {
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { },
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }
                        ),
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1)},
                            new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }
                        ),
                    }
                ),
                (
                    new Goal("generic", 1, new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }),
                    new IAction[]
                    {
                        new SampleAction(
                            "generic",
                            1f,
                            new List<IRequirement>() { },
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }
                        ),
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) },
                            new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }
                        ),
                        new SampleAction(
                            "generic",
                            2.5f,
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) },
                            new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }
                        ),
                    }
                ),
                (
                    new Goal("generic", 1, new List<IRequirement>() { new ResourceRequirement("achievement03", 1) }), 
                    new IAction[] 
                    {
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { },
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }
                        ),
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) },
                            new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }
                        ),
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { new ResourceRequirement("achievement01", 1) },
                            new List<IRequirement>() { new ResourceRequirement("achievement02", 1) }
                        ),
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { new ResourceRequirement("achievement02", 1) },
                            new List<IRequirement>() { new ResourceRequirement("achievement03", 1) }
                        ),
                    }
                ),
            };

            foreach (var planner in planners)
            {
                foreach (var c in cases)
                {
                    bool success = planner.TryCreatePlan(c.Item1, c.Item2, new IRequirement[0], out Queue<IAction> _plan);
                    Assert.True(success);
                    Assert.NotZero(_plan.Count());
                }                
            }
        }

        [Test]
        public void TestTryCreatePlanFail()
        {
            IPlanner[] planners = new IPlanner[]
            {
                new AStarPlanner(),
            };

            var cases = new (IGoal, IAction[])[]
            {
                (
                    new Goal("generic", 1, new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }),
                    new IAction[]
                    {
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) },
                            new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }
                        ),
                    }
                ),
                (
                    new Goal("generic", 1, new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }),
                    new IAction[]
                    {
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { },
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }
                        ),
                    }
                ),
                (
                    new Goal("generic", 1, new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }),
                    new IAction[]
                    {
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { },
                            new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }
                        ),
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { },
                            new List<IRequirement>() { new ResourceRequirement("achievement02", 1) }
                        ),
                        new SampleAction(
                            "generic",
                             0f,
                            new List<IRequirement>() { new ResourceRequirement("achievement02", 1) },
                            new List<IRequirement>() { new ResourceRequirement("achievement04", 1) }
                        ),
                    }
                ),
            };

            foreach (var planner in planners)
            {
                foreach (var c in cases)
                {
                    bool success = planner.TryCreatePlan(c.Item1, c.Item2, new IRequirement[0], out Queue<IAction> _plan);
                    Assert.False(success);
                    Assert.Zero(_plan.Count());
                }                
            }
        }

        [Test]
        public void TestCheapestPath()
        {
            IPlanner[] planners = new IPlanner[]
            {
                new AStarPlanner(),
            };

            IAction action1 = new SampleAction(
                "action1",
                1f,
                new List<IRequirement>() { },
                new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }
            );

            IAction action2 = new SampleAction(
                "action2",
                1f,
                new List<IRequirement>() { new ResourceRequirement("achievement00", 1) },
                new List<IRequirement>() { new ResourceRequirement("achievement01", 1) }
            );

            IAction action3 = new SampleAction(
                "action3",
                1f,
                new List<IRequirement>() { new ResourceRequirement("achievement01", 1) },
                new List<IRequirement>() { new ResourceRequirement("achievement02", 1) }
            );

            IAction action4 = new SampleAction(
                "action4",
                2f,
                new List<IRequirement>() { },
                new List<IRequirement>() { new ResourceRequirement("achievement00", 1) }
            );

            IAction action5 = new SampleAction(
                "action5",
                1.1f,
                new List<IRequirement>() { new ResourceRequirement("achievement01", 1) },
                new List<IRequirement>() { new ResourceRequirement("achievement02", 1) }
            );

            var cases = new (IGoal, IAction[], IAction[])[]
            {
                (
                    new Goal("generic", 1, new List<IRequirement>() { new ResourceRequirement("achievement02", 1) }),
                    new IAction[]
                    {
                        action1,
                        action4,
                        action2,
                        action5,
                        action3,
                    },
                    new IAction[]
                    {
                        action1,
                        action2,
                        action3,
                    }
                ),
            };

            foreach (var planner in planners)
            {
                foreach (var c in cases)
                {
                    bool success = planner.TryCreatePlan(c.Item1, c.Item2, new IRequirement[0], out Queue<IAction> _plan);
                    Assert.True(success);
                    Assert.AreEqual(3f, _plan.Sum(x => x.Cost));
                    for (int i = 0; i < c.Item3.Length; i++)
                    {
                        IAction expected = c.Item3[i];
                        IAction actual = _plan.ElementAt(i);
                        Assert.AreEqual(expected, actual);
                    }
                }
            }
        }
    }
}