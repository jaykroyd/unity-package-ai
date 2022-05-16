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
    public class Test_RequirementCollection
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
            var t1 = (new List<IRequirement>() { new ResourceRequirement("test", 1) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test", 1) });
            t1.Item1.Add(t1.Item2);
            Assert.AreEqual(2, (t1.Item1.ElementAt(0) as ResourceRequirement).Quantity);

            var t2 = (new List<IRequirement>() { new ResourceRequirement("test", 12) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test", 1) });
            t2.Item1.Add(t2.Item2);
            Assert.AreEqual(13, (t2.Item1.ElementAt(0) as ResourceRequirement).Quantity);

            var t3 = (new List<IRequirement>() { new ResourceRequirement("test", 2) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 1) });
            t3.Item1.Add(t3.Item2);
            Assert.AreEqual(2, (t3.Item1.ElementAt(0) as ResourceRequirement).Quantity);

            var t4 = (new List<IRequirement>() { new ResourceRequirement("test", 2) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 1), new ResourceRequirement("test", 1) });
            t4.Item1.Add(t4.Item2);
            Assert.AreEqual(3, (t4.Item1.ElementAt(0) as ResourceRequirement).Quantity);

            var t5 = (new List<IRequirement>() { new ResourceRequirement("test", 2), new ResourceRequirement("test1", 15) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 2), new ResourceRequirement("test", 1) });
            t5.Item1.Add(t5.Item2);
            Assert.AreEqual(3, (t5.Item1.ElementAt(0) as ResourceRequirement).Quantity);
            Assert.AreEqual(17, (t5.Item1.ElementAt(1) as ResourceRequirement).Quantity);
        }

        [Test]
        public void TestDeduct()
        {
            var t1 = (new List<IRequirement>() { new ResourceRequirement("test", 1) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test", 1) });
            t1.Item1.Deduct(t1.Item2);
            Assert.AreEqual(0, t1.Item1.Count);

            var t2 = (new List<IRequirement>() { new ResourceRequirement("test", 12) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test", 1) });
            t2.Item1.Deduct(t2.Item2);
            Assert.AreEqual(11, (t2.Item1.First() as ResourceRequirement).Quantity);

            var t3 = (new List<IRequirement>() { new ResourceRequirement("test", 2) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 1) });
            t3.Item1.Deduct(t3.Item2);
            Assert.AreEqual(2, (t3.Item1.First() as ResourceRequirement).Quantity);

            var t4 = (new List<IRequirement>() { new ResourceRequirement("test", 2) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 1), new ResourceRequirement("test", 1) });
            t4.Item1.Deduct(t4.Item2);
            Assert.AreEqual(1, (t4.Item1.First() as ResourceRequirement).Quantity);

            var t5 = (new List<IRequirement>() { new ResourceRequirement("test", 2), new ResourceRequirement("test1", 5) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 2), new ResourceRequirement("test", 1) });
            t5.Item1.Deduct(t5.Item2);
            Assert.AreEqual(1, (t5.Item1.First() as ResourceRequirement).Quantity);
            Assert.AreEqual(3, (t5.Item1.ElementAt(1) as ResourceRequirement).Quantity);
        }

        [Test]
        public void TestIsSatisfiedBy()
        {
            var t1 = (new List<IRequirement>() { new ResourceRequirement("test", 1) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test", 1) });
            Assert.True(t1.Item1.IsSatisfiedBy(t1.Item2));

            var t2 = (new List<IRequirement>() { new ResourceRequirement("test", 12) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test", 1) });
            Assert.False(t2.Item1.IsSatisfiedBy(t2.Item2));

            var t3 = (new List<IRequirement>() { new ResourceRequirement("test", 1) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test", 12) });
            Assert.True(t3.Item1.IsSatisfiedBy(t3.Item2));

            var t4 = (new List<IRequirement>() { new ResourceRequirement("test", 1) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 12) });
            Assert.False(t4.Item1.IsSatisfiedBy(t4.Item2));

            var t5 = (new List<IRequirement>() { new ResourceRequirement("test", 1) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 1), new ResourceRequirement("test", 2) });
            Assert.True(t5.Item1.IsSatisfiedBy(t5.Item2));

            var t6 = (new List<IRequirement>() { new ResourceRequirement("test", 2) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 1), new ResourceRequirement("test", 1) });
            Assert.False(t6.Item1.IsSatisfiedBy(t6.Item2));

            var t7 = (new List<IRequirement>() { new ResourceRequirement("test", 1), new ResourceRequirement("test1", 2) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 5), new ResourceRequirement("test", 2) });
            Assert.True(t7.Item1.IsSatisfiedBy(t7.Item2));

            var t8 = (new List<IRequirement>() { new ResourceRequirement("test", 2), new ResourceRequirement("test1", 5) }.ToCollection(), new List<IRequirement>() { new ResourceRequirement("test1", 2), new ResourceRequirement("test", 1) });
            Assert.False(t8.Item1.IsSatisfiedBy(t8.Item2));
        }
    }
}