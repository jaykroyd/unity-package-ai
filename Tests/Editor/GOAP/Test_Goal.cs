using System.Collections;
using System.Collections.Generic;
using Elysium.AI.GOAP;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Elysium.AI.Tests.GOAP
{
    public class Test_Goal
    {
        [SetUp]
        public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            PlayerPrefs.DeleteAll();
        }

        [Test]
        public void IsMetBy()
        {
            var cases = new (Goal, IEnumerable<IRequirement>, bool)[]
            {
                (new Goal("test", 1, new List<IRequirement>(){ }), new List<IRequirement>(){ }, true),
                (new Goal("test", 1, new List<IRequirement>(){ new ResourceRequirement("resource", 1) }), new List<IRequirement>(){ new ResourceRequirement("resource", 1) }, true),
                (new Goal("test", 1, new List<IRequirement>(){ new ResourceRequirement("resource", 1) }), new List<IRequirement>(){ new ResourceRequirement("resource", 4) }, true),
                (new Goal("test", 1, new List<IRequirement>(){ new ResourceRequirement("resource", 1) }), new List<IRequirement>(){ new ResourceRequirement("resource1", 4) }, false),
                (new Goal("test", 1, new List<IRequirement>(){ new ResourceRequirement("resource", 1) }), new List<IRequirement>(){ new ResourceRequirement("resource1", 4), new ResourceRequirement("resource", 1) }, true),
                (new Goal("test", 1, new List<IRequirement>(){ new ResourceRequirement("resource", 1), new ResourceRequirement("resource1", 23), new ResourceRequirement("resource2", 4) }), new List<IRequirement>(){ new ResourceRequirement("resource1", 555), new ResourceRequirement("resource", 1) }, false),
                (new Goal("test", 1, new List<IRequirement>(){ new ResourceRequirement("resource", 1), new ResourceRequirement("resource1", 23), new ResourceRequirement("resource2", 4) }), new List<IRequirement>(){ new ResourceRequirement("resource2", 34), new ResourceRequirement("resource1", 555), new ResourceRequirement("resource", 1) }, true),
            };

            foreach (var c in cases)
            {
                bool met = c.Item1.IsMetBy(c.Item2);
                Assert.AreEqual(c.Item3, met);
            }
        }
    }
}