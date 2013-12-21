using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Indexes.BPlusTree.UnitTests
{
    [TestClass]
    public class LeafNodeTests
    {
        [TestMethod]
        public void Test_LeafNode_Merge()
        {
            var left = new LeafNode<int, int>(3);
            left.Keys.Add(1);
            left.Values.Add(new List<int>());
            left.Values[0].Add(1);
            left.Keys.Add(2);
            left.Values.Add(new List<int>());
            left.Values[1].Add(2);

            var right = new LeafNode<int, int>(3);
            right.Keys.Add(3);
            right.Values.Add(new List<int>());
            right.Values[0].Add(3);

            left.Merge(right);
            Assert.AreEqual(0, right.Keys.Count);
            Assert.AreEqual(0, right.Values.Count);
            Assert.AreEqual(3, left.Keys.Count);
            Assert.AreEqual(3, left.Values.Count);
        }

        [TestMethod]
        public void Test_LeafNode_Split()
        {
            var node = new LeafNode<int, int>(3);
            for (int i = 1; i <= 3; i++)
            {
                node.Keys.Add(i);
                node.Values.Add(new List<int>());
                node.Values[0].Add(1);
            }
            var split = node.Split() as LeafNode<int, int>;
            Assert.IsNotNull(split);
            Assert.AreEqual(1, node.Keys.Count);
            Assert.AreEqual(1, node.Values.Count);
            Assert.AreEqual(2, split.Keys.Count);
            Assert.AreEqual(2, split.Values.Count);
            Assert.AreSame(node.Next, split);
        }

        [TestMethod]
        public void Test_LeafNode_Insert()
        {
            var node = new LeafNode<int, int>(3);
            for (int i = 1; i <= 3; i++)
                node.Insert(i, i);
            Assert.IsTrue(node.IsFull());
            Assert.AreEqual(1, node.Keys[0]);
            Assert.AreEqual(2, node.Keys[1]);
            Assert.AreEqual(3, node.Keys[2]);
        }

        [TestMethod]
        public void Test_LeafNode_Redistribute_Left()
        {
            var child = new LeafNode<int, int>(4);
            var sibling = new LeafNode<int, int>(4);
            for (int i = 1; i <= 4; i++)
            {
                var node = sibling;                
                if (i <= 1)
                    node = child;                
                    
                node.Keys.Add(i);
                node.Values.Add(new List<int>());
                node.Values[0].Add(i);
            }

            child.Redistribute(sibling, -1);
            Assert.AreEqual(2, child.Keys.Count);
            Assert.AreEqual(2, sibling.Keys.Count);
        }
    }
}
