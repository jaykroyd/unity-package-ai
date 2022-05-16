using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Elysium.AI.GOAP;
using Elysium.Core.Utils;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Elysium.AI.Tests.GOAP
{
    public class Test_ResourceRequirement
    {
        [SetUp]
        public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            PlayerPrefs.DeleteAll();
        }

        [Test]
        public void TestAdd()
        {
            var cases = new (ResourceRequirement, IRequirementCollection, IRequirementCollection)[]
            {
                (
                    new ResourceRequirement("test", 1), 
                    new List<IRequirement>(){ new ResourceRequirement("test", 1) }.ToCollection(), 
                    new List<IRequirement>(){ new ResourceRequirement("test", 2) }.ToCollection()
                ),
                (
                    new ResourceRequirement("test", 4), 
                    new List<IRequirement>(){ new ResourceRequirement("test", 12) }.ToCollection(), 
                    new List<IRequirement>(){ new ResourceRequirement("test", 16) }.ToCollection()
                ),
                (
                    new ResourceRequirement("test1", 4), 
                    new List<IRequirement>(){ new ResourceRequirement("test2", 12) }.ToCollection(), 
                    new List<IRequirement>(){ new ResourceRequirement("test1", 4), new ResourceRequirement("test2", 12) }.ToCollection()
                ),
                (
                    new ResourceRequirement("test1", 2), 
                    new List<IRequirement>(){ new ResourceRequirement("test2", 0) }.ToCollection(), 
                    new List<IRequirement>(){ new ResourceRequirement("test1", 2), new ResourceRequirement("test2", 0) }.ToCollection()
                ),
            };

            foreach (var c in cases)
            {
                c.Item1.AddTo(c.Item2);
                CollectionAssert.AreEquivalent(c.Item3, c.Item2);
            }            
        }

        [Test]
        public void TestDeductThisFrom()
        {
            var cases = new (ResourceRequirement, IRequirementCollection, int)[]
            {
                (new ResourceRequirement("test", 1), new List<IRequirement>(){ new ResourceRequirement("test", 1) }.ToCollection(), -1),
                (new ResourceRequirement("test", 4), new List<IRequirement>(){ new ResourceRequirement("test", 12) }.ToCollection(), 8),
                (new ResourceRequirement("test", 4), new List<IRequirement>(){ new ResourceRequirement("test", 2) }.ToCollection(), -1),
                (new ResourceRequirement("test1", 4), new List<IRequirement>(){ new ResourceRequirement("test2", 2) }.ToCollection(), 2),
                (new ResourceRequirement("test1", 0), new List<IRequirement>(){ new ResourceRequirement("test2", 2) }.ToCollection(), 2),
                (new ResourceRequirement("test", 1), new List<IRequirement>(){ new ResourceRequirement("test", 2), new ResourceRequirement("test1", 552) }.ToCollection(), 1),
                (new ResourceRequirement("test", 1), new List<IRequirement>(){ new ResourceRequirement("test", 1), new ResourceRequirement("test1", 552), new ResourceRequirement("test3", 552) }.ToCollection(), -1),
                (new ResourceRequirement("test", 14), new List<IRequirement>(){ new ResourceRequirement("test", 1), new ResourceRequirement("test1", 552), new ResourceRequirement("test3", 552) }.ToCollection(), -1),
            };

            foreach (var c in cases)
            {
                c.Item1.DeductFrom(c.Item2);

                if (c.Item3 > 0) 
                {
                    int qty = (c.Item2.First() as ResourceRequirement).Quantity;
                    Assert.AreEqual(c.Item3, qty);
                } 
                else
                {
                    Assert.False(c.Item2.Contains(c.Item1));
                }
            }
        }

        [Test]
        public void TestEquals()
        {
            var res1 = new ResourceRequirement("test", 1);
            var res2 = new ResourceRequirement("test", 1);
            var res3 = new ResourceRequirement("test1", 1);

            Assert.True(res1.Equals(res1));
            Assert.True(res1.Equals(res2));
            Assert.False(res1.Equals(res3));
        }

        [Test]
        public void TestIsSatisfiedBy()
        {
            var cases = new (ResourceRequirement, IRequirementCollection, bool)[]
            {
                (new ResourceRequirement("test", 1), new List<IRequirement>(){ new ResourceRequirement("test", 1) }.ToCollection(), true),
                (new ResourceRequirement("test", 4), new List<IRequirement>(){ new ResourceRequirement("test", 12) }.ToCollection(), true),
                (new ResourceRequirement("test", 12), new List<IRequirement>(){ new ResourceRequirement("test", 4) }.ToCollection(), false),
                (new ResourceRequirement("test1", 4), new List<IRequirement>(){ new ResourceRequirement("test", 12) }.ToCollection(), false),
                (new ResourceRequirement("test1", 0), new List<IRequirement>(){ }.ToCollection(), true),
                (new ResourceRequirement("test1", 1), new List<IRequirement>(){ new ResourceRequirement("test2", 2) }.ToCollection(), false),
            };

            foreach (var c in cases)
            {
                bool satisfied = c.Item1.IsSatisfiedBy(c.Item2);
                Debug.Log($"Test Case {c.Item1} => {satisfied}");
                Assert.AreEqual(c.Item3, satisfied);
            }
        }
    }
}