using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Indexes.BPlusTree.UnitTests
{
    [TestClass]
    public class InnerNodeTests
    {
        [TestInitialize]
        public void Initialize_InnerNode_Tests()
        { 
        }

        [TestMethod]
        public void Test_InnerNode_Merge_Test()
        {
            var leftInnerNode = new InnerNode<int, int>(3);
            var leaf = new LeafNode<int, int>(3);
            leaf.Keys.Add(1);
            leaf.Values.Add(new List<int>());
            leaf.Values[0].Add(1);
            leftInnerNode.Children.Add(leaf);

            leaf = new LeafNode<int, int>(3);
            leaf.Keys.Add(2);
            leaf.Values.Add(new List<int>());
            leaf.Values[0].Add(2);
            leftInnerNode.Children.Add(leaf);

            leftInnerNode.Keys.Add(1);

            var rightInnerNode = new InnerNode<int, int>(3);
            leaf = new LeafNode<int, int>(3);
            leaf.Keys.Add(3);
            leaf.Values.Add(new List<int>());
            leaf.Values[0].Add(3);
            rightInnerNode.Children.Add(leaf);

            rightInnerNode.Keys.Add(4);

            leftInnerNode.Merge(rightInnerNode);

            Assert.AreEqual(0, rightInnerNode.Keys.Count);
            Assert.AreEqual(0, rightInnerNode.Children.Count);
            Assert.AreEqual(2, leftInnerNode.Keys.Count);
            Assert.AreEqual(3, leftInnerNode.Children.Count);
        }

        [TestMethod]
        public void Test_InnerNode_Split_Test()
        {
            var innerNode = new InnerNode<int, int>(3);
            
            var leafNode = new LeafNode<int, int>(3);
            leafNode.Keys.Add(1);
            leafNode.Values.Add(new List<int>());
            leafNode.Values[0].Add(1);
            innerNode.Children.Add(leafNode);

            innerNode.Keys.Add(2);

            leafNode = new LeafNode<int, int>(3);
            leafNode.Keys.Add(2);
            leafNode.Values.Add(new List<int>());
            leafNode.Values[0].Add(2);
            innerNode.Children.Add(leafNode);

            innerNode.Keys.Add(3);

            leafNode = new LeafNode<int, int>(3);
            leafNode.Keys.Add(3);
            leafNode.Values.Add(new List<int>());
            leafNode.Values[0].Add(3);
            innerNode.Children.Add(leafNode);

            innerNode.Keys.Add(4);

            leafNode = new LeafNode<int, int>(3);
            leafNode.Keys.Add(4);
            leafNode.Values.Add(new List<int>());
            leafNode.Values[0].Add(4);
            leafNode.Keys.Add(5);
            leafNode.Values.Add(new List<int>());
            leafNode.Values[1].Add(5);
            innerNode.Children.Add(leafNode);

            var split = innerNode.Split() as InnerNode<int, int>;
            Assert.IsNotNull(split);
            Assert.AreEqual(2, innerNode.Children.Count);
            Assert.AreEqual(2, innerNode.Keys.Count);
            Assert.AreEqual(2, split.Children.Count);
            
            // this key will get promoted
            Assert.AreEqual(1, split.Keys.Count);
        }

        [TestMethod]
        public void Test_InnerNode_Redistribute()
        {
            var child = new InnerNode<int, int>(3);
            var sibling = new InnerNode<int, int>(3);

            child.Keys.Add(1);
            var childLeaf = new LeafNode<int, int>(3);
            childLeaf.Keys.Add(1);
            childLeaf.Values.Add(new List<int>());
            childLeaf.Values[0].Add(1);
            child.Children.Add(childLeaf);

            sibling.Keys.Add(2);
            var siblingChild1 = new LeafNode<int, int>(3);
            siblingChild1.Keys.Add(2);
            siblingChild1.Values.Add(new List<int>());
            siblingChild1.Values[0].Add(2);

            siblingChild1.Keys.Add(3);
            siblingChild1.Values.Add(new List<int>());
            siblingChild1.Values[1].Add(3);

            sibling.Children.Add(siblingChild1);

            sibling.Keys.Add(4);
            var siblingChild2 = new LeafNode<int, int>(3);
            sibling.Children.Add(siblingChild2);
        }
    }
}
